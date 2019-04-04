using System;
using System.Collections.Generic;
using System.Text;

namespace Doppelganger.Image.ImageFormatConverters
{
    public class NefToPngConverter : ImageFormatConverter
    {
        public NefToPngConverter(string inputFileFullName, string outputFileFullName) : base(inputFileFullName, outputFileFullName)
        {

        }

        public override void Convert(FileMode fileMode)
        {
            using (Stream stream = new FileStream(_OutputFileFullName, fileMode))
            {
                int png_Width = 160;
                Uri uri = new Uri(_InputFileFullName, UriKind.Relative);
                BitmapDecoder bmpDec = BitmapDecoder.Create(uri, BitmapCreateOptions.DelayCreation, BitmapCacheOption.None);
                BitmapFrame bitmapSource = CreateResizedImage(bmpDec.Frames[0], png_Width, (int)(bmpDec.Frames[0].Height / (float)bmpDec.Frames[0].Width * png_Width));

                var pngEncoder = new PngBitmapEncoder();
                pngEncoder.Frames.Add(bitmapSource);

                pngEncoder.Save(stream);
            }
        }
    }
}
