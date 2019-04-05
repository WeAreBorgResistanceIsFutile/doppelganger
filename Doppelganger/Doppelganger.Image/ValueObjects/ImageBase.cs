using System;

namespace Doppelganger.Image.ValueObjects
{
    public abstract class ImageBase : FileSystemElement
    {
        public int Hash { get; private set; }
        public int ByteCount { get; private set; }

        public ImageBase() : base()
        {

        }
        public ImageBase(string fileName, int hashCode, int byteCount) : base(fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentNullException(nameof(fileName), "Should not be empthy or null");

            Hash = hashCode;
            ByteCount = byteCount;
        }
        public virtual void SetHash(int hash)
        {
            Hash = hash;
        }

        public virtual void SetByteCount(int byteCount)
        {
            ByteCount = byteCount;
        }
    }
}
