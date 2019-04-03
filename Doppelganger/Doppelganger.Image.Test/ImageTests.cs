using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Doppelganger.Image.Test
{
    [TestClass]
    public class ImageTests
    {
        [TestMethod]
        public void Create_Should_Work()
        {
            new ValueObjects.Image("some file name", 0, 0, ValueObjects.Image.ImageType.Jpeg);
        }

        [TestMethod]
        public void Create_With_Name_NULL_Should_Fail()
        {
            Action action = () => new ValueObjects.Image(null, 0, 0, ValueObjects.Image.ImageType.Jpeg);
            action.Should().Throw<ArgumentNullException>();
        }
    }
}
