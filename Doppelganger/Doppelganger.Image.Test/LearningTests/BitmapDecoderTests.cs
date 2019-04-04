using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Doppelganger.Image.Test
{
    [TestClass]
    public class BitmapDecoderTests
    {
        const string PATH = @".\Resources";
        const string NEF_FILENAME = "NIK_4062.NEF";

        //For this to work download and install Nikon NEF codec. Source for codec: https://downloadcenter.nikonimglib.com/en/download/sw/97.html
        [TestMethod]
        [TestCategory("Performance")]
        public void Convert_NEF_to_PNG()
        {
            string currentFile = Path.Combine(PATH, NEF_FILENAME);
            string outputFileName= Path.Combine(PATH, NEF_FILENAME + ".png");
            
            CreatePng(currentFile, outputFileName);
            VerifyIfPngHasCorrectSize(outputFileName);
            DeleteCreatedPng(outputFileName);
        }

        [TestMethod]
        [TestCategory("Performance")]
        public void Convert_Multiple_NEF_to_JPEG()
        {
            string[] files = new string[5];
            for (int i = 0; i < files.Length; i++)
            {
                files[i] = Path.Combine(PATH, NEF_FILENAME);
            }

            var random = new Random();

            Parallel.ForEach(files, (currentFile) =>
            {
                string outputFileName = Path.Combine(currentFile + random.Next() + ".png");
                CreatePng(currentFile, outputFileName);
                VerifyIfPngHasCorrectSize(outputFileName);
                DeleteCreatedPng(outputFileName);
            });
        }

        private static void VerifyIfPngHasCorrectSize(string outputFileName)
        {
            FileInfo fi = new FileInfo(outputFileName);
            fi.Length.Should().BeGreaterThan(1024);
        }

        private static void DeleteCreatedPng(string outputFileName)
        {
            FileInfo fi = new FileInfo(outputFileName);
            fi.Delete();
        }

        private static void CreatePng(string currentFile, string outputFileName)
        {
            using (Stream stream = new FileStream(outputFileName, FileMode.Create))
            {
                int png_Width = 160;
                Uri uri = new Uri(currentFile, UriKind.Relative);
                BitmapDecoder bmpDec = BitmapDecoder.Create(uri, BitmapCreateOptions.DelayCreation, BitmapCacheOption.None);
                BitmapFrame bitmapSource = CreateResizedImage(bmpDec.Frames[0], png_Width, (int)(bmpDec.Frames[0].Height / (float)bmpDec.Frames[0].Width * png_Width));

                var pngEncoder = new PngBitmapEncoder();
                pngEncoder.Frames.Add(bitmapSource);

                pngEncoder.Save(stream);
            }
        }

        private static BitmapFrame CreateResizedImage(ImageSource source, int width, int height)
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


