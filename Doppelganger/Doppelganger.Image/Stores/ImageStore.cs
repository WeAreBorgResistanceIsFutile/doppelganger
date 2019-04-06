using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Doppelganger.Image.ValueObjects;

namespace Doppelganger.Image.Stores
{
    public class ImageStore
    {
        readonly Dictionary<int, List<Guid>> _hashIndex;
        readonly List<ImageBase> _store;

        public ImageStore()
        {
            _hashIndex = new Dictionary<int, List<Guid>>();
            _store = new List<ImageBase>();
        }

        public int Count => _store.Count();
        public ImageBase this[int index] => _store[index];

        public void Add(ImageBase element)
        {
            if (element is null)
                throw new ArgumentNullException(nameof(element), "Should be an initialized ImageBase instance");

            _store.Add(element);
            UpdateHashIndex(element);
        }

        public void Add(ImageBase[] images)
        {
            foreach (var image in images)
                Add(image);
        }

        public void Remove(ImageBase element)
        {
            ElementExistsCheck(element);

            _store.Remove(element);
            RemoveFromHashIndex(element);
        }

        public IEnumerable<T> ImagesOfTypeCount<T>() where T : ImageBase
        {
            return _store.OfType<T>();
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
        private void UpdateHashIndex(ImageBase element)
        {
            if (!_hashIndex.ContainsKey(element.Hash))
                _hashIndex.Add(element.Hash, new List<Guid>());
            _hashIndex[element.Hash].Add(element.UniqueId);
        }
        private void ElementExistsCheck(ImageBase element)
        {
            if (!(_hashIndex.ContainsKey(element.Hash) && _store.Contains(element)))
            {
                throw new ArgumentException($"Could not find {nameof(element)}: {element} in the {nameof(ImageStore)}.");
            }
        }
    }
}
