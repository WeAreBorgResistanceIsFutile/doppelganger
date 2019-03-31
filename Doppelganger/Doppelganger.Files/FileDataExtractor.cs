using System;
using System.Collections.Generic;
using System.IO;

namespace Doppelganger.Files
{
    public class FileDataExtractor
    {
        public FileData GetFileData(string fileName, string filePath, byte[] content)
        {
            int hashCode = GetHashCode(content);
            return new FileData(fileName: fileName, fullPath: Path.Combine(filePath, fileName), hashCode: hashCode);
        }

        private static int GetHashCode<T>(T obj)
        {
            unchecked
            {
                const int HashingBase = (int)2166136261;
                const int HashingMultiplier = 16777619;

                int hash = HashingBase;

                hash = (hash * HashingMultiplier) ^ (!(obj is null) ? obj.GetHashCode() : 0);
                return hash;
            }
        }

        private static int GetHashCode<T>(IEnumerable<T> obj)
        {
            unchecked
            {
                const int HashingBase = (int)2166136261;
                const int HashingMultiplier = 16777619;

                int hash = HashingBase;

                var enumerator = obj?.GetEnumerator();

                if(!(enumerator is null))
                {
                    while(enumerator.MoveNext())
                    {
                        hash = (hash * HashingMultiplier) ^ (!(enumerator.Current is null) ? enumerator.Current.GetHashCode() : 0);
                    }
                }
                return hash;
            }
        }
    }
}
