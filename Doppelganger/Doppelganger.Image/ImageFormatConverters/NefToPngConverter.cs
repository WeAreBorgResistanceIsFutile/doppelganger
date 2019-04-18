using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Doppelganger.Image.ImageFormatConverters
{
    public class NefToPngConverter : ImageFormatConverter
    {
        public NefToPngConverter(string inputFileFullName) : base(inputFileFullName, "png")
        {
        }

        public override Stream Convert(Size convertedImageSize)
        {
            CheckFileExists(_InputFileFullName);

            Stream stream = new MemoryStream();

            WriteableBitmap originalImage = GetOriginalImageBitmapFrame(_InputFileFullName);

            int width = (int)originalImage.Width;
            int height = (int)originalImage.Height;

            BitmapFrame bitmapSource = GetResizedImage(originalImage, width, height);
            PngBitmapEncoder pngEncoder = SetupPNGEncoder(bitmapSource);

            pngEncoder.Save(stream);
            stream.Flush();

            return stream;
        }

        public override Stream Convert(int dimensionMaxLength)
        {
            CheckFileExists(_InputFileFullName);

            Stream stream = new MemoryStream();

            WriteableBitmap originalImage = GetOriginalImageBitmapFrame(_InputFileFullName);

            var originalDimensionMaxLength = (int)Math.Max(originalImage.Width, originalImage.Height);

            int width = (int)((float)dimensionMaxLength / (float)originalDimensionMaxLength * originalImage.Width);
            int height = (int)((float)dimensionMaxLength / (float)originalDimensionMaxLength * originalImage.Height);

            BitmapFrame bitmapSource = GetResizedImage(originalImage, width, height);
            PngBitmapEncoder pngEncoder = SetupPNGEncoder(bitmapSource);

            pngEncoder.Save(stream);
            stream.Flush();

            return stream;
        }

        public override Stream Convert()
        {
            CheckFileExists(_InputFileFullName);

            Stream stream = new MemoryStream();

            WriteableBitmap originalImage = GetOriginalImageBitmapFrame(_InputFileFullName);
            BitmapFrame bitmapSource = GetResizedImage(originalImage, (int)originalImage.Width, (int)originalImage.Height);
            PngBitmapEncoder pngEncoder = SetupPNGEncoder(bitmapSource);

            pngEncoder.Save(stream);
            stream.Flush();

            return stream;
        }

        private static PngBitmapEncoder SetupPNGEncoder(BitmapFrame bitmapSource)
        {
            var pngEncoder = new PngBitmapEncoder();
            pngEncoder.Frames.Add(bitmapSource);
            return pngEncoder;
        }

        private static WriteableBitmap GetOriginalImageBitmapFrame(string fileName)
        {
            Uri uri = new Uri(fileName, UriKind.Relative);
            BitmapDecoder bmpDec = BitmapDecoder.Create(uri, BitmapCreateOptions.DelayCreation, BitmapCacheOption.None);

            BitmapFrame frame = bmpDec.Frames[0];
            var stride = frame.PixelWidth * ((frame.Format.BitsPerPixel + 7) / 8);
            var pixels = new byte[frame.PixelHeight * stride];
            frame.CopyPixels(pixels, stride, 0);

            var bitmap = new WriteableBitmap(frame.PixelWidth, frame.PixelHeight, frame.DpiX, frame.DpiY, frame.Format, frame.Palette);
            var rect = new System.Windows.Int32Rect(0, 0, frame.PixelWidth, frame.PixelHeight);
            bitmap.WritePixels(rect, pixels, stride, 0);

            return bitmap;
        }

        private static BitmapFrame GetResizedImage(ImageSource source, int width, int height)
        {
            var rect = new Rect(0, 0, width, height);

            var group = new DrawingGroup();
            RenderOptions.SetBitmapScalingMode(group, BitmapScalingMode.HighQuality);
            group.Children.Add(new ImageDrawing(source, rect));

            var drawingVisual = new DrawingVisual();
            using (var drawingContext = drawingVisual.RenderOpen())
                drawingContext.DrawDrawing(group);

            var resizedImage = new RenderTargetBitmap(width, height, 96, 96, PixelFormats.Default);
            resizedImage.Render(drawingVisual);

            return BitmapFrame.Create(resizedImage);
        }
    }
}