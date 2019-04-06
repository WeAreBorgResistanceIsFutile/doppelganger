using System;
using System.IO;

namespace Doppelganger.Image.ValueObjects
{
    public abstract class ImageBase : FileSystemElement
    {
        private Guid _uniqueId;

        public int Hash { get; private set; }
        public int ByteCount { get; private set; }
        public Guid UniqueId => _uniqueId;

        public ImageBase() : base()
        {
            _uniqueId = Guid.NewGuid();
        }
        public ImageBase(string fileName, int hash, int byteCount) : base(Path.GetFileNameWithoutExtension(fileName))
        {
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentNullException(nameof(fileName), "Should not be empthy or null");

            Hash = hash;
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

        public override bool Equals(object obj)
        {
            bool equals = false;
            if (obj is ImageBase image && !(image is null))
            {
                equals = image.UniqueId.Equals(UniqueId);
            }
            return equals;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                // Choose large primes to avoid hashing collisions
                const int HashingBase = (int)2166136261;
                const int HashingMultiplier = 16777619;

                int hash = HashingBase;

                hash = (hash * HashingMultiplier) ^ UniqueId.GetHashCode();
                hash = (hash * HashingMultiplier) ^ (GetPath()?.GetHashCode() ?? 0);

                return hash;
            }
        }

        public override string ToString()
        {
            return GetPath();
        }
    }
}
