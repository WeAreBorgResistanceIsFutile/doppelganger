using System;

namespace Doppelganger.Image.ValueObjects
{
    public class Image : FileSystemElement
    {
        public enum ImageType
        {
            Jpeg,
            NEF
        }

        public int Hash{ get; }
        public int ByteCount { get; }
        public ImageType Type { get; }
        
        public Image(string fileName, int hashCode, int byteCount, ImageType imageType) :base(fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentNullException(nameof(fileName), "Should not be empthy or null");

            Hash = hashCode;
            ByteCount = byteCount;
            Type = imageType;
        }
    }
}
