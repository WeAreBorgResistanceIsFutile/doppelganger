using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Doppelganger.Image.Stores;

namespace Doppelganger.Image.ValueObjects
{
    public class ImageLibrary : FileSystemElement
    {
        [Doppelganger.Image.Api.Attributes.Serializable(nameof(_imageStore))]
        protected readonly ImageStore _imageStore;

        [Doppelganger.Image.Api.Attributes.Serializable(nameof(_libraryStore))]
        protected readonly ImageLibraryStore _libraryStore;

        public int ImageLibraryCount => _libraryStore.Count;
        public int ImageCount => _imageStore.Count;

        public ImageLibrary(string name) : base(Path.GetFileName(name))
        {
            _imageStore = new ImageStore();
            _libraryStore = new ImageLibraryStore();
        }

        public ImageBase this[int index] => _imageStore[index];

        protected internal override void SetParent(FileSystemElement parent)
        {
            base.SetParent(parent);
            SetMeAsParrentOfEachChildLibrary();
            SetMeAsParrentOfEachChildImage();
        }

        protected void SetMeAsParrentOfEachChildImage()
        {
            for (int i = 0; i < _imageStore.Count; i++)
            {
                _imageStore[i].SetParent(this);
            }
        }

        protected void SetMeAsParrentOfEachChildLibrary()
        {
            for (int i = 0; i < _libraryStore.Count; i++)
            {
                _libraryStore[i].SetParent(this);
            }
        }

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
            if (element.GetParent() is ImageLibrary imageLibrary)
            {
                dynamic e = element;
                imageLibrary.Remove(e);
                element.SetParent(null);
            }
        }

        public ImageLibrary GetImageLibraryByPath(string path)
        {
            return _libraryStore.GetImageLibrary(path);
        }

        public int ImagesOfTypeCount<T>() where T : ImageBase
        {
            return _imageStore.ImagesOfTypeCount<T>().Count();
        }

        public ImageLibrary GetImageLibraryAt(int index)
        {
            return _libraryStore[index];
        }

        public ImageBase GetImageByFullName(string fullName)
        {
            var image = _imageStore.GetImageByFullName(fullName);

            for (int i = 0; i < _libraryStore.Count && image is null; i++)
            {
                image = _libraryStore[i].GetImageByFullName(fullName);
            }

            return image;
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
                if (equals)
                    for (int i = 0; i < this.ImageLibraryCount; i++)
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