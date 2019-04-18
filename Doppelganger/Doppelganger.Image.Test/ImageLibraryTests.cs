using Doppelganger.Image.ValueObjects;

using FluentAssertions;
using FluentAssertions.Execution;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Doppelganger.Image.Test
{
    [TestClass]
    public class ImageLibraryTests
    {
        [TestMethod]
        public void Create_Should_Work()
        {
            new ImageLibrary("some library");
        }

        [TestMethod]
        public void Create_Should_Fail_With_Name_null()
        {
            new ImageLibrary(null);
        }

        [TestMethod]
        public void AddChild_Should_Work()
        {
            var root = new ImageLibrary("root");
            var someLibrary = new ImageLibrary("someLibrary");
            var someLibrary2 = new ImageLibrary("someLibrary2");
            var someImage = new ImageLibrary("someLibrary3");

            root.Add(someLibrary);
            root.Add(someLibrary2);
            someLibrary2.Add(someImage);

            using (new AssertionScope())
            {
                root.ImageLibraryCount.Should().Be(2);
                someLibrary.ImageLibraryCount.Should().Be(0);
                someLibrary2.ImageLibraryCount.Should().Be(1);
            }
        }

        [TestMethod]
        public void Delete_Image_Should_Work()
        {
            DeleteImage(@"root\someImage2");
        }

        [TestMethod]
        public void Delete_Image_Should_Work2()
        {
            DeleteImage(@"root\someLibrary2\someImage");
        }

        private void DeleteImage(string fullName)
        {
            var root = CreateObjectTree() as ImageLibrary;
            var image = root.GetImageByFullName(fullName);
            (image.GetParent() as ImageLibrary).Remove(image);

            using (new AssertionScope())
            {
                image.GetParent().Should().BeNull();
                image.GetPath().Should().Be(image.Name);
            }
        }

        [TestMethod]
        public void Delete_Library_Should_Work()
        {
            DeleteLibrary(@"root\someLibrary");
        }

        [TestMethod]
        public void Delete_Library_Should_Work2()
        {
            DeleteLibrary(@"root\someLibrary2\someLibrary2");
        }

        private void DeleteLibrary(string path)
        {
            var root = CreateObjectTree() as ImageLibrary;

            var someLibrary = root.GetImageLibraryByPath(path);

            (someLibrary.GetParent() as ImageLibrary).Remove(someLibrary);

            using (new AssertionScope())
            {
                someLibrary.GetParent().Should().BeNull();
                someLibrary.GetPath().Should().Be(someLibrary.Name);
            }
        }

        [TestMethod]
        public void GetPath_Should_Work()
        {
            var root = CreateObjectTree() as ImageLibrary;

            using (new AssertionScope())
            {
                root.GetPath().Should().Be("root");
                root.GetImageLibraryAt(0).GetPath().Should().Be(@"root\someLibrary");
                root.GetImageLibraryAt(1).GetPath().Should().Be(@"root\someLibrary2");
                (root.GetImageLibraryAt(1) as ImageLibrary)[0].GetPath().Should().Be(@"root\someLibrary2\someImage");
            }
        }

        [TestMethod]
        public void GetImageByFullName_Should_Work()
        {
            var root = CreateObjectTree() as ImageLibrary;

            var image = root.GetImageByFullName(@"root\someLibrary2\someImage");

            using (new AssertionScope())
            {
                image.Should().NotBeNull();
                image.Name.Should().Be("someImage");
            }
        }

        [TestMethod]
        public void GetImageLibraryByPath_Should_Work()
        {
            var root = CreateObjectTree() as ImageLibrary;

            var imageLibrary = root.GetImageLibraryByPath(@"root\someLibrary2\someLibrary2");

            using (new AssertionScope())
            {
                imageLibrary.Should().NotBeNull();
                imageLibrary.Name.Should().Be("someLibrary2");
            }
        }

        private FileSystemElement CreateObjectTree()
        {
            var root = new RootImageLibrary("root");
            var someLibrary = new ImageLibrary("someLibrary");
            var someLibrary2 = new ImageLibrary("someLibrary2");
            var someLibrary3 = new ImageLibrary("someLibrary2");
            var someImage = new NEF("someImage", 0, 0, new byte[0]);
            var someImage2 = new NEF("someImage2", 0, 0, new byte[0]);

            root.Add(someLibrary);
            root.Add(someImage2);
            root.Add(someLibrary2);
            someLibrary2.Add(someImage);
            someLibrary2.Add(someLibrary3);
            return root;
        }
    }
}