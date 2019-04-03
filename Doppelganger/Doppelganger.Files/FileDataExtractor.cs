using Doppelganger.Files.ValueObjects;
using System;
using System.IO;

namespace Doppelganger.Files
{
    public class FileDataExtractor
    {
        private const int BYTE_COUNT_TO_GENERATE_HASH_FROM = 8 * 1024 * 1024;

        public Image GetFileData(string fileName, string filePath, byte[] content)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException("Argument should not be null and should contain a valid file name.", nameof(fileName));
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("Argument should not be null and should contain a valid path.", nameof(fileName));
            if (content is null)
                throw new ArgumentNullException(nameof(content));

            int hashCode = GetFileHash(content);
            return new Image(fileName: fileName, hashCode: hashCode, byteCount: content.Length, imageType: Image.ImageType.NEF);
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
}
