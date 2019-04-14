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

                structure.GetImageLibrary("Png7").Should().NotBeNull();
                structure.GetImageLibrary("Png7").ImagesOfTypeCount<NEF>().Should().Be(0);
                structure.GetImageLibrary("Png7").ImagesOfTypeCount<PNG>().Should().Be(2);
            }
        }

        [TestMethod]
        public void BuildStructure_Should_Succeed2()
        {
            RootImageLibrary expectedStructure = GetExpectedRootImageLibrary();
            expectedStructure.FinishDehydrationProcess();
            
            var structure = imageStructureBuilder.BuildStructure(); 

            //var alma = DeHydrateRootImageLibrary(structure);
            structure.Should().Be(expectedStructure);
        }

        private static RootImageLibrary GetExpectedRootImageLibrary()
        {
            RootImageLibrary expectedStructure;

            using (TextReader tr = new StreamReader(new FileStream(Path.Combine(PATH, "ResourcesStructure.json"), FileMode.Open)))
            {
                string expectedOutput = tr.ReadToEnd();
                expectedStructure = HydrateRootImageLibrary<RootImageLibrary>(expectedOutput);
            }

            return expectedStructure;
        }

        private static T HydrateRootImageLibrary<T>(string dehydratedObject)
        {
            if (string.IsNullOrWhiteSpace(dehydratedObject))
            {
                throw new ArgumentException("This can not be a dehydrated object!", nameof(dehydratedObject));
            }

            JsonSerializerSettings settings = GetJSonSerializerSettings();
            T expectedStructure = JsonConvert.DeserializeObject<T>(dehydratedObject, settings);

            return expectedStructure;
        }

        private static string DeHydrateRootImageLibrary<T>(T hydratedObject)
        {
            if (hydratedObject == null)
            {
                throw new ArgumentNullException(nameof(hydratedObject));
            }

            var settings = GetJSonSerializerSettings();

            var dehydratedObject = JsonConvert.SerializeObject(hydratedObject, settings);
            return dehydratedObject;
        }

        private static JsonSerializerSettings GetJSonSerializerSettings()
        {
            return new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All
            };
        }
    }
}
