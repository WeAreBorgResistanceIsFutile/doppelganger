using System;
using System.IO;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Doppelganger.Image.Test
{
    [TestClass]
    public class FileHashCalculatorTests
    {
        const string PATH = @".\Resources";

        [TestMethod]
        public void GetFileHash_SmallFile_Should_Succeed()
        {
            GetSmallFileHash().Should().Be(1743049329);
        }



        [TestMethod]
        public void GetFileHash_LargeFile_Should_Succeed()
        {
            using (var s = new FileStream(Path.Combine(PATH, "NIK_9586.NEF"), FileMode.Open))
            {
                FileHashCalculator.GetFileHash(s).Should().Be(-1076553324);
            }
        }


        [TestMethod]
        public void GetFileHash_Should_Be_Idempotent()
        {
            GetSmallFileHash().Should().Be(GetSmallFileHash());
        }

        private static int GetSmallFileHash()
        {
            using (var s = new FileStream(Path.Combine(PATH, "NIK_9586.png"), FileMode.Open))
            {
                return FileHashCalculator.GetFileHash(s);
            }
        }
    }
}
