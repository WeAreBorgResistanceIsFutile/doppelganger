using System.IO;

using Doppelganger.Image.ValueObjects;

using FluentAssertions;
using FluentAssertions.Execution;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Doppelganger.Infrastructure.Serializer.Tests
{
    [TestClass]
    public class ObjectDeserializer
    {
        private const string PATH = @".\Resources";

        [TestMethod]
        public void DeserializeObject_Should_Succeed()
        {
            string dehydratedObject = null;
            using (FileStream fs = new FileStream(Path.Combine(PATH, "ResourcesStructure.json"), FileMode.Open))
            {
                using (TextReader sr = new StreamReader(fs))
                {
                    dehydratedObject = sr.ReadToEnd();
                }
            }

            ObjectDeserializer<RootImageLibrary> objectDeserializer = new ObjectDeserializer<RootImageLibrary>();
            RootImageLibrary structure = objectDeserializer.DeserializeObject(dehydratedObject);
            structure.FinishDehydrationProcess();

            using (new AssertionScope())
            {
                structure.Should().NotBeNull();
                structure.Name.Should().Be("Resources");
                structure.Should().BeOfType<RootImageLibrary>();
                structure.ImageLibraryCount.Should().Be(7);
                structure.ImagesOfTypeCount<NEF>().Should().Be(0);
                structure.ImagesOfTypeCount<PNG>().Should().Be(4);

                string png7Path = Path.Combine(structure.GetPath(), "Png7");

                structure.GetImageLibraryByPath(png7Path).Should().NotBeNull();
                structure.GetImageLibraryByPath(png7Path).ImagesOfTypeCount<NEF>().Should().Be(0);
                structure.GetImageLibraryByPath(png7Path).ImagesOfTypeCount<PNG>().Should().Be(2);
            }
        }
    }
}