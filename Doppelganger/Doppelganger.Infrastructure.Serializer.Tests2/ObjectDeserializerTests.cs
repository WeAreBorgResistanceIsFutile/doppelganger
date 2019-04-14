using System.IO;
using Doppelganger.Image;
using Doppelganger.Image.PHash;
using Doppelganger.Image.ValueObjects;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Doppelganger.Infrastructure.Serializer.Tests
{
    [TestClass]
    public class ObjectDeserializer
    {
        const string PATH = @".\Resources";
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

            using (new AssertionScope())
            {
                structure.Should().NotBeNull();
                structure.FileName.Should().Be("Resources");
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
