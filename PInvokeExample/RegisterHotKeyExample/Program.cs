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

            RegisterHotKey(NULL, 1, NULL, Keys.A);   // Register HotKey 'A'.

            MSG _message = new MSG();                // Reference pointer of MSG structure.
            IntPtr Message = (IntPtr)(&_message);

            while (GetMessage(Message, IntPtr.Zero, WM_NULL, WM_NULL) > 0)
            {
                TranslateMessage(Message);
                DispatchMessage(Message);

                // Pressed registered hotkey.
                if (_message.message == WM_HOTKEY)
                {
                    // Pressed key and modifier key.
                    Keys key = (Keys)(((int)_message.lParam >> 16) & 0xFFFF);
                    KeyModifiers modifier = (KeyModifiers)((int)_message.lParam & 0xFFFF);

                    // Any key wasn't pressed with modifier key.
                    if (KeyModifiers.None == modifier && Keys.A == key)
                    {
                        Console.WriteLine("A");
                    }
                        
                }
            }
            UnregisterHotKey(NULL, 1);
        }
    }
}
