using System;
using System.IO;
using System.Windows.Media.Imaging;
using Shipwreck.Phash;
using Shipwreck.Phash.PresentationCore;

namespace Doppelganger.Image.PHash
{
    public class PHashCalculator
    {
        public byte[] CalculatePHash(Stream stream)
        {
            BitmapDecoder bmpDec = BitmapDecoder.Create(stream, BitmapCreateOptions.DelayCreation, BitmapCacheOption.OnDemand);
            return ImagePhash.ComputeDigest(bmpDec.Frames[0].ToLuminanceImage()).Coefficents;
        }

        public double CompareHashes(byte[] hash1, byte[] hash2)
        {
            if (hash1 is byte[] digest1 && hash2 is byte[] digest2)
                return ImagePhash.GetCrossCorrelation(digest1, digest2);

            return 0;
        }
    }
}
