using System;
using System.Collections.Generic;
using System.Text;

namespace Doppelganger.Image.ValueObjects
{
    public class ImageLibrary : FileSystemElement
    {
        private readonly List<FileSystemElement> _childElements;

        public IReadOnlyList<FileSystemElement> ChildElements { get { return _childElements; } }

        public ImageLibrary(string name) : base(name)
        {
            _childElements = new List<FileSystemElement>();
        }

        public void AddChild(FileSystemElement element)
        {
            ElementNullCheck(element);

            _childElements.Add(element);
            element.SetParent(this);
        }

        public void RemoveChild(FileSystemElement element)
        {
            ElementNullCheck(element);
            ElementExistsCheck(element);

            _childElements.Remove(element);
            element.SetParent(null);
        }

        

        private static void ElementNullCheck(FileSystemElement element)
        {
            if (element is null)
                throw new ArgumentNullException(nameof(element), "should not be null");
        }

        private void ElementExistsCheck(FileSystemElement element)
        {
            if (!_childElements.Contains(element))
                throw new ArgumentException($"{nameof(element)}: {element} is not a member of {nameof(ImageLibrary)}'s child elements.");
        }
    }
}
