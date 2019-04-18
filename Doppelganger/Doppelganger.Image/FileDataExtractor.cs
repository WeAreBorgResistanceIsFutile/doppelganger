using System;
using System.IO;

using Doppelganger.Image.PHash;
using Doppelganger.Image.ValueObjects;

namespace Doppelganger.Image
{
    public class FileDataExtractor : IFileDataExtractor
    {
        private readonly ImageFactory _ImageFactory;
        private readonly PHashCalculator _pHashCalculator;

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
                int hash = FileHashCalculator.GetFileHash(stream);
                return _ImageFactory.Create<T>(file.Name, hash, (int)stream.Length, pHash);
            }
        }
    }

    public interface IFileDataExtractor
    {
        T GetFileData<T>(FileInfo file) where T : ImageBase;
    }
}