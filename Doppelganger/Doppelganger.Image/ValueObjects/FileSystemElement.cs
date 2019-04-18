namespace Doppelganger.Image.ValueObjects
{
    public abstract class FileSystemElement
    {
        protected FileSystemElement _parent;

        [Doppelganger.Image.Api.Attributes.Serializable("FileName")]
        public string Name { get; private set; }

        public FileSystemElement(string name)
        {
            Name = name;
        }

        public FileSystemElement GetParent()
        {
            return _parent;
        }

        protected internal virtual void SetParent(FileSystemElement parent) => _parent = parent;

        protected internal string GetPath()
        {
            return GetPath("");
        }

        protected virtual string GetPath(string path)
        {
            if (_parent is FileSystemElement p)
                path += $@"{p.GetPath(path)}\";

            path += $@"{Name}";
            return path;
        }
    }
}