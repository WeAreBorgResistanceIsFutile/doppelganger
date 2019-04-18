using System;
using System.Collections.Generic;
using System.Linq;

using Doppelganger.Image.ValueObjects;

namespace Doppelganger.Image.Stores
{
    public class ImageStore
    {
        private readonly Dictionary<int, List<Guid>> _hashIndex;
        private readonly Dictionary<string, Guid> _nameIndex;

        [Doppelganger.Image.Api.Attributes.Serializable(nameof(_store))]
        protected readonly Dictionary<Guid, ImageBase> _store;

        public ImageStore()
        {
            _nameIndex = new Dictionary<string, Guid>();
            _hashIndex = new Dictionary<int, List<Guid>>();
            _store = new Dictionary<Guid, ImageBase>();
        }

        public int Count => _store.Count();
        public ImageBase this[int index] => _store.Values.ToList()[index];

        public void Add(ImageBase element)
        {
            if (element is null)
                throw new ArgumentNullException(nameof(element), "Should be an initialized ImageBase instance");

            if (_store.ContainsKey(element.UniqueId))
                throw new ArgumentNullException(nameof(element), $"This image (name: {element.Name}, unique id: {element.UniqueId}) is already a member of this library!");

            if (_nameIndex.ContainsKey(element.Name))
                throw new ArgumentNullException(nameof(element), $"This image (name: {element.Name}) is already a member of this library!");

            _store.Add(element.UniqueId, element);
            UpdateHashIndex(element);
            UpdateNameIndex(element);
        }

        public void Add(ImageBase[] images)
        {
            foreach (var image in images)
                Add(image);
        }

        public void Remove(ImageBase element)
        {
            ElementExistsCheck(element);

            _store.Remove(element.UniqueId);
            RemoveFromHashIndex(element);
            RemoveFromNameIndex(element);
        }

        public ImageBase GetImageByFullName(string fullName)
        {
            return _store.Values.SingleOrDefault(p => p.GetPath().Equals(fullName));
        }

        public IEnumerable<T> ImagesOfTypeCount<T>() where T : ImageBase
        {
            return _store.Values.OfType<T>();
        }

        private void RemoveFromHashIndex(ImageBase element)
        {
            if (_hashIndex.ContainsKey(element.Hash))
            {
                _hashIndex[element.Hash].Remove(element.UniqueId);
                if (_hashIndex[element.Hash].Count == 0)
                    _hashIndex.Remove(element.Hash);
            }
        }

        private void RemoveFromNameIndex(ImageBase element)
        {
            if (_nameIndex.ContainsKey(element.Name))
                _nameIndex.Remove(element.Name);
        }

        private void UpdateHashIndex(ImageBase element)
        {
            if (!_hashIndex.ContainsKey(element.Hash))
                _hashIndex.Add(element.Hash, new List<Guid>());
            _hashIndex[element.Hash].Add(element.UniqueId);
        }

        private void UpdateNameIndex(ImageBase element)
        {
            _nameIndex.Add(element.Name, element.UniqueId);
        }

        private void ElementExistsCheck(ImageBase element)
        {
            if (!(_store.ContainsKey(element.UniqueId)))
            {
                throw new ArgumentException($"Could not find {nameof(element)}: {element} in the {nameof(ImageStore)}.");
            }
        }

        public bool ContainsImage(Guid uniqueId)
        {
            return _store.ContainsKey(uniqueId);
        }
    }
}