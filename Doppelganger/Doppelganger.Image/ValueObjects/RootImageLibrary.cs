using System;
using System.Collections.Generic;
using System.Text;

namespace Doppelganger.Image.ValueObjects
{
    public class RootImageLibrary : ImageLibrary
    {
        private readonly string _rootPath;
        public RootImageLibrary(string path) : base(path)
        {
            _rootPath = path;
        }

        protected internal override void SetParent(FileSystemElement parent)
        {
            throw new InvalidOperationException($"{nameof(RootImageLibrary)} can not have any parents. That is why is called root image library");
        }

        protected override string GetPath(string path)
        {
            return _rootPath;
        }

        public override bool Equals(object obj)
        {
            return obj is RootImageLibrary library && _rootPath == library._rootPath;
        }

        public override int GetHashCode()
        {
            var hashCode = 1472853439;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + _rootPath.GetHashCode();
            return hashCode;
        }
    }
}
