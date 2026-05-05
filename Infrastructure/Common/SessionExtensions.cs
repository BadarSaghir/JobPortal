using System.Text.Json;


public static class SessionExtensions 
{
    public static void SetObject(this ISession session, string key, object value) => 
        session.SetString(key, JsonSerializer.Serialize(value));

    // Fixed: Return type is now nullable (T?) to explicitly indicate possible null return
    public static T? GetObject<T>(this ISession session, string key)
    {
        var value = session.GetString(key);
        return value == null ? default : JsonSerializer.Deserialize<T>(value);
    }
}