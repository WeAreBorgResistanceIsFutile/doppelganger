using System;

namespace Doppelganger.Image.ValueObjects
{
    public abstract class ImageBase : FileSystemElement
    {
        public enum ImageType
        {
            Jpeg,
            NEF
        }

        public int Hash{ get; }
        public int ByteCount { get; }
        
        public ImageBase(string fileName, int hashCode, int byteCount) :base(fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentNullException(nameof(fileName), "Should not be empthy or null");

            Hash = hashCode;
            ByteCount = byteCount;
        }
    }
}
