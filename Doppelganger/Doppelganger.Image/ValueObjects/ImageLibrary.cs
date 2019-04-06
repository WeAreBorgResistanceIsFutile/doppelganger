using Doppelganger.Image.Stores;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Doppelganger.Image.ValueObjects
{
    public class ImageLibrary : FileSystemElement
    {
        readonly ImageStore _imageStore;
        readonly ImageLibraryStore _libraryStore;

        public int ImageLibraryCount => _libraryStore.Count;
        public int ImageCount => _imageStore.Count;

        public ImageLibrary(string path) : base(Path.GetFileName(path))
        {
            _imageStore = new ImageStore();
            _libraryStore = new ImageLibraryStore();
        }

        public ImageBase this[int index] => _imageStore[index];

        public virtual void Add(FileSystemElement element)
        {
            ElementNullCheck(element);

            dynamic e = element;
            Add(e);

            element.SetParent(this);
        }

        public void Add(List<FileSystemElement> list)
        {
            foreach (var fse in list)
            {
                Add(fse);
            }
        }

        public void Remove(FileSystemElement element)
        {
            ElementNullCheck(element);
            dynamic e = element;
            Remove(e);
            element.SetParent(null);
        }

        public ImageLibrary GetImageLibrary(string name)
        {
            return _libraryStore.GetImageLibrary(name);
        }

        public int ImagesOfTypeCount<T>() where T : ImageBase
        {
            return _imageStore.ImagesOfTypeCount<T>().Count();
        }

        public ImageLibrary GetImageLibraryAt(int index)
        {
            return _libraryStore[index];
        }

        private static void ElementNullCheck(FileSystemElement element)
        {
            if (element is null)
                throw new ArgumentNullException(nameof(element), "should not be null");
        }        
        private void Add(ImageLibrary element)
        {
            _libraryStore.Add(element);

        }
        private void Add(ImageBase element)
        {
            _imageStore.Add(element);
        }
        private void Remove(ImageLibrary element)
        {
            _libraryStore.Remove(element);
        }
        private void Remove(ImageBase element)
        {
            _imageStore.Remove(element);
        }

        public override bool Equals(object obj)
        {
            bool equals = false;
            if (obj is ImageLibrary imageLibrary && !(imageLibrary is null))
            {
                equals = (imageLibrary.Name?.Equals(this.Name) ?? false)
                            && imageLibrary.ImageLibraryCount.Equals(this.ImageLibraryCount)
                            && imageLibrary.GetPath().Equals(this.GetPath());
                if(equals)
                    for(int i = 0; i < this.ImageLibraryCount; i++)
                    {
                        equals = equals && GetImageLibraryAt(i).Equals(imageLibrary.GetImageLibraryAt(i));
                    }
                if (equals)
                    for (int i = 0; i < this.ImageCount; i++)
                    {
                        equals = equals && this[i].Equals(imageLibrary[i]);
                    }
            }
            return equals;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                // Choose large primes to avoid hashing collisions
                const int HashingBase = (int)2166136261;
                const int HashingMultiplier = 16777619;

                int hash = HashingBase;

                hash = (hash * HashingMultiplier) ^ Name.GetHashCode();
                

                return hash;
            }
        }

        public override string ToString()
        {
            return GetPath();
        }
    }
}
