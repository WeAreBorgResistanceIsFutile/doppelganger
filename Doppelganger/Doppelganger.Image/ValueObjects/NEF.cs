using System;
using System.Collections.Generic;
using System.Text;

namespace Doppelganger.Image.ValueObjects
{
    public class NEF : ImageBase
    {
        public NEF(string fileName, int hashCode, int byteCount) : base(fileName, hashCode, byteCount)
        {
        }
    }
}
