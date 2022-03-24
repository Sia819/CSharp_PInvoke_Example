using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PInvokeExtension
{
    public static partial class User32
    {
        [DllImport("user32.dll")]
        public static extern int UnregisterHotKey(int hwnd, int id);
    }
    
}
