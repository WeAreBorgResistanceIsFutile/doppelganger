using Doppelganger.Image;
using Doppelganger.Image.PHash;
using Doppelganger.Image.ValueObjects;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace Doppelganger.Image.Test
{
    [TestClass]
    public class FileDataExtractorTests
    {
        const string PATH = @".\Resources";
        FileDataExtractor fde;

        [TestInitialize]
        public void InitTests()
        {
            fde = new FileDataExtractor(new ImageFactory(), new PHashCalculator());
        }

        [TestMethod]
        public void Create_Should_Fail_null_ImageFactory()
        {
            Action getFileData = () => new FileDataExtractor(null, new PHashCalculator());
            getFileData.Should().Throw<ArgumentNullException>();
        }


        [TestMethod]
        public void Create_Should_Fail_null_PHashCalculator()
        {
            Action getFileData = () => new FileDataExtractor(new ImageFactory(), null);
            getFileData.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void GetFileData_Should_Succeed()
        {
            string fileName = "NIK_4062";
            var fd = GetFileData(Path.Combine(PATH, fileName + ".NEF"));

            using (new AssertionScope())
            {
                fd.Name.Should().Be(fileName);
                fd.Hash.Should().Be(-1331913275);
                fd.ByteCount.Should().Be(30906305);
            }
        }

        [TestMethod]
        public void GetFileData_Should_Succeed_Multiple_Times()
        {
            int howManyTimes = 5;

            using (new AssertionScope())
            {
                for (int i = 0; i < howManyTimes; i++)
                {
                    string fileName = "NIK_9588";
                    ValueObjects.ImageBase fd = GetFileData(Path.Combine(PATH, fileName + ".png"));

                    fd.Name.Should().Be(fileName);
                    fd.Hash.Should().Be(815570373);
                    fd.ByteCount.Should().Be(31459);
                }
            }
        }

        private ValueObjects.ImageBase GetFileData(string fileName)
        {
            ValueObjects.ImageBase fd = fde.GetFileData<NEF>(new FileInfo( fileName));
            return fd;
        }

        [TestMethod]
        public void GetFileData_Should_Fail_null_file()
        {
            Action getFileData = () => fde.GetFileData<NEF>(null);
            getFileData.Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void GetFileData_Should_Fail_invalid_filepath()
        {
            Action action = () => fde.GetFileData<NEF>(new FileInfo("Some file"));
            action.Should().Throw<FileNotFoundException>();
        }       
    }
}
