using System;
using System.Drawing;
using System.IO;
using Shipwreck.Phash;
using Shipwreck.Phash.Bitmaps;

namespace Doppelganger.Image.PHash
{
    public class PHashCalculator
    {
        public byte[] CalculatePHash (Stream bitmapSource)
        {
            Bitmap image = new Bitmap(bitmapSource);
            return ImagePhash.ComputeDigest(image.ToLuminanceImage()).Coefficents;            
        }

        public double CompareHashes(byte[] hash1, byte[] hash2)
        {
            if(hash1 is byte[] digest1 && hash2 is byte[] digest2)
                return ImagePhash.GetCrossCorrelation(digest1, digest2);

            return 0;
        }
    }
}
