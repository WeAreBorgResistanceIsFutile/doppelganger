using System;
using System.IO;

using Doppelganger.Image.PHash;
using Doppelganger.Image.ValueObjects;

using FluentAssertions;
using FluentAssertions.Execution;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

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
            _FileDataExtractor = new FileDataExtractor(new ImageFactory(), new PHashCalculator());
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

                structure.GetImageLibraryByPath("Png7").Should().NotBeNull();
                structure.GetImageLibraryByPath("Png7").ImagesOfTypeCount<NEF>().Should().Be(0);
                structure.GetImageLibraryByPath("Png7").ImagesOfTypeCount<PNG>().Should().Be(2);
            }
        }

        [TestMethod]
        public void UpdateStructure_Should_Fill_Missing_Directory_Succeed()
        {
            var structure = imageStructureBuilder.BuildStructure();

            var imageLibrary = structure.GetImageLibraryByPath("Png7");
            structure.Remove(imageLibrary);

            imageStructureBuilder.UpdateStructure(structure);

            using (new AssertionScope())
            {
                structure.Should().NotBeNull();
                structure.Should().BeOfType<RootImageLibrary>();
                structure.ImageLibraryCount.Should().Be(7);
                structure.ImagesOfTypeCount<NEF>().Should().Be(3);
                structure.ImagesOfTypeCount<PNG>().Should().Be(4);

                structure.GetImageLibraryByPath("Png7").Should().NotBeNull();
                structure.GetImageLibraryByPath("Png7").ImagesOfTypeCount<NEF>().Should().Be(0);
                structure.GetImageLibraryByPath("Png7").ImagesOfTypeCount<PNG>().Should().Be(2);
            }
        }

        [TestMethod]
        public void UpdateStructure_Should_Fill_Missing_PNG_Succeed()
        {
            var structure = imageStructureBuilder.BuildStructure();


            var image = structure.GetImageByFullName(Path.Combine(structure.GetPath(), "NIK_9586.png"));
            structure.Remove(image);
            
            imageStructureBuilder.UpdateStructure(structure);

            using (new AssertionScope())
            {
                structure.Should().NotBeNull();
                structure.Should().BeOfType<RootImageLibrary>();
                structure.ImageLibraryCount.Should().Be(7);
                structure.ImagesOfTypeCount<NEF>().Should().Be(3);
                structure.ImagesOfTypeCount<PNG>().Should().Be(4);

                structure.GetImageLibraryByPath("Png7").Should().NotBeNull();
                structure.GetImageLibraryByPath("Png7").ImagesOfTypeCount<NEF>().Should().Be(0);
                structure.GetImageLibraryByPath("Png7").ImagesOfTypeCount<PNG>().Should().Be(2);
            }
        }
    }
}
