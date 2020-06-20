using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Loader;
using System.Reflection;

namespace TrainTime.Models
{
    class PluginLoadContext : AssemblyLoadContext
    {
        private AssemblyDependencyResolver resolver;

        public PluginLoadContext(string asmPath)
        {
            resolver = new AssemblyDependencyResolver(asmPath);
        }

        protected override Assembly Load(AssemblyName assemblyName)
        {
            string asmpath = resolver.ResolveAssemblyToPath(assemblyName);
            if (asmpath != null)
                return LoadFromAssemblyPath(asmpath);
            return null;
        }

        protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
        {
            string libpath = resolver.ResolveUnmanagedDllToPath(unmanagedDllName);
            if (libpath != null) return LoadUnmanagedDllFromPath(libpath);
            return IntPtr.Zero;
        }
    }
}
