using System;
using System.Runtime.InteropServices;
using PInvoke;
using static PInvoke.Gdi32;
using static PInvoke.User32;

namespace PInvokeExtension
{
    /// <summary>
    /// DLL Import
    /// </summary>
    internal static class User32Import
    {
        [DllImport("user32.dll")]
        public static extern IntPtr LoadCursor(IntPtr hInstance, int lpCursorName);
    }

}
