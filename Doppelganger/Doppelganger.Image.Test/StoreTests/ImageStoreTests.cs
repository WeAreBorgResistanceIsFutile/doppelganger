using System;
using Doppelganger.Image.Stores;
using Doppelganger.Image.ValueObjects;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Doppelganger.Image.Test.StoreTests
{
    [TestClass]
    public class ImageStoreTests
    {
        [TestMethod]
        public void Create_Should_Work()
        {
            new ImageStore();
        }

        [TestMethod]
        public void Add_Should_Work()
        {
            var ImageStore = new ImageStore();
            ImageStore.Add(new NEF("some name",0,0, pHash: new byte[0]));
            ImageStore.Count.Should().Be(1);
        }

        [TestMethod]
        public void Add_Should_Fail_NULL_Not_Allowed()
        {
            var ImageStore = new ImageStore();
            Action action = () => ImageStore.Add((PNG)null);
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Add_Multiple_Should_Work()
        {
            var ImageStore = new ImageStore();
            ImageStore.Add(new ImageBase[] { new NEF("some name", 0, 0, pHash: new byte[0]), new PNG("some name", 0, 0, pHash: new byte[0]) });
            ImageStore.Count.Should().Be(2);
        }

        [TestMethod]
        public void Remove_Should_Work()
        {
            var ImageStore = new ImageStore();
            ImageStore.Add(new ImageBase[] { new NEF("some name", 0, 0, pHash: new byte[0]), new PNG("some name", 0, 0, pHash: new byte[0]) });
            ImageStore.Remove(ImageStore[1]);
            ImageStore.Count.Should().Be(1);
        }
    }
}
