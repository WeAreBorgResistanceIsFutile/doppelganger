using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tensorflow;

namespace Doppelganger.Image.ML.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var tensor1 = tf.constant(8); // int
            var tensor2 = tf.constant(6.0f); // float
            var tensor3 = tf.constant(6.0); // double
        }
    }
}
