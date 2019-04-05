using System.Text;

namespace Doppelganger.Image.ValueObjects
{
    public abstract class FileSystemElement
    {
        protected FileSystemElement Parent { get; private set; }
        public string Name { get; private set; }

        public FileSystemElement()
        {
            
        }
        public FileSystemElement(string name)
        {
            Name = name;
        }

        public void SetName(string name)
        {
            Name = name;
        }
        protected internal void SetParent(FileSystemElement parent) => Parent = parent;

        public string GetPath()
        {
            StringBuilder sb = new StringBuilder();
            return GetPath("");
        }

        private string GetPath(string path)
        {
            if (Parent is FileSystemElement p)
                path += $@"{p.GetPath(path)}\";

            path += $@"{Name}";
            return path;
        }

    }
}
