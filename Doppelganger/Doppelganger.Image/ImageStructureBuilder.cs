using Doppelganger.Image.ValueObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Doppelganger.Image
{
    public class ImageStructureBuilder
    {
        private readonly string _sourcePath;

        public ImageStructureBuilder(string sourcePath)
        {
            if (string.IsNullOrWhiteSpace(sourcePath))
                throw new ArgumentException("Argument should not be null and should contain a valid path.", nameof(sourcePath));

            if (!Directory.Exists(sourcePath))
                throw new DirectoryNotFoundException($"Argument {nameof(sourcePath)} should contain a valid path.");

            this._sourcePath = sourcePath;
        }

        public ImageLibrary BuildStructure()
        {
            DirectoryInfo di = new DirectoryInfo(_sourcePath);

            var retVar = new ImageLibrary(di.FullName);
            SetupImageLibrary(retVar);
            return retVar;
        }

        private void SetupImageLibrary(ImageLibrary imagLibrary)
        {
            var di = new DirectoryInfo(imagLibrary.Name);
            imagLibrary.AddChild(GetDirectories(di));
            imagLibrary.AddChild(GetNEFs(di));
            imagLibrary.AddChild(GetPNGs(di));

            foreach(var il in imagLibrary.GetImageLibraries())
            {
                SetupImageLibrary(il);
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

        private List<FileSystemElement> GetImageObject<T>(DirectoryInfo di, string extention) where T : ImageBase, new()
        {
            IFileDataExtractor fde = new FileDataExtractor();

            var retVar = new List<FileSystemElement>();
            retVar = di.GetFiles().Where(p => p.Extension.ToLowerInvariant().Equals(extention)).Select(p =>
            {
                T fd = fde.GetFileData<T>(p.Name, p.FullName, File.ReadAllBytes(p.FullName));
                return fd;
            }).ToList<FileSystemElement>();

            return retVar;
        }
    }
}
