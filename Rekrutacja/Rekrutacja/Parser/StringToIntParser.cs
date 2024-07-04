using System;
using System.Linq;

namespace Rekrutacja.Parser
{
    public static class StringToIntParser
    {
        public static int Parse(string input)
        {
            WalidujInput(ref input);

            int wynik = 0;
            bool czyUjemna = input[0] == '-';

            for (int i = czyUjemna ? 1 : 0; i < input.Length; i++)
            {
                char znak = input[i];

                int cyfra = znak - '0';

                if (wynik > (int.MaxValue - cyfra) / 10)
                {
                    throw new OverflowException("Wartość jest zbyt duża, aby zmieścić się w typie int.");
                }

                wynik = wynik * 10 + cyfra;
            }

            return czyUjemna ? -wynik : wynik;
        }

        private static void WalidujInput(ref string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                throw new ArgumentException("Ciąg wejściowy nie może być pusty.");
            }

            input = input.Trim();

            if (input.Any(znak => !char.IsDigit(znak) && znak != '-') || (input.IndexOf('-') > 0))
            {
                throw new FormatException($"Niewłaściwy znak w ciągu wejściowym.");
            }
        }
    }
}
