using Doppelganger.Image.ValueObjects;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;

namespace Doppelganger.Image.Test
{
    [TestClass]
    public class ImageStructureBuilder
    {
        const string PATH = @".\Resources";
        Image.ImageStructureBuilder imageStructureBuilder;

        [TestInitialize]
        public void TestInit()
        {
            imageStructureBuilder = new Image.ImageStructureBuilder(sourcePath: PATH);
        }

        [TestMethod]
        public void Should_Not_Be_Created_NULL_directoryPath()
        {
            Action action = () => new Image.ImageStructureBuilder(sourcePath: null);
            action.Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void Should_Not_Be_Created_Directory_NotFound()
        {
            Action action = () => new Image.ImageStructureBuilder(sourcePath: "some fake path");
            action.Should().Throw<DirectoryNotFoundException>();
        }

        [TestMethod]
        public void BuildStructure_Should_Succeed()
        {
            var structure = imageStructureBuilder.BuildStructure();

            using (new AssertionScope())
            {
                structure.Should().NotBeNull();
                structure.Should().BeOfType<ImageLibrary>();
                structure.ChildElements.Count.Should().Be(14);
                structure.ChildElements.OfType<ImageLibrary>().Count().Should().Be(7);
                structure.ChildElements.OfType<NEF>().Count().Should().Be(3);
                structure.ChildElements.OfType<PNG>().Count().Should().Be(4);

                structure.ChildElements.OfType<ImageLibrary>().Where(p => p.Name.ToLower().EndsWith("png7")).Should().NotBeNull();
                structure.ChildElements.OfType<ImageLibrary>().Where(p => p.Name.ToLower().EndsWith("png7")).First().ChildElements.OfType<NEF>().Count().Should().Be(0);
                structure.ChildElements.OfType<ImageLibrary>().Where(p => p.Name.ToLower().EndsWith("png7")).First().ChildElements.OfType<PNG>().Count().Should().Be(2);
            }
        }
    }
}
