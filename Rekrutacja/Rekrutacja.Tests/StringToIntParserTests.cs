using NUnit.Framework;
using Rekrutacja.Parser;
using System;

namespace Rekrutacja.Tests
{
    [TestFixture]
    public class StringToIntParserTests
    {
        [Test]
        [TestCase("1", 1)]
        [TestCase("123", 123)]
        [TestCase("-123", -123)]
        [TestCase(" 52", 52)]
        [TestCase(" 52 ", 52)]
        [TestCase("   -52   ", -52)]
        public void Parse_Test(string input, int expectedResult)
        {
            var actualResult = StringToIntParser.Parse(input);

            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        public void Parse_Zbyt_Duza_Wartosc()
        {
            Assert.Throws<OverflowException>(() => StringToIntParser.Parse("123456789123456789"));
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("    ")]
        public void Parse_Pusty_Ciag_Wejsciowy(string input)
        {
            Assert.Throws<ArgumentException>(() => StringToIntParser.Parse(input));
        }

        [Test]
        [TestCase("abc")]
        [TestCase("123a12")]
        [TestCase("43-12")]
        [TestCase("4343,5")]
        [TestCase("4343.5")]
        public void Parse_Niewlasciwy_Znak(string input)
        {
            Assert.Throws<FormatException>(() => StringToIntParser.Parse(input));
        }
    }
}
