using System;
using System.Runtime.InteropServices;
using PInvoke;
using static PInvoke.User32;
using static PInvoke.Gdi32;
using System.ComponentModel;

namespace PInvokeExtension
{
    public static partial class User32
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, KeyModifiers fsModifiers, Keys vk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool RegisterHotKey(int hWnd, int id, KeyModifiers fsModifiers, Keys vk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool RegisterHotKey(int hWnd, int id, int fsModifiers, Keys vk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool RegisterHotKey(int hWnd, int id, int fsModifiers, int vk);
    }
}