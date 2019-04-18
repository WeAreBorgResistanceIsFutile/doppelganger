namespace Doppelganger.Infrastructure.Api
{
    public interface IObjectDeserializer<T> where T : class
    {
        T DeserializeObject(string dehydratedObject);
    }

    public interface IObjectSerializer<T> where T : class
    {
        string SerializeObject(T hydratedObject);
    }
}