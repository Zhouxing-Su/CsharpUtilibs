using System;
using System.Reflection;


namespace IDeal.Szx.CsharpUtilibs.System {
    /// <summary>
    ///     provide utilities for embedded assembly. 
    ///     the most basic usage is just adding this file to your project.
    /// </summary>
    /// <remarks> 
    ///     to make it work, you need to add your resources in project Propertiies->Resources. <para />
    ///     to load embedded assembly, you can uncomment one of the register method in static constructor, 
    ///     or invoke one of them explicitly before your reference to any embedded assemblies.
    /// </remarks>
    public static class EmbeddedResources {
        static EmbeddedResources() {
            // uncomment one of them or copy one of them to your Main() to make it work.
            EmbeddedResources.registerAutoAssemblyResolve();
            //EmbeddedAssembly.registerManualAssemblyResolve();
            //EmbeddedAssembly.registerManualAssemblyResolve("ClosedXML", "DocumentFormat_OpenXml");
        }

        /// <summary> recursion banner for registerAutoAssemblyResolve(). </summary>
        /// <remarks> Never use it out of registerAutoAssemblyResolve(). </remarks>
        private static bool insideAssemblyResolve = false;
        /// <remarks> all embedded assemblies are automatically detected. </remarks>
        public static void registerAutoAssemblyResolve() {
            AppDomain.CurrentDomain.AssemblyResolve += (object sender, ResolveEventArgs args) => {
                if (insideAssemblyResolve) { return null; }
                insideAssemblyResolve = true;
                try {
                    string asmName = args.Name.extractAssemblyName();
                    byte[] asmBytes = (byte[])(Properties.Resources.ResourceManager.GetObject(asmName));
                    return asmBytes?.loadAsAssembly();
                } finally {
                    insideAssemblyResolve = false;
                }
            };
        }

        /// <remarks> you need to add resources name to switch cases inside this method manually. </remarks>
        public static void registerManualAssemblyResolve() {
            AppDomain.CurrentDomain.AssemblyResolve += (object sender, ResolveEventArgs args) => {
                switch (args.Name.extractAssemblyName()) {
                    // TODO[szx][0]: replace them to your own resources.
                    //case "ClosedXML":
                    //    return Assembly.Load(Properties.Resources.ClosedXML);
                    //case "DocumentFormat_OpenXml":
                    //    return Assembly.Load(Properties.Resources.DocumentFormat_OpenXml);
                    default:
                        return null;
                }
            };
        }

        /// <remarks> you need to pass resources name manually into this method. </remarks>
        public static void registerManualAssemblyResolve(params string[] embeddedAssemblyNames) {
            foreach (var asmName in embeddedAssemblyNames) {
                AppDomain.CurrentDomain.AssemblyResolve += (object sender, ResolveEventArgs args) => {
                    if (args.Name.extractAssemblyName() != asmName) { return null; }
                    byte[] asmBytes = (byte[])(Properties.Resources.ResourceManager.GetObject(asmName));
                    return asmBytes?.loadAsAssembly();
                };
            }
        }

        private static string extractAssemblyName(this string asmFullName) {
            return asmFullName.Substring(0, asmFullName.IndexOf(',')).Replace(".dll", "").Replace(".", "_");
        }

        private static Assembly loadAsAssembly(this byte[] asmBytes) {
            return Assembly.Load(asmBytes);
        }
    }
}
