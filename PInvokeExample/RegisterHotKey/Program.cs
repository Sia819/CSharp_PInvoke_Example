using System;
using System.Runtime.InteropServices;
using PInvokeExtension;
using static PInvokeExtension.User32;
using static PInvoke.Gdi32;
using static PInvoke.User32;
using static PInvoke.User32.ClassStyles;
using static PInvoke.User32.Cursors;
using static PInvoke.User32.WindowStyles;
using static PInvoke.User32.WindowMessage;

namespace RegisterHotKey
{
    class Program
    {
        static void Main(string[] args)
        {
            RegisterHotKey(0, 1, 0, 0x41); //Register A
            MSG msg = { 0 };

            while (GetMessageA(&msg, NULL, 0, 0) != 0)
            {
                if (msg.message == WM_HOTKEY)
                {
                    cout << "A"; //Print A if I pressed it
                }
            }

            UnregisterHotKey(NULL, 1);
            return 0;
        }
    }
}
