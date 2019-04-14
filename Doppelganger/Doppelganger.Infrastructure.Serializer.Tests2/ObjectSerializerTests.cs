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

            ObjectSerializer < RootImageLibrary > serializer = new ObjectSerializer<RootImageLibrary>();
            string dehydratedObject = serializer.SerializeObject(structure);

            dehydratedObject.GetHashCode().Should().Be(10);
        }
    }
}
