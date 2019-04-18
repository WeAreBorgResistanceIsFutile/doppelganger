using Doppelganger.Image.ValueObjects;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Doppelganger.Image.Test
{
    [TestClass]
    public class ImageFactoryTests
    {
        [TestMethod]
        public void Create_Should_Work_NEF()
        {
            ImageFactory factory = new ImageFactory();
            factory.Create<NEF>(name: "some name", hash: 0, byteCount: 0, pHash: new byte[0]);
        }

        [TestMethod]
        public void Create_Should_Work_PNG()
        {
            ImageFactory factory = new ImageFactory();
            factory.Create<PNG>(name: "some name", hash: 0, byteCount: 0, pHash: new byte[0]);
        }
    }
}