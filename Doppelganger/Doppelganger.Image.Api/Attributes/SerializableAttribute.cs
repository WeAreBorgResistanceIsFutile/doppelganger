using System;

namespace Doppelganger.Image.Api.Attributes
{
    [System.AttributeUsage(System.AttributeTargets.Field | System.AttributeTargets.Property)]
    public class Serializable : Attribute
    {
        public string Name { get; private set; }
        public Serializable(string alias)
        {
            Name = alias;
        }
    }
}
