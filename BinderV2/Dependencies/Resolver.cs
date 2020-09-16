using System;
using System.Reflection;
using System.Windows;
using System.Threading;

namespace BinderV2.DependencyResolver
{
    public static class Resolver
    {
        private static volatile bool _loaded;

        public static void RegisterDependencyResolver()
        {
            if (!_loaded)
            {
                AppDomain.CurrentDomain.AssemblyResolve += OnResolve;
                _loaded = true;
            }
        }

        private static Assembly OnResolve(object sender, ResolveEventArgs args)
        {
            Assembly execAssembly = Assembly.GetExecutingAssembly();
            string resourceName = new AssemblyName(args.Name).Name;
            foreach (var name in execAssembly.GetManifestResourceNames())//ищем самое подходящее
            {
                if (name.Contains(resourceName))
                {
                    using (var stream = execAssembly.GetManifestResourceStream(name))
                    {
                        int read = 0, toRead = (int)stream.Length;
                        byte[] data = new byte[toRead];

                        do
                        {
                            int n = stream.Read(data, read, data.Length - read);
                            toRead -= n;
                            read += n;
                        } while (toRead > 0);

                        return Assembly.Load(data);
                    }
                }
            }
            return null;
        }
    }
}
