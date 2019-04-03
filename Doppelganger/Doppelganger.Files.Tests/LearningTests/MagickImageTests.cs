using System;
using System.IO;
using System.Threading.Tasks;
using ImageMagick;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Doppelganger.Files.Tests
{
    [TestClass]
    public class MagickImageTests
    {
        const string PATH = @".\Resources";
        const string NEF_FILENAME = "NIK_4062.NEF";
        
        [TestMethod]
        public void Convert_NEF_to_PNG()
        {
            using (Stream memStream = new MemoryStream())
            {
                // Create image that is completely purple and 800x600
                using (MagickImage image = new MagickImage(Path.Combine(PATH, NEF_FILENAME)))
                {
                    image.Resize(160, 0);
                    // Sets the output format to png
                    image.Format = MagickFormat.Png;
                    // Write the image to the memorystream
                    image.Write(memStream);

                    memStream.Flush();
                    memStream.Close();
                }
            }
        }

        [TestMethod]
        public void Convert_NEF_to_JPEG()
        {
            using (Stream memStream = new MemoryStream())
            {
                // Create image that is completely purple and 800x600
                using (MagickImage image = new MagickImage(Path.Combine(PATH, NEF_FILENAME)))
                {
                    image.Resize(160, 0);
                    // Sets the output format to png
                    image.Format = MagickFormat.Jpeg;
                    // Write the image to the memorystream
                    image.Write(memStream);

                    memStream.Flush();
                    memStream.Close();
                }
            }
        }

        [TestMethod]
        [TestCategory("Performance")]
        public void Convert_Multiple_NEF_to_PNG()
        {
            string[] files = new string[10];
            for (int i = 0; i < files.Length; i++)
            {
                files[i] = Path.Combine(PATH, NEF_FILENAME);
            }
            
            Parallel.ForEach(files, (currentFile) =>
            {
                using (Stream memStream = new MemoryStream())
                {
                    // Create image that is completely purple and 800x600
                    using (MagickImage image = new MagickImage(currentFile))
                    {
                        image.Resize(160, 0);
                        image.Format = MagickFormat.Png;
                        image.Write(memStream);

                        memStream.Flush();
                        memStream.Close();
                    }
                }
            });
        }

        [TestMethod]
        [TestCategory("Performance")]
        public void Convert_Multiple_NEF_to_JPEG()
        {
            string[] files = new string[10];
            for (int i = 0; i < files.Length; i++)
            {
                files[i] = Path.Combine(PATH, NEF_FILENAME);
            }

            Parallel.ForEach(files, (currentFile) =>
            {
                using (Stream memStream = new MemoryStream())
                {
                    using (MagickImage image = new MagickImage(currentFile))
                    {
                        image.Resize(160, 0);
                        image.Format = MagickFormat.Jpeg;
                        image.Write(memStream);

                        memStream.Flush();
                        memStream.Close();
                    }
                }
            });
        }
    }
}


