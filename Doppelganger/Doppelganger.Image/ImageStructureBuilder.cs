using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
            UpdateStructure(retVar);
            return retVar;
        }

        public void UpdateStructure(RootImageLibrary imageLibrary)
        {
            UpdateImageLibrary(imageLibrary);
        }

        private void UpdateImageLibrary(ImageLibrary imageLibrary)
        {
            var di = new DirectoryInfo(imageLibrary.GetPath());

            var directories = GetDirectories(di);
            foreach (var directory in directories)
            {
                if (imageLibrary.GetImageLibraryByPath(directory.Name) is null)
                {
                    imageLibrary.Add(directory);
                }
            }

            var nefs = GetNEFs(di, imageLibrary);
            foreach (var nef in nefs)
            {
                imageLibrary.Add(nef);
            }

            var pngs = GetPNGs(di, imageLibrary);
            foreach (var png in pngs)
            {
                imageLibrary.Add(png);
            }

            for (int i = 0; i < imageLibrary.ImageLibraryCount; i++)
            {
                UpdateImageLibrary(imageLibrary.GetImageLibraryAt(i));
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

        private List<FileSystemElement> GetPNGs(DirectoryInfo di, ImageLibrary imageLibrary)
        {
            return GetImageObjectIfNotInLibrary<PNG>(di, ".png", imageLibrary);
        }

        private List<FileSystemElement> GetNEFs(DirectoryInfo di, ImageLibrary imageLibrary)
        {
            return GetImageObjectIfNotInLibrary<NEF>(di, ".nef", imageLibrary);
        }


        private List<FileSystemElement> GetImageObjectIfNotInLibrary<T>(DirectoryInfo di, string extention, ImageLibrary imageLibrary) where T : ImageBase
        {
            var retVar = new ConcurrentBag<FileSystemElement>();
            var alma = di.GetFiles().Where(p => p.Extension.ToLowerInvariant().Equals(extention));

            Parallel.ForEach(alma, (p) =>
            {
                var imageFromLibrary = imageLibrary.GetImageByFullName(p.FullName);
                if (!(imageFromLibrary != null && imageFromLibrary.ByteCount == p.Length && imageFromLibrary.Hash == GetFileHash(p)))
                {
                    T fd = _FileDataExtractor.GetFileData<T>(p);
                    retVar.Add(fd);
                }
            });
            return retVar.ToList();
        }

        private static int GetFileHash(FileInfo fi)
        {
            using (Stream s = fi.OpenRead())
            {
                return FileHashCalculator.GetFileHash(s);
            }
        }
    }
}
