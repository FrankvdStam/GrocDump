using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GrocDump.Gui
{
    public static class Native
    {
        [DllImport("user32.dll", SetLastError = true)]
        private static extern UInt32 GetWindowLong(IntPtr hWnd, int nIndex);

        public static bool HasActiveWindow(this Process process)
        {
            var windowStyleDisabled = 0x8000000;
            int gwlStyle = -16;

            if (process.MainWindowHandle != IntPtr.Zero)
            {
                var style = GetWindowLong(process.MainWindowHandle, gwlStyle);
                return ((style & windowStyleDisabled) != windowStyleDisabled);
            }
            return false;
        }
    }
}
