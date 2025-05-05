using System.Text.Json;

namespace VirtualHealth.UI
{
    public static class Helper
    {
        public static T Deserialize<T>(string json)
        {
            return JsonSerializer.Deserialize<T>(json, JsonSerializerOptions);
        }

        public static string Serialize<T>(T obj)
        {
            return JsonSerializer.Serialize(obj, JsonSerializerOptions);
        }

        public static JsonSerializerOptions GetJsonSerializerOptions()
        {
            return JsonSerializerOptions;
        }

        private static readonly JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }
}
