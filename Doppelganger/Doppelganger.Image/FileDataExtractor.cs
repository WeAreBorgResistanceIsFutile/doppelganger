using System;

using Doppelganger.Image.ValueObjects;

namespace Doppelganger.Image
{
    public class FileDataExtractor : IFileDataExtractor 
    {
        private const int BYTE_COUNT_TO_GENERATE_HASH_FROM = 8 * 1024 * 1024;
        readonly ImageFactory _ImageFactory;
        public FileDataExtractor(ImageFactory imageFactory)
        {
            if (imageFactory is null)
                throw new ArgumentNullException(nameof(imageFactory), $"{nameof(imageFactory)} should not be null. We'll need it later to create ImageBase instances.");

            _ImageFactory = imageFactory;
        }

        public T GetFileData<T>(string fileName, string filePath, byte[] content) where T : ImageBase
        {
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException("Argument should not be null and should contain a valid file name.", nameof(fileName));
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("Argument should not be null and should contain a valid path.", nameof(fileName));
            if (content is null)
                throw new ArgumentNullException(nameof(content));            

            int hash = GetFileHash(content);

            return _ImageFactory.Create<T>(fileName,  hash, content.Length);      
        }

        private static int GetFileHash(byte[] content)
        {
            unchecked
            {
                const int HashingBase = (int)2166136261;
                const int HashingMultiplier = 16777619;

                int hash = HashingBase;
                for (int i = 0; i < Math.Min(content.Length, BYTE_COUNT_TO_GENERATE_HASH_FROM); i++)
                {
                    hash = (hash * HashingMultiplier) ^ content[i].GetHashCode();
                }
                return hash;
            }
        }        
    }

    public interface IFileDataExtractor
    {
        T GetFileData<T>(string fileName, string filePath, byte[] content) where T : ImageBase;
    }
}
