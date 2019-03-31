using System;
using System.Collections.Generic;
using System.Text;

namespace Doppelganger.Files
{
    public class FileData
    {
        public string Name { get; }
        public string FullPath { get; }
        public int Hash{ get; }

        public FileData(string fileName, string fullPath, int hashCode)
        {
            Name = fileName;
            FullPath = fullPath;
            Hash = hashCode;
        }
    }
}
