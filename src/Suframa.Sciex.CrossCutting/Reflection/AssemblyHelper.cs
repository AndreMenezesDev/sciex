using NLog;
using System.Diagnostics;

namespace Suframa.Sciex.CrossCutting.Reflection
{
    public static class AssemblyHelper
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public static string GetCallingAssemblyVersion()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetCallingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);

            return fvi.FileVersion;
        }

        public static string GetExecutingAssemblyVersion()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            return fvi.FileVersion;
        }
    }
}