using System.IO;
using System.Windows.Media.Imaging;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Shipwreck.Phash;
using Shipwreck.Phash.PresentationCore;

namespace Doppelganger.Image.Test.LearningTests
{
    [TestClass]
    public class PHashTests
    {
        private const string FILE = @".\resources\NIK_9586.png";

        [TestMethod]
        public void CalculatePHash_FullSize()
        {
            FileStream stream = new FileStream(FILE, FileMode.Open);
            BitmapDecoder bmpDec = BitmapDecoder.Create(stream, BitmapCreateOptions.DelayCreation, BitmapCacheOption.OnDemand);
            ImagePhash.ComputeDigest(bmpDec.Frames[0].ToLuminanceImage());
        }
    }
}