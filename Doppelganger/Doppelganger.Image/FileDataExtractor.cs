using System;
using System.IO;

using Doppelganger.Image.PHash;
using Doppelganger.Image.ValueObjects;

namespace Doppelganger.Image
{
    public class FileDataExtractor : IFileDataExtractor
    {
        private const int BYTE_COUNT_TO_GENERATE_HASH_FROM = 8 * 1024 * 1024;
        readonly ImageFactory _ImageFactory;
        readonly PHashCalculator _pHashCalculator;

        public FileDataExtractor(ImageFactory imageFactory, PHashCalculator pHashCalculator)
        {
            if (imageFactory is null)
                throw new ArgumentNullException(nameof(imageFactory), $"{nameof(imageFactory)} should not be null. We'll need it later to create ImageBase instances.");

            if (pHashCalculator is null)
                throw new ArgumentNullException(nameof(pHashCalculator), $"{nameof(pHashCalculator)} should not be null. We'll need it later to create ImageBase instances.");

            _ImageFactory = imageFactory;
            _pHashCalculator = pHashCalculator;
        }

        public T GetFileData<T>(FileInfo file) where T : ImageBase
        {
            if (file == null)
                throw new ArgumentException($"Argument should not be null and should contain a valid FileInfo object.", nameof(file));

            if (file.Length > int.MaxValue)
                throw new ArgumentException($"The stream size exceeds {int.MaxValue} bytes.");

            using (var stream = file.Open(FileMode.Open))
            {
                byte[] pHash = _pHashCalculator.CalculatePHash(stream);
                int hash = GetFileHash(stream);
                return _ImageFactory.Create<T>(file.Name, hash, (int)stream.Length, pHash);
            }            
        }

        private static int GetFileHash(Stream stream)
        {
            unchecked
            {
                const int HashingBase = (int)2166136261;
                const int HashingMultiplier = 16777619;

                int hash = HashingBase;
                stream.Position = 0;

                var readByte = stream.ReadByte();
                var bytesRead = 1;
                while (readByte >= 0 && bytesRead <= BYTE_COUNT_TO_GENERATE_HASH_FROM)
                {
                    hash = (hash * HashingMultiplier) ^ ((byte)readByte).GetHashCode();
                    bytesRead++;
                }
                return hash;
            }
        }
    }

    public interface IFileDataExtractor
    {
        T GetFileData<T>(FileInfo file) where T : ImageBase;
    }
}
