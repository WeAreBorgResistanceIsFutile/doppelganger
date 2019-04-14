using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Doppelganger.Image.ML.Test
{
    [TestClass]
    public class TrainingPackTests
    {
        private const int BYTE_ARRAY_SIZE = 106 * 159 * 4;

        [TestMethod]
        public void Create_Should_Work()
        {
            new TrainingPack(new byte[BYTE_ARRAY_SIZE], new byte[BYTE_ARRAY_SIZE], true);
        }

        [TestMethod]
        public void Create_Should_Fail_Image1_NULL()
        {
            Action action = () => new TrainingPack(null, new byte[BYTE_ARRAY_SIZE], true);
            action.Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void Create_Should_Fail_Image1_WrongDimention()
        {
            Action action = () => new TrainingPack(new byte[1], new byte[BYTE_ARRAY_SIZE], true);
            action.Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void Create_Should_Fail_Image2_NULL()
        {
            Action action = () => new TrainingPack(new byte[BYTE_ARRAY_SIZE], null, true);
            action.Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void Create_Should_Fail_Image2_WrongDimention()
        {
            Action action = () => new TrainingPack(new byte[BYTE_ARRAY_SIZE], new byte[1], true);
            action.Should().Throw<ArgumentException>();
        }
    }
}
