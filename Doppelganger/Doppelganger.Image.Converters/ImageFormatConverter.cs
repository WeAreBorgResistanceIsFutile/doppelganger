using System;
using System.Collections.Generic;
using System.Text;

namespace Doppelganger.Image.ImageFormatConverter
{
    public abstract class ImageFormatConverter
    {
        private readonly string _InputFileFullName;
        private readonly string _OutputFileFullName;

        protected ImageFormatConverter(string inputFileFullName, string outputFileFullName)
        {
            _InputFileFullName = inputFileFullName;
            _OutputFileFullName = outputFileFullName;
        }

        public abstract void Convert();
    }
}
