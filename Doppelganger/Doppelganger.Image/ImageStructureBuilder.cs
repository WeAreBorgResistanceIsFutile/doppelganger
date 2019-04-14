using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Doppelganger.Image.ValueObjects;

namespace Doppelganger.Image
{
    public class ImageStructureBuilder
    {
        readonly string _sourcePath;
        readonly IFileDataExtractor _FileDataExtractor;

        public ImageStructureBuilder(string sourcePath, IFileDataExtractor fileDataExtractor)
        {
            if (string.IsNullOrWhiteSpace(sourcePath))
                throw new ArgumentException("Argument should not be null and should contain a valid path.", nameof(sourcePath));

            if (!Directory.Exists(sourcePath))
                throw new DirectoryNotFoundException($"Argument {nameof(sourcePath)} should contain a valid path.");

            if (fileDataExtractor is null)
                throw new ArgumentNullException(nameof(FileDataExtractor), "Should not be null, we'll need it later");

            this._sourcePath = sourcePath;
            this._FileDataExtractor = fileDataExtractor;
        }

        public RootImageLibrary BuildStructure()
        {
            DirectoryInfo di = new DirectoryInfo(_sourcePath);

            var retVar = new RootImageLibrary(di.FullName);
            SetupImageLibrary(retVar);
            return retVar;
        }

        private void SetupImageLibrary(ImageLibrary imageLibrary)
        {
            var di = new DirectoryInfo(imageLibrary.GetPath());
            imageLibrary.Add(GetDirectories(di));
            imageLibrary.Add(GetNEFs(di));
            imageLibrary.Add(GetPNGs(di));


            for (int i = 0; i < imageLibrary.ImageLibraryCount; i++)
            {
                SetupImageLibrary(imageLibrary.GetImageLibraryAt(i));
            }            
        }

        private List<FileSystemElement> GetDirectories(DirectoryInfo di)
        {
            var retVar = new List<FileSystemElement>();
            retVar = di.GetDirectories().Select(p =>
            {
                ImageLibrary il = new ImageLibrary(p.FullName);
                return il;
            }).ToList<FileSystemElement>();

            return retVar;
        }

        private List<FileSystemElement> GetPNGs(DirectoryInfo di)
        {
            return GetImageObject<PNG>(di, ".png");
        }

        private List<FileSystemElement> GetNEFs(DirectoryInfo di)
        {
            return GetImageObject<NEF>(di, ".nef");
        }

        private List<FileSystemElement> GetImageObject<T>(DirectoryInfo di, string extention) where T : ImageBase
        {
            var retVar = new List<FileSystemElement>();
            retVar = di.GetFiles().Where(p => p.Extension.ToLowerInvariant().Equals(extention)).Select(p =>
            {
                T fd = _FileDataExtractor.GetFileData<T>(p.Name, p.FullName, File.ReadAllBytes(p.FullName));
                return fd;
            }).ToList<FileSystemElement>();

            return retVar;
        }
    }
}
