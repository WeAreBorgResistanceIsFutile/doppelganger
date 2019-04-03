using Doppelganger.Image;
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
            fde = new FileDataExtractor();
        }

        [TestMethod]
        public void GetFileData_Should_Succeed()
        {
            string fileName = "NIK_4062.NEF";
            var fd = GetFileData(fileName);

            using (new AssertionScope())
            {
                fd.Name.Should().Be(fileName);
                fd.Hash.Should().Be(766030301);
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
                    string fileName = "NIK_4062 - Copy.NEF";
                    ValueObjects.Image fd = GetFileData(fileName);
                    
                    fd.Name.Should().Be(fileName);
                    fd.Hash.Should().Be(766030301);
                    fd.ByteCount.Should().Be(30906305);
                }
            }
        }

        private ValueObjects.Image GetFileData(string fileName)
        {                        
            byte[] content = System.IO.File.ReadAllBytes(Path.Combine(PATH, fileName));

            ValueObjects.Image fd = fde.GetFileData(fileName, Path.Combine(PATH), content);
            return fd;
        }

        [TestMethod]
        public void GetFileData_Should_Fail_null_filename()
        {
            Action getFileData = () => fde.GetFileData(null, Path.Combine(PATH), new byte[0]);
            getFileData.Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void GetFileData_Should_Fail_null_filepath()
        {
            Action getFileData = () => fde.GetFileData("some.file", null, new byte[0]);
            getFileData.Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void GetFileData_Should_Fail_null_content()
        {
            Action getFileData = ()=> fde.GetFileData("some.file", Path.Combine(PATH), null);
            getFileData.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]        
        public void GetFileData_Should_Not_Fail_invalid_filename_and_path()
        {
            fde.GetFileData("some.file", Path.Combine(PATH), new byte[0]);            
        }
    }
}
