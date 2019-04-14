using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doppelganger.Image.ML.Test
{
    [TestClass]
    public class ImageSimilarityVerificationTrainerTests
    {
        [TestMethod]
        public void Create_Should_Work()
        {
            new ImageSimilarityVerificationTrainer(new ImageSimilarityVerificationTrainer.MyData[5]);
        }

        [TestMethod]
        public void Create_Should_Fail_Inputa_data_null()
        {
            Action action = ()=> new ImageSimilarityVerificationTrainer(null);
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Learn_Should_Work()
        {
            var inputData = new ImageSimilarityVerificationTrainer.MyData[1];
            var array = new byte[] { 1,2,3};
            inputData[0] = new ImageSimilarityVerificationTrainer.MyData(array, array);

            var alma = new ImageSimilarityVerificationTrainer(inputData);
            alma.SomeFunc();
        }
    }
}
