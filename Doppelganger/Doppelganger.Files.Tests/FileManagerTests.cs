using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace Doppelganger.Files.Tests
{
    [TestClass]
    public class FileManagerTests
    {
        const string PATH = @".\Resources";
        FileManager fileManager;

        [TestInitialize]
        public void TestInit()
        {
            fileManager = new FileManager(directoryPath: PATH);
        }

        [TestMethod]
        public void Should_Not_Be_Created_NULL_directoryPath()
        {
            Action action = () => new FileManager(directoryPath: null);
            action.Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void Should_Not_Be_Created_Directory_NotFound()
        {
            Action action = () => new FileManager(directoryPath: "some fake path");
            action.Should().Throw<DirectoryNotFoundException>();
        }

        [TestMethod]
        public void GetFiles_Should_Succeed()
        {
            var fileNames = fileManager.GetFiles();

            using (new AssertionScope())
            {
                fileNames.Should().NotBeNull();
                fileNames.Length.Should().Be(2);
            }
        }
    }
}
