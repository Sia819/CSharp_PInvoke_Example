using System;
using System.Runtime.InteropServices;

namespace PInvokeExtension
{
    public static partial class User32
    {
        [DllImport("kernel32.dll")]
        public static extern IntPtr GetCurrentThread();
    }
}
