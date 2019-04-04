using System;
using System.IO;
using Doppelganger.Image.ImageFormatConverters;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Doppelganger.Image.Test
{
    [TestClass]
    public class NefToPngConverterTests
    {
        const string PATH = @".\Resources";
        const string NEF_FILENAME = "NIK_4062.NEF";

        [TestMethod]
        public void Create_Should_Work()
        {
            new NefToPngConverter("some input");
        }

        [TestMethod]
        public void Create_Should_Fail_InputFileName_NULL()
        {
            Action action = () => new NefToPngConverter(null);
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Convert_Should_Fail_InputFile_Not_Exists()
        {
            string currentFile = Path.Combine(PATH, NEF_FILENAME);
            
            Action action = () => new NefToPngConverter("some input").Convert(new System.Windows.Size(10, 10));
            action.Should().Throw<FileNotFoundException>();
        }

        [TestMethod]
        public void Convert_Should_Work_Size()
        {
            string currentFile = Path.Combine(PATH, NEF_FILENAME);

            using (var stream = new NefToPngConverter(currentFile).Convert(new System.Windows.Size(160, 10)))
            {
                stream.Should().NotBeNull();
            }                
        }

        [TestMethod]
        public void Convert_Should_Work_DimensionMaxLength_Portrait()
        {
            string currentFile = Path.Combine(PATH, NEF_FILENAME);
            int dimensionMaxLength = 160;
            
            using (var stream = new NefToPngConverter(currentFile).Convert(dimensionMaxLength))
            {
                stream.Should().NotBeNull();
            }
        }

        [TestMethod]
        public void Convert_Should_Work_DimensionMaxLength_Landscape()
        {
            string currentFile = Path.Combine(PATH, "NIK_9586.NEF");
            int dimensionMaxLength = 160;

            using (var stream = new NefToPngConverter(currentFile).Convert(dimensionMaxLength))
            {
                stream.Should().NotBeNull();
            }
        }
    }
}
