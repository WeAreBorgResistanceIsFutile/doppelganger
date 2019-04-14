using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Doppelganger.Image.ValueObjects
{
    public class RootImageLibrary : ImageLibrary
    {
        [JsonProperty("Path")]
        private readonly string _rootPath;

        public RootImageLibrary(string path) : base(path)
        {
            _rootPath = path;
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
