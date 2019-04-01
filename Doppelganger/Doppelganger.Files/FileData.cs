namespace Doppelganger.Files
{
    public class FileData
    {
        public string Name { get; }
        public string FullPath { get; }
        public int Hash{ get; }
        public int ByteCount { get; }

        public FileData(string fileName, string fullPath, int hashCode, int byteCount)
        {
            Name = fileName;
            FullPath = fullPath;
            Hash = hashCode;
            ByteCount = byteCount;
        }
    }
}
