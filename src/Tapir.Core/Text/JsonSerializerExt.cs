using System.Reflection;
using System.Text.Json;

namespace Tapir.Core.Text
{
    public static class JsonSerializerExt
    {
        public static void PopulateObject(string json, object target)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            using (var document = JsonDocument.Parse(json))
            {
                foreach (var property in document.RootElement.EnumerateObject())
                {
                    var propertyInfo = target.GetType().GetProperty(property.Name, BindingFlags.Public | BindingFlags.Instance);
                    if (propertyInfo != null && propertyInfo.CanWrite)
                    {
                        var value = JsonSerializer.Deserialize(property.Value.GetRawText(), propertyInfo.PropertyType, options);
                        propertyInfo.SetValue(target, value);
                    }
                }
            }
        }
    }
}
