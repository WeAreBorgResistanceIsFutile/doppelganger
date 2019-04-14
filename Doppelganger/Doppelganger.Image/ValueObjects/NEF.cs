namespace Doppelganger.Image.ValueObjects
{
    public class NEF : ImageBase 
    {
        public NEF(string fileName, int hash, int byteCount, byte [] pHash) : base(fileName, hash, byteCount, pHash)
        {
        }
    }
}
