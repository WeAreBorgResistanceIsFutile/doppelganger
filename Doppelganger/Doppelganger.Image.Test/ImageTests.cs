using System;
using Doppelganger.Image.ValueObjects;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Doppelganger.Image.Test
{
    [TestClass]
    public class ImageTests
    {
        [TestMethod]
        public void NEF_Create_Should_Work()
        {
            new NEF("some file name", 0, 0);
        }

        [TestMethod]
        public void NEF_Create_With_Name_NULL_Should_Fail()
        {
            Action action = () => new NEF(null, 0, 0);
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Jpeg_Create_Should_Work()
        {
            new PNG("some file name", 0, 0);
        }

        [TestMethod]
        public void Jpeg_Create_With_Name_NULL_Should_Fail()
        {
            Action action = () => new PNG(null, 0, 0);
            action.Should().Throw<ArgumentNullException>();
        }
    }
}
