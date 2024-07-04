using Soneta.Business;
using System;
using Soneta.Kadry;
using Soneta.Types;
using Rekrutacja.Workers.Template;
using Rekrutacja.Kalkulator;
using Rekrutacja.Enums;
using Rekrutacja.Parser;

[assembly: Worker(typeof(KalkulatorWorker), typeof(Pracownicy))]
namespace Rekrutacja.Workers.Template
{
    public class KalkulatorWorker
    {
        public class KalkulatorWorkerParametry : ContextBase
        {
            [Caption("A")]
            public string A { get; set; }

            [Caption("B")]
            public string B { get; set; }

            [Caption("Data obliczeń")]
            public Date DataObliczen { get; set; }

            [Caption("Operacja")]
            public OperacjaMatematyczna Operacja { get; set; }

            public KalkulatorWorkerParametry(Context context) : base(context)
            {
                this.DataObliczen = Date.Today;
                this.A = string.Empty;
                this.B = string.Empty;
                this.Operacja = OperacjaMatematyczna.Dodaj;
            }
        }

        [Context]
        public Context Cx { get; set; }

        [Context]
        public KalkulatorWorkerParametry Parametry { get; set; }

        [Action("Kalkulator",
           Description = "Prosty kalkulator",
           Priority = 10,
           Mode = ActionMode.ReadOnlySession,
           Icon = ActionIcon.Accept,
           Target = ActionTarget.ToolbarWithText)]
        public void WykonajAkcje()
        {
            DebuggerSession.MarkLineAsBreakPoint();

            var pracownicy = (Pracownik[])Cx[typeof(Pracownik[])];

            if (pracownicy == null || pracownicy.Length == 0)
            {
                throw new InvalidOperationException("Nie zaznaczono pracownika.");
            }

            using (var nowaSesja = this.Cx.Login.CreateSession(false, false, "ModyfikacjaPracownika"))
            {
                using (ITransaction trans = nowaSesja.Logout(true))
                {
                    foreach (var pracownik in pracownicy)
                    {
                        var pracownikZSesja = nowaSesja.Get(pracownik);

                        var parsedA = StringToIntParser.Parse(this.Parametry.A);
                        var parsedB = StringToIntParser.Parse(this.Parametry.B);

                        pracownikZSesja.Features["Wynik"] = KalkulatorEngine.WykonajOpercje(parsedA, parsedB, this.Parametry.Operacja);
                        pracownikZSesja.Features["DataObliczen"] = this.Parametry.DataObliczen;
                    }

                    trans.CommitUI();
                }

                nowaSesja.Save();
            }
        }
    }
}