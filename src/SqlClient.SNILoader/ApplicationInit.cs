using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Hosting;
using SqlClient.SNILoader;

[assembly: PreApplicationStartMethod(typeof(InitializeHttpModules), nameof(InitializeHttpModules.Initialization))]

namespace SqlClient.SNILoader
{
    public static class InitializeHttpModules
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr LoadLibrary(string dllToLoad);

        public static void Initialization()
        {
            if (HostingEnvironment.InClientBuildManager)
            {
                return;
            }

            var moduleName = Environment.Is64BitProcess
              ? "Microsoft.Data.SqlClient.SNI.x64.dll"
              : "Microsoft.Data.SqlClient.SNI.x86.dll";

            AppDomain appDomain = AppDomain.CurrentDomain;

            var targetFile = Path.Combine(appDomain.DynamicDirectory, moduleName);
            if (!File.Exists(targetFile))
            {
                var resourceName = $"SqlClient.SNILoader.{moduleName}";
                Stream assemblyResource = typeof(InitializeHttpModules).Assembly.GetManifestResourceStream(resourceName);
                using (FileStream targetStream = new FileStream(targetFile, FileMode.Create, FileAccess.Write))
                {
                    assemblyResource.CopyTo(targetStream);
                    targetStream.Flush();
                }
            }

            LoadLibrary(targetFile);
        }
    }
}
