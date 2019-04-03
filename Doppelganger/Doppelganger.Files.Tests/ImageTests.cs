using System;
using Doppelganger.Files.ValueObjects;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Doppelganger.Files.Tests
{
    [TestClass]
    public class ImageTests
    {
        [TestMethod]
        public void Create_Should_Work()
        {
            new Image("some file name", 0, 0, Image.ImageType.Jpeg);
        }

        [TestMethod]
        public void Create_With_Name_NULL_Should_Fail()
        {
            Action action = () => new Image(null, 0, 0, Image.ImageType.Jpeg);
            action.Should().Throw<ArgumentNullException>();
        }
    }
}
