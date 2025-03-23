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

            using (var reader = new StreamReader(assembly.GetManifestResourceStream(resourceName)))
            {
                return await reader.ReadToEndAsync();
            }
        }
    }
}
