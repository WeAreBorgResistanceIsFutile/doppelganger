using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Doppelganger.Image
{
    public class FileHashCalculator
    {
        private const int BYTE_COUNT_TO_GENERATE_HASH_FROM = 8 * 1024 * 1024;

        public static int GetFileHash(Stream stream)
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
                    readByte = stream.ReadByte();
                    bytesRead++;
                }
                return hash;
            }
        }
    }
}
