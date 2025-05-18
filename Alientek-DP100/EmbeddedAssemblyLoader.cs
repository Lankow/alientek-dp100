using System;
using System.Reflection;

namespace Alientek_DP100
{
    internal static class EmbeddedAssemblyLoader
    {
        static EmbeddedAssemblyLoader()
        {
            AppDomain.CurrentDomain.AssemblyResolve += ResolveAssembly;
        }

        public static void Attach() {}

        private static Assembly ResolveAssembly(object sender, ResolveEventArgs args)
        {
            var name = new AssemblyName(args.Name).Name + ".dll";
            var resource = Array.Find(
                Assembly.GetExecutingAssembly().GetManifestResourceNames(),
                r => r.EndsWith(name));

            if (resource == null)
                return null;

            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resource);
            if (stream == null)
                return null;

            byte[] assemblyData = new byte[stream.Length];
            stream.Read(assemblyData, 0, assemblyData.Length);
            return Assembly.Load(assemblyData);
        }
    }
}
