using System;

using Doppelganger.Infrastructure.Api;

using Newtonsoft.Json;

namespace Doppelganger.Infrastructure.Serializer
{
    public class ObjectDeserializer<T> : IObjectDeserializer<T> where T : class
    {
        public T DeserializeObject(string dehydratedObject)
        {
            return HydrateRootImageLibrary(dehydratedObject);
        }

        private static T HydrateRootImageLibrary(string dehydratedObject)
        {
            if (string.IsNullOrWhiteSpace(dehydratedObject))
            {
                throw new ArgumentException("This can not be a dehydrated object!", nameof(dehydratedObject));
            }

            var settings = SerializerSettings.GetJSonSerializerSettings();

            T expectedStructure = JsonConvert.DeserializeObject<T>(dehydratedObject, settings);

            return expectedStructure;
        }
    }
}
