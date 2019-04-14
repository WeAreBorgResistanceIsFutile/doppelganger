using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Doppelganger.Image.ValueObjects
{
    public class NEF : ImageBase 
    {
        public NEF(string fileName, int hash, int byteCount, byte [] pHash) : base(fileName, hash, byteCount, pHash)
        {
        }
    }


    public class ShippingMethodConverter : JsonConverter
    {

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException("Not implemented yet");
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return string.Empty;
            }
            else if (reader.TokenType == JsonToken.String)
            {
                return serializer.Deserialize(reader, objectType);
            }
            else
            {
                JObject obj = JObject.Load(reader);
                if (obj["Code"] != null)
                    return obj["Code"].ToString();
                else
                    return serializer.Deserialize(reader, objectType);
            }
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override bool CanConvert(Type objectType)
        {
            return false;
        }
    }
}
