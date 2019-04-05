using System;
using Doppelganger.Image.ValueObjects;

namespace Doppelganger.Image
{
    public class FileDataExtractor : IFileDataExtractor 
    {
        private const int BYTE_COUNT_TO_GENERATE_HASH_FROM = 8 * 1024 * 1024;

        public FileDataExtractor()
        {
        }

        public T GetFileData<T>(string fileName, string filePath, byte[] content) where T : ImageBase, new()
        {
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException("Argument should not be null and should contain a valid file name.", nameof(fileName));
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("Argument should not be null and should contain a valid path.", nameof(fileName));
            if (content is null)
                throw new ArgumentNullException(nameof(content));

            int hashCode = GetFileHash(content);

            return CreateReturnValue<T>(fileName, content, hashCode);
        }

        private T CreateReturnValue<T>(string fileName, byte[] content, int hashCode) where T : ImageBase, new()
        {
            var retVar = new T();
            retVar.SetHash(hashCode);
            retVar.SetName(fileName);
            retVar.SetByteCount(content.Length);

            return retVar;
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
        T GetFileData<T>(string fileName, string filePath, byte[] content) where T : ImageBase, new();
    }
}
