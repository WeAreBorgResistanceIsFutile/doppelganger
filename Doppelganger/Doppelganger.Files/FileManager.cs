using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Doppelganger.Files
{
    public class FileManager
    {
        private readonly string _directoryPath;

        public FileManager(string directoryPath)
        {
            if (string.IsNullOrWhiteSpace(directoryPath))
                throw new ArgumentException("Argument should not be null and should contain a valid path.", nameof(directoryPath));

            if (!Directory.Exists(directoryPath))
                throw new DirectoryNotFoundException($"Argument {nameof(directoryPath)} should contain a valid path.");

            this._directoryPath = directoryPath;
        }

        public string[] GetFiles()
        {
            DirectoryInfo di = new DirectoryInfo(_directoryPath);

            return di.GetFiles().Select(p => p.FullName).ToArray();
        }
    }
}
