using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GrocDump.Gui
{
    public static class Elevator
    {
        public static bool IsRunningAsAdmin()
        {
            using var identity = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(identity);
            var isAdmin = principal.IsInRole(WindowsBuiltInRole.Administrator);
            return isAdmin;
        }

        public static void Elevate()
        {
            if (!IsRunningAsAdmin())
            {
                var currentProcessPath = Environment.ProcessPath;

                var startInfo = new ProcessStartInfo();
                startInfo.FileName = currentProcessPath;
                startInfo.UseShellExecute = true;
                startInfo.Verb = "runas";
                Process.Start(startInfo);
                App.Current.Shutdown(0);
            }
        }
    }
}
