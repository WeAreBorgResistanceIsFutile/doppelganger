using System;

using Doppelganger.Infrastructure.Api;

using Newtonsoft.Json;

namespace Doppelganger.Infrastructure.Serializer
{
    public class ObjectSerializer<T> : IObjectSerializer<T> where T : class
    {
        public string SerializeObject(T hydratedObject)
        {
           return DeHydrateRootImageLibrary(hydratedObject);
        }

        private static string DeHydrateRootImageLibrary(T hydratedObject)
        {
            if (hydratedObject == null)
            {
                throw new ArgumentNullException(nameof(hydratedObject));
            }

            var settings = SerializerSettings.GetJSonSerializerSettings();

            var dehydratedObject = JsonConvert.SerializeObject(hydratedObject, settings);
            return dehydratedObject;
        }
    }
}
