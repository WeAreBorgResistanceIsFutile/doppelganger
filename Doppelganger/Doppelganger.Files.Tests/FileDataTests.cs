using System;
using System.IO;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Doppelganger.Files.Tests
{
    [TestClass]
    public class FileDataTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            string fileName = "NIK_4062.NEF";
            string filePath = @".\Resources";
            FileDataExtractor fde = new FileDataExtractor();
            byte[] content = File.ReadAllBytes(Path.Combine(filePath, fileName));

            FileData fd = fde.GetFileData(fileName, Path.Combine(filePath), content);

            fd.Name.Should().Be(fileName);
            fd.FullPath.Should().Be(Path.Combine(filePath, fileName));
            fd.Hash.Should().Be(73002467);
        }
    }
}
