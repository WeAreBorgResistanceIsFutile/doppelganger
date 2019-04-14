using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace Doppelganger.Image.ValueObjects
{
    public abstract class FileSystemElement
    {
        protected FileSystemElement Parent { get; private set; }

        [JsonProperty("FileName")]
        public string Name { get; private set; }

        public FileSystemElement(string name)
        {
            Name = name;
        }

        protected internal virtual void SetParent(FileSystemElement parent) => Parent = parent;

        public string GetPath()
        {
            return GetPath("");
        }

        protected virtual string GetPath(string path)
        {
            if (Parent is FileSystemElement p)
                path += $@"{p.GetPath(path)}\";

            path += $@"{Name}";
            return path;
        }

    }
}
