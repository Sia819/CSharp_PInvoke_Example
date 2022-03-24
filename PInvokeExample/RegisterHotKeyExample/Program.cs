using System;
using System.Runtime.InteropServices;
using static Windows.define;
using static PInvokeExtension.User32;
using static PInvoke.User32;
using static PInvoke.User32.WindowMessage;

namespace RegisterHotKeyExample
{
    public unsafe class Program
    {
        static void Main(string[] args)
        {
            new Program();
        }

        public Program()
        {
            IntPtr hInstance = Marshal.GetHINSTANCE(typeof(Program).Module);

            RegisterHotKey(NULL, 1, NULL, Keys.A);   // Register HotKey A
            MSG _message = new MSG();                // point referenced of MSG structor
            IntPtr Message = (IntPtr)(&_message);

            while (GetMessage(Message, IntPtr.Zero, WM_NULL, WM_NULL) > 0)
            {
                TranslateMessage(Message);
                DispatchMessage(Message);
                if (_message.message == WM_HOTKEY)
                {
                    Console.WriteLine("A");
                }
            }
            UnregisterHotKey(NULL, 1);
        }
    }
}
