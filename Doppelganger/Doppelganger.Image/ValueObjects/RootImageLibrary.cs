using System;

namespace Doppelganger.Image.ValueObjects
{
    public class RootImageLibrary : ImageLibrary
    {
        [Doppelganger.Image.Api.Attributes.Serializable("Path")]
        private readonly string _rootPath;
               

        public RootImageLibrary(string name) : base(name)
        {
            _rootPath = name;
        }

        public void FinishDehydrationProcess()
        {
            SetMeAsParrentOfEachChildLibrary();
            SetMeAsParrentOfEachChildImage();
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
