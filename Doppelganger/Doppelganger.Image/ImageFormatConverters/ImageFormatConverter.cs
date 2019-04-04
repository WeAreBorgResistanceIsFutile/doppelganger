using System;
using System.Windows;
using System.IO;

namespace Doppelganger.Image.ImageFormatConverters
{
    public abstract class ImageFormatConverter
    {
        protected readonly string _InputFileFullName;
        protected readonly string _OutputPath;
        protected readonly string _Extention;

        protected ImageFormatConverter(string inputFileFullName, string extention)
        {
            if (string.IsNullOrWhiteSpace(inputFileFullName))
                throw new ArgumentNullException(nameof(inputFileFullName), "Should not be null and should contain a valid path or a valid file name and its path.");

            if (string.IsNullOrWhiteSpace(extention))
                throw new ArgumentNullException(nameof(extention), "Should not be null and should contain a valid file extention.");

            _InputFileFullName = inputFileFullName;
            _Extention = extention;
        }

        protected void CheckFileExists(string fileFullPath)
        {
            if (!File.Exists(fileFullPath))
            {
                throw new FileNotFoundException("Could not find file!", fileFullPath);
            }
        }

        public abstract Stream Convert(Size convertedImageSize);
        public abstract Stream Convert(int dimensionMaxLength);
    }
}

