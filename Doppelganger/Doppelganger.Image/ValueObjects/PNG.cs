using System;
using System.Collections.Generic;
using System.Text;

namespace Doppelganger.Image.ValueObjects
{
    public class PNG : ImageBase
    {
        public PNG()
        {
        }

        public PNG(string fileName, int hashCode, int byteCount) : base(fileName, hashCode, byteCount)
        {
        }
    }
}
