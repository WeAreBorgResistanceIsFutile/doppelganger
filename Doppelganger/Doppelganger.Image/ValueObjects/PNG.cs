namespace Doppelganger.Image.ValueObjects
{
    public class PNG : ImageBase
    {
        public PNG(string fileName, int hash, int byteCount, byte[] pHash) : base(fileName, hash, byteCount, pHash)
        {
        }
    }
}