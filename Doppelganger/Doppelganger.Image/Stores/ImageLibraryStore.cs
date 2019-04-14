using System;
using System.Collections.Generic;
using System.Linq;

using Doppelganger.Image.ValueObjects;

using Newtonsoft.Json;

namespace Doppelganger.Image.Stores
{
    public class ImageLibraryStore
    {
        [JsonProperty]
        readonly List<ImageLibrary> _library = new List<ImageLibrary>();

        public int Count { get { return _library.Count; } }

        public ImageLibrary this[int index] => (index >=0 && index < Count) ? _library[index] : null;
            

        public void Add(ImageLibrary library)
        {
            if (library is null)
                throw new ArgumentNullException(nameof(library), "Should be an initialized ImageLibrary instance");
            _library.Add(library);
        }

        public void Add(ImageLibrary[] imageLibrary)
        {
            _library.AddRange(imageLibrary);
        }

        public void Remove(ImageLibrary imageLibrary)
        {
            _library.Remove(imageLibrary);
        }

        internal ImageLibrary GetImageLibrary(string name)
        {
            return _library.FirstOrDefault(p => p.Name.Equals(name));
        }
    }
}
