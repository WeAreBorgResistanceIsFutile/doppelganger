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
            ImageStore.Add(new ImageBase[] { new NEF("some name", 0, 0, pHash: new byte[0]), new PNG("some name 2", 0, 0, pHash: new byte[0]) });
            ImageStore.Count.Should().Be(2);
        }

        [TestMethod]
        public void Remove_Should_Work()
        {
            var ImageStore = new ImageStore();
            ImageStore.Add(new ImageBase[] { new NEF("some name", 0, 0, pHash: new byte[0]), new PNG("some name 2", 0, 0, pHash: new byte[0]) });
            ImageStore.Remove(ImageStore[1]);
            ImageStore.Count.Should().Be(1);
        }

        [TestMethod]
        public void Add_Multiple_Items_With_Same_Name_Should_Fail()
        {
            var ImageStore = new ImageStore();
            Action action = ()=> ImageStore.Add(new ImageBase[] { new NEF("some name", 0, 0, pHash: new byte[0]), new PNG("some name", 0, 0, pHash: new byte[0]) });
            action.Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void Add_Same_Item_Multiple_Times_Should_Fail()
        {
            var image = new NEF("some name", 0, 0, pHash: new byte[0]);
            var ImageStore = new ImageStore();
            Action action = () => ImageStore.Add(new ImageBase[] { image, image });
            action.Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void GetImageByName_Should_Succeed()
        {
            var ImageStore = new ImageStore();
            ImageStore.Add(new ImageBase[] { new NEF("some name", 1, 2, pHash: new byte[0]), new PNG("some name 1", 0, 0, pHash: new byte[0]) });
            var image = ImageStore.GetImageByFullName("some name");
            image.Should().BeOfType<NEF>();
            image.Hash.Should().Be(1);
            image.ByteCount.Should().Be(2);
        }

        [TestMethod]
        public void Non_Existing_Image_Retreival_By_Name_Should_Fail()
        {
            var ImageStore = new ImageStore();
            ImageStore.Add(new ImageBase[] { new NEF("some name", 1, 2, pHash: new byte[0]), new PNG("some name 1", 0, 0, pHash: new byte[0]) });
            var image = ImageStore.GetImageByFullName("some");
            image.Should().BeNull();
        }
    }
}
