using System;
using Doppelganger.Image.Stores;
using Doppelganger.Image.ValueObjects;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Doppelganger.Image.Test.StoreTests
{
    [TestClass]
    public class ImageLibraryStoreTests
    {
        [TestMethod]
        public void Create_Should_Work()
        {
            new ImageLibraryStore();
        }

        [TestMethod]
        public void Add_Should_Work()
        {
            var imageLibraryStore = new ImageLibraryStore();
            imageLibraryStore.Add(new ImageLibrary("some name"));
            imageLibraryStore.Count.Should().Be(1);
        }

        [TestMethod]
        public void Add_Should_Fail_NULL_Not_Allowed()
        {
            var imageLibraryStore = new ImageLibraryStore();
            Action action = ()=> imageLibraryStore.Add((ImageLibrary)null);
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Add_Multiple_Should_Work()
        {
            var imageLibraryStore = new ImageLibraryStore();
            imageLibraryStore.Add(new ImageLibrary[] { new ImageLibrary("some name"), new ImageLibrary("some other name") });
            imageLibraryStore.Count.Should().Be(2);
        }

        [TestMethod]
        public void Remove_Should_Work()
        {
            var imageLibraryStore = new ImageLibraryStore();
            imageLibraryStore.Add(new ImageLibrary[] { new ImageLibrary("some name"), new ImageLibrary("some other name") });
            imageLibraryStore.Remove(new ImageLibrary("some name"));
            imageLibraryStore.Count.Should().Be(1);
        }
    }
}
