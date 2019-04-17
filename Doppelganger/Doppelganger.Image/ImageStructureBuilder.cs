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

            var directories = GetDirectoriesThatAreNotInTheImageLibrary(di, imageLibrary);
            imageLibrary.Add(directories);
            
            var nefs = GetNEFsThatAreNotInTheImageLibrary(di, imageLibrary);
            imageLibrary.Add(nefs);
            
            var pngs = GetPNGsThatAreNotInTheImageLibrary(di, imageLibrary);
            imageLibrary.Add(pngs);
            
            for (int i = 0; i < imageLibrary.ImageLibraryCount; i++)
            {
                UpdateImageLibrary(imageLibrary.GetImageLibraryAt(i));
            }
        }

        private List<FileSystemElement> GetDirectoriesThatAreNotInTheImageLibrary(DirectoryInfo di, ImageLibrary imageLibrary)
        {
            var retVar = new List<FileSystemElement>();
            retVar = di.GetDirectories().Where(p => imageLibrary.GetImageLibraryByPath(p.FullName) is null).Select(p =>
            {
                ImageLibrary il = new ImageLibrary(p.FullName);
                return il;
            }).ToList<FileSystemElement>();

            return retVar;
        }

        private List<FileSystemElement> GetPNGsThatAreNotInTheImageLibrary(DirectoryInfo di, ImageLibrary imageLibrary)
        {
            return GetImageObjectsThatAreNotInTheImageLibrary<PNG>(di, ".png", imageLibrary);
        }

        private List<FileSystemElement> GetNEFsThatAreNotInTheImageLibrary(DirectoryInfo di, ImageLibrary imageLibrary)
        {
            return GetImageObjectsThatAreNotInTheImageLibrary<NEF>(di, ".nef", imageLibrary);
        }


        private List<FileSystemElement> GetImageObjectsThatAreNotInTheImageLibrary<T>(DirectoryInfo di, string extention, ImageLibrary imageLibrary) where T : ImageBase
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
