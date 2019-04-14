﻿using Doppelganger.Image.ValueObjects;

namespace Doppelganger.Image
{
    public class ImageFactory
    {
        public T Create<T>(string name, int hash, int byteCount) where T : ImageBase
        {
            if (typeof(T) == typeof(NEF))
            {
                return new NEF(name, hash, byteCount) as T;
            }
            else if (typeof(T) == typeof(PNG))
            {
                return new PNG(name, hash, byteCount) as T;
            }
            else
                return null;

        }
    }
}
