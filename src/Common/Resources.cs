using System.Reflection;

namespace Common;

public class Resources
{
    public static string GetResourceFile(string resourceFileName)
    {
        var assembly = Assembly.GetCallingAssembly();
        Stream? stream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.{resourceFileName}");

        if (stream == null)
        {
            string? resource = assembly.GetManifestResourceNames().FirstOrDefault(res => res.EndsWith(resourceFileName));
            if (resource != null)
            {
                stream = assembly.GetManifestResourceStream(resource);
            }
        }

        using (stream)
        using (var sr = new StreamReader(stream ?? throw new FileNotFoundException($"Couldn't locate resource file '{resourceFileName}'")))
        {
            return sr.ReadToEnd();
        }
    }
}
