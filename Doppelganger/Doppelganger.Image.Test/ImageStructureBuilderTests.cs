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
        IFileDataExtractor _FileDataExtractor;

        [TestInitialize]
        public void TestInit()
        {
            _FileDataExtractor = new FileDataExtractor(new ImageFactory());
            imageStructureBuilder = new Image.ImageStructureBuilder(sourcePath: PATH, _FileDataExtractor);
        }

        [TestMethod]
        public void Should_Not_Be_Created_NULL_FileDataExtractor()
        {
            Action action = () => new Image.ImageStructureBuilder(sourcePath: PATH, null);
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Should_Not_Be_Created_NULL_directoryPath()
        {
            Action action = () => new Image.ImageStructureBuilder(sourcePath: null, _FileDataExtractor);
            action.Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void Should_Not_Be_Created_Directory_NotFound()
        {
            Action action = () => new Image.ImageStructureBuilder(sourcePath: "some fake path", _FileDataExtractor);
            action.Should().Throw<DirectoryNotFoundException>();
        }

        [TestMethod]
        public void BuildStructure_Should_Succeed()
        {
            var structure = imageStructureBuilder.BuildStructure();

            using (new AssertionScope())
            {
                structure.Should().NotBeNull();
                structure.Should().BeOfType<RootImageLibrary>();
                structure.ImageLibraryCount.Should().Be(7);
                structure.ImagesOfTypeCount<NEF>().Should().Be(3);
                structure.ImagesOfTypeCount<PNG>().Should().Be(4);

                structure.GetImageLibrary("Png7").Should().NotBeNull();
                structure.GetImageLibrary("Png7").ImagesOfTypeCount<NEF>().Should().Be(0);
                structure.GetImageLibrary("Png7").ImagesOfTypeCount<PNG>().Should().Be(2);
            }
        }
    }
}
