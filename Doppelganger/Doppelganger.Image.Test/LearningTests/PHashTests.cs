using System.Drawing;
using Doppelganger.Image.ImageFormatConverters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Shipwreck.Phash;
using Shipwreck.Phash.Bitmaps;

namespace Doppelganger.Image.Test.LearningTests
{
    [TestClass]
    public class PHashTests
    {
        const string FILE = @".\resources\NIK_9586.png";

        [TestMethod]
        public void CalculatePHash_FullSize()
        {
            var converter = new NefToPngConverter(FILE);

            Bitmap image = new Bitmap(converter.Convert());
            ImagePhash.ComputeDigest(image.ToLuminanceImage());
        }


        [TestMethod]
        public void CalculatePHash_160()
        {
            var converter = new NefToPngConverter(FILE);

            Bitmap image = new Bitmap(converter.Convert(160));
            ImagePhash.ComputeDigest(image.ToLuminanceImage());
        }
    }
}
