using Doppelganger.Image;
using Doppelganger.Image.PHash;
using Doppelganger.Image.ValueObjects;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Doppelganger.Infrastructure.Serializer.Tests
{
    [TestClass]
    public class ObjectSerializer
    {
        const string DEHYDRATEDOBJECT_SHOULD_START_WITH = "{\"$type\":\"Doppelganger.Image.ValueObjects.RootImageLibrary, Doppelganger.Image\",\"ImageLibraryCount\":7,\"ImageCount\":4,\"FileName\":\"Resources\"";
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
        public void SerializeObject_Should_Succeed()
        {
            var structure = imageStructureBuilder.BuildStructure();

            ObjectSerializer<RootImageLibrary> serializer = new ObjectSerializer<RootImageLibrary>();
            string dehydratedObject = serializer.SerializeObject(structure);

            dehydratedObject.Should().StartWith(DEHYDRATEDOBJECT_SHOULD_START_WITH);
            dehydratedObject.Should().HaveLength(18266);
        }
    }
}
