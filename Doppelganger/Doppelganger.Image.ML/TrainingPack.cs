using System;

namespace Doppelganger.Image.ML
{
    public class TrainingPack
    {
        private const int BYTE_ARRAY_SIZE = 106 * 159 * 4;

        public byte[] Image1 { get; private set; }
        public byte[] Image2 { get; private set; }
        public bool IsDoppelganger { get; private set; }

        public TrainingPack(byte[] image1, byte[] image2, bool isDoppelganger)
        {
            if (image1 is null || image1.Length != BYTE_ARRAY_SIZE)
                throw new ArgumentException($"{nameof(image1)} should be a {BYTE_ARRAY_SIZE} length byte array");

            if (image2 is null || image2.Length != BYTE_ARRAY_SIZE)
                throw new ArgumentException($"{nameof(image2)} should be a {BYTE_ARRAY_SIZE} length byte array");

            Image1 = image1;
            Image2 = image2;
            IsDoppelganger = isDoppelganger;
        }
    }
}