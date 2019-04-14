using System;
using Doppelganger.Image.ImageFormatConverters;
using Doppelganger.Image.PHash;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Doppelganger.Image.Test
{
    [TestClass]
    public class PHashCalculatorTests
    {
        const string FILE = @".\resources\NIK_9586.png";

        [TestMethod]
        public void CalculatePHash_Should_Succeed()
        {
            NefToPngConverter converter = new NefToPngConverter(FILE);
            PHashCalculator calculator = new PHashCalculator();
            var hash = calculator.CalculatePHash(converter.Convert());
            hash[0].Should().Be(123);
            hash[1].Should().Be(119);
        }


        [TestMethod]
        public void ComparePHashes_Should_Succeed()
        {
            NefToPngConverter converter = new NefToPngConverter(FILE);
            PHashCalculator calculator = new PHashCalculator();
            var hash = calculator.CalculatePHash(converter.Convert());
            var hash2 = calculator.CalculatePHash(converter.Convert());
            var result = calculator.CompareHashes(hash, hash2);
            result.Should().Be(1);
        }
    }
}
