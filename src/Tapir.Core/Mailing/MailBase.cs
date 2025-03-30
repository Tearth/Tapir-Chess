using System.Reflection;

namespace Tapir.Core.Mailing
{
    public abstract class MailBase
    {
        protected async Task<string> ReadTemplate(string name)
        {
            var assembly = GetType().Assembly;
            var resources = assembly.GetManifestResourceNames();
            var resourceName = resources.FirstOrDefault(p => p.EndsWith(name));

            if (string.IsNullOrEmpty(resourceName))
            {
                throw new InvalidOperationException($"Template {name} not found.");
            }

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                {
                    throw new InvalidOperationException($"Cannot read stream for resource {name}");
                }

                using (var reader = new StreamReader(stream))
                {
                    return await reader.ReadToEndAsync();
                }
            }
        }
    }
}
