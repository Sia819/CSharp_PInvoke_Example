using System;
using System.Runtime.InteropServices;
using PInvokeExtension;
using static Windows.define;
using static PInvokeExtension.User32;
using static PInvoke.Gdi32;
using static PInvoke.User32;
using static PInvoke.User32.ClassStyles;
using static PInvoke.User32.Cursors;
using static PInvoke.User32.WindowStyles;
using static PInvoke.User32.WindowMessage;
using System.Collections.Generic;

namespace HotkeyMessageReceiver
{
    /// <summary>
    /// C++ Invoked Windows Desktop Application
    /// </summary>
    public unsafe class MessageReceiver
    {
        #region Public Properties
        private string name;
        public string Name
        {
            get
            {
                if (name == null)
                    return "Window";
                else
                    return name;
            }
            private set { name = value; }
        }

        public IntPtr Handle { get; private set; }
        #endregion

        #region Private Structor
        private struct Hotkey
        {
            public Keys keys;
            public KeyModifiers keyModifiers;
        }
        #endregion

        #region Private Constant
        private const int MACRO_REG = 11111;
        private const int MACRO_UNREG = 22222;
        #endregion

        #region Private Member
        private MSG msg;
        private IntPtr g_hInst;
        private Dictionary<Hotkey, int> registerdKeys = new Dictionary<Hotkey, int>();
        private int hotkeyCount;
        #endregion

        public unsafe MessageReceiver()  // primary cpp param is "WinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPSTR lpszCmdParam, int nCmdShow)"
        {
            // Primary param init
            IntPtr hInstance = Marshal.GetHINSTANCE(typeof(MessageReceiver).Module);
            g_hInst = hInstance;

            // Initialization
            msg = new MSG();
            WNDCLASS WndClass = new WNDCLASS();
            WndClass.hInstance = hInstance;
            WndClass.lpfnWndProc = WndProc;
            WndClass.lpszClassName = Name.ToCharPointer();
            RegisterClass(ref WndClass);

            // If doesn't make CreateWindow, we can't get a Handle
            Handle = CreateWindow(Name,
                                Name,
                                WS_OVERLAPPEDWINDOW,
                                CW_USEDEFAULT,
                                CW_USEDEFAULT,
                                CW_USEDEFAULT,
                                CW_USEDEFAULT,
                                IntPtr.Zero,
                                IntPtr.Zero,
                                hInstance,
                                IntPtr.Zero);
            // ShowWindow(Handle, nCmdShow);
        }

        public MessageReceiver(string windowName) : this()
        {
            Name = windowName;
        }

        [Obsolete]
        public void MessageLoop()
        {
            fixed (MSG* messagePointer = &msg)
            {
                while (GetMessage(messagePointer, IntPtr.Zero, WM_NULL, WM_NULL) > 0)
                {
                    try
                    {
                        TranslateMessage(messagePointer);
                        DispatchMessage(messagePointer);
                    }
                    catch (System.ExecutionEngineException c)
                    {
                        Console.WriteLine(c.Message);
                    }
                    

                    switch ((int)msg.message)
                    {
                        case (int)WM_HOTKEY:
                            {
                                Keys key = (Keys)(((int)msg.lParam >> 16) & 0xFFFF);
                                KeyModifiers modifier = (KeyModifiers)((int)msg.lParam & 0xFFFF);

                                if (KeyModifiers.None == modifier && Keys.A == key)
                                {
                                    RegisterHotKey(NULL, 1, NULL, Keys.A);
                                }
                                else if (KeyModifiers.Control == modifier && Keys.B == key)
                                {
                                    RegisterHotKey(NULL, 1, NULL, Keys.A);
                                }
                                else if (((int)KeyModifiers.Control | (int)KeyModifiers.Shift) == (int)modifier && Keys.C == key)
                                {
                                    RegisterHotKey(NULL, 1, NULL, Keys.A);
                                }
                                break;
                            }

                        case (int)MACRO_REG:  // Custom RegisterHotKey message received
                            {
                                Keys key = (Keys)(((int)msg.lParam >> 16) & 0xFFFF);
                                KeyModifiers modifier = (KeyModifiers)((int)msg.lParam & 0xFFFF);

                                Hotkey hotkey = new Hotkey { keys = key, keyModifiers = modifier };
                                if (registerdKeys.TryGetValue(hotkey, out int value))   // Get registered hotkey of id from Dictionary
                                {// Avoid duplicate registration
                                    break;
                                }
                                registerdKeys.Add(hotkey, hotkeyCount);
                                RegisterHotKey(NULL, hotkeyCount, modifier, key);

                                Console.WriteLine("HotKey Registerd! (numberof : {0}, modifier : {1}, key : {2})", hotkeyCount, modifier, key);
                                hotkeyCount++;

                                break;
                            }
                        case MACRO_UNREG:  // Custom UnregisterHotKey message received
                            {
                                Keys key = (Keys)(((int)msg.lParam >> 16) & 0xFFFF);
                                KeyModifiers modifier = (KeyModifiers)((int)msg.lParam & 0xFFFF);

                                Hotkey hotkey = new Hotkey { keys = key, keyModifiers = modifier };
                                if (registerdKeys.TryGetValue(hotkey, out int value))  // Get registered hotkey of id from Dictionary
                                {// Avoid duplicate unregistration
                                    UnregisterHotKey(NULL, value);
                                    registerdKeys.Remove(hotkey); // Removed hotkeys can be register again
                                    Console.WriteLine("HotKey Unegisterd! (numberof : {0}, modifier : {1}, key : {2})", value, modifier, key);
                                }
                                break;
                            }
                        default:
                            break;
                    }
                    Console.WriteLine("{0}, {1}, {2}", ((Keys)msg.message), msg.lParam, msg.wParam);
                }
            }
        }


        public unsafe IntPtr WndProc(IntPtr hWnd, WindowMessage msg, void* wParam, void* lParam)
        {
            switch (msg)
            {
                case WM_KEYDOWN:
                    return IntPtr.Zero;
                case WM_PAINT:
                    return IntPtr.Zero;
                case WM_DESTROY:
                    PostQuitMessage(0);     // GetMessage returns 0
                    return IntPtr.Zero;
                default:
                    return DefWindowProc(hWnd, msg, (IntPtr)wParam, (IntPtr)lParam);
            }
        }

        public void AddHotkey(Keys key, KeyModifiers keyModifiers)
        {
            PostMessage(this.Handle, (int)MACRO_REG, 1, MAKELPARAM((int)keyModifiers, (int)key));
        }

        public void RemoveHotkey(Keys key, KeyModifiers keyModifiers)
        {
            PostMessage(this.Handle, (int)MACRO_UNREG, 2, MAKELPARAM((int)keyModifiers, (int)key));
        }
    }
}