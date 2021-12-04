﻿using System.Reflection;

namespace Common;

public class Resources
{
    public static string[] GetResourceFileLines(string resourceFileName)
        => GetResourceFile(Assembly.GetCallingAssembly(), resourceFileName)
            .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

    public static string GetResourceFile(string resourceFileName)
        => GetResourceFile(Assembly.GetCallingAssembly(), resourceFileName);

    public static StreamReader GetResourceStream(string resourceFileName)
        => new(GetResourceStream(Assembly.GetCallingAssembly(), resourceFileName));

    private static string GetResourceFile(Assembly assembly, string resourceFileName)
    {
        using (var stream = GetResourceStream(assembly, resourceFileName))
        using (var sr = new StreamReader(stream ?? throw new FileNotFoundException($"Couldn't locate resource file '{resourceFileName}'")))
        {
            return sr.ReadToEnd();
        }
    }

    private static Stream GetResourceStream(Assembly assembly, string resourceFileName)
    {
        Stream? stream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.{resourceFileName}");

        if (stream == null)
        {
            string? resource = assembly.GetManifestResourceNames().FirstOrDefault(res => res.EndsWith(resourceFileName));
            if (resource != null)
            {
                stream = assembly.GetManifestResourceStream(resource);
            }
        }

        if (stream == null)
        {
            throw new Exception($"Resource {resourceFileName} not found in {assembly.GetName().Name}");
        }

        return stream;
    }
}
