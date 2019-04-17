using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Doppelganger.Infrastructure.Serializer
{
    internal static class SerializerSettings
    {
        internal static JsonSerializerSettings GetJSonSerializerSettings()
        {
            return new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All,
                ContractResolver = new MyContractResolver()
            };
        }
    }

    public class MyContractResolver : Newtonsoft.Json.Serialization.DefaultContractResolver
    {
        readonly Type includeAttributeType = typeof(Doppelganger.Image.Api.Attributes.Serializable);

        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            var publicProperties = GetPublicProperties(type, memberSerialization);
            var nonPublicProperties = GetMarkedNonPublicProperties(type, memberSerialization);
            var variables = GetMarkedFields(type, memberSerialization).ToList();
            variables.ForEach(p => { p.Writable = true; p.Readable = true; });

            return publicProperties.Union(nonPublicProperties).Union(variables).ToList();
        }



        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);
            property.PropertyName = GetPropertyName(member);
            return property;
        }

        private IEnumerable<JsonProperty> GetPublicProperties(Type type, MemberSerialization memberSerialization)
        {
            return type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                                        .Where(p => p.GetIndexParameters().Length == 0)
                                        .Select(p => CreateProperty(p, memberSerialization));
        }

        private IEnumerable<JsonProperty> GetMarkedNonPublicProperties(Type type, MemberSerialization memberSerialization)
        {
            return type.GetProperties(BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(p => HasBeenMarkedWithAttribute(p) && p.GetIndexParameters().Length == 0)
                .Select(CreateJsonProperty(memberSerialization));
        }

        private IEnumerable<JsonProperty> GetMarkedFields(Type type, MemberSerialization memberSerialization)
        {
            return type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)
                .Where(p => HasBeenMarkedWithAttribute(p))
                .Select(CreateJsonProperty(memberSerialization));
        }

        private Func<MemberInfo, JsonProperty> CreateJsonProperty(MemberSerialization memberSerialization)
        {
            return f => CreateProperty(f, memberSerialization);
        }

        private bool HasBeenMarkedWithAttribute(MemberInfo p)
        {
            return !(p.GetCustomAttribute(includeAttributeType) is null);
        }

        private string GetPropertyName(MemberInfo p)
        {
            if (p.GetCustomAttribute(includeAttributeType) is Doppelganger.Image.Api.Attributes.Serializable attribute)
                return attribute.Name;
            else
                return p.Name;
        }
    }
}
