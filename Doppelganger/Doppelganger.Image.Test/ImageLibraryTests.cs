using System;
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

            root.AddChild(someLibrary);
            root.AddChild(someLibrary2);
            someLibrary2.AddChild(someImage);

            using (new AssertionScope())
            {
                root.ChildElements.Count.Should().Be(2);
                someLibrary.ChildElements.Count.Should().Be(0);
                someLibrary2.ChildElements.Count.Should().Be(1);
            }
        }

        [TestMethod]
        public void Delete_Should_Work()
        {
            var root = CreateObjectTree() as ImageLibrary;
            var someLibrary = root.ChildElements[1] as ImageLibrary;

            root.RemoveChild(someLibrary);

            using (new AssertionScope())
            {
                root.ChildElements.Count.Should().Be(1);
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
                root.ChildElements[0].GetPath().Should().Be(@"root\someLibrary");
                root.ChildElements[1].GetPath().Should().Be(@"root\someLibrary2");
                (root.ChildElements[1] as ImageLibrary).ChildElements[0].GetPath().Should().Be(@"root\someLibrary2\someImage");
            }
        }

        private FileSystemElement CreateObjectTree()
        {
            var root = new ImageLibrary("root");
            var someLibrary = new ImageLibrary("someLibrary");
            var someLibrary2 = new ImageLibrary("someLibrary2");
            var someImage = new ImageLibrary("someImage");

            root.AddChild(someLibrary);
            root.AddChild(someLibrary2);
            someLibrary2.AddChild(someImage);
            return root;
        }
    }
}
