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
using System.Threading;

namespace HotkeyMessageReceiver
{
    /// <summary>
    /// C++ Invoked Windows Desktop Application
    /// </summary>
    public class MessageReceiver
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

        #region Private Constant
        private const int MACRO_REG = 11111;
        private const int MACRO_UNREG = 22222;
        #endregion

        #region Public Delegate
        public unsafe delegate void HotkeyDelegate();
        #endregion

        #region Private Member
        private IntPtr g_hInst;
        private MSG msg;
        private readonly Dictionary<Hotkey, HotkeyData> registerdKeys = new();
        private int hotkeyCount;
        #endregion

        #region Private Structor
        private struct Hotkey
        {
            public Keys keys;
            public KeyModifiers keyModifiers;
        }
        #endregion

        #region Private Class
        private class HotkeyData
        {
            public int HotkeyID { get; set; }

            public HotkeyDelegate HotkeyDelegate { get; set; }
        }
        #endregion

        public unsafe MessageReceiver()  // primary cpp param is "WinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPSTR lpszCmdParam, int nCmdShow)"
        {
            // Primary param init
            IntPtr hInstance = Marshal.GetHINSTANCE(typeof(MessageReceiver).Module);
            g_hInst = hInstance;

            // Initialization
            msg = new MSG();
            WNDCLASS WndClass = new();
            WndClass.hInstance = hInstance;
            WndClass.lpfnWndProc = WndProc;
            WndClass.lpszClassName = Name.ToCharPointer();
            _ = RegisterClass(ref WndClass);
        }

        public MessageReceiver(string windowName) : this()
        {
            Name = windowName;
        }

        public void Run()
        {
            new Thread(new ThreadStart(() =>
            {
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
                                    g_hInst,
                                    IntPtr.Zero);
                // ShowWindow(Handle, nCmdShow);

                // Thread do
                MessageLoop();

            }))
            {
                // Thread Properties
                IsBackground = true
            }
            .Start();
        }

        private unsafe void MessageLoop()
        {
            fixed (MSG* messagePointer = &msg)
            {
                while (GetMessage(messagePointer, IntPtr.Zero, WM_NULL, WM_NULL) > 0)
                {
                    TranslateMessage(messagePointer);
                    //DispatchMessage(messagePointer);

                    switch ((int)msg.message)
                    {
                        case (int)WM_HOTKEY:
                            {
                                Keys key = (Keys)(((int)msg.lParam >> 16) & 0xFFFF);
                                KeyModifiers modifier = (KeyModifiers)((int)msg.lParam & 0xFFFF);
                                Hotkey hotkey = new() { keys = key, keyModifiers = modifier };

                                if (registerdKeys.TryGetValue(hotkey, out HotkeyData value))
                                {
                                    value.HotkeyDelegate.Invoke();
                                }
                                break;
                            }

                        case (int)MACRO_REG:  // Custom RegisterHotKey message received
                            {
                                Keys key = (Keys)(((int)msg.lParam >> 16) & 0xFFFF);
                                KeyModifiers modifier = (KeyModifiers)((int)msg.lParam & 0xFFFF);
                                Hotkey hotkey = new() { keys = key, keyModifiers = modifier };

                                if (registerdKeys.TryGetValue(hotkey, out HotkeyData value))   // Get registered hotkey of id from Dictionary
                                {// Avoid duplicate registration
                                    break;
                                }
                                HotkeyDelegate h = (HotkeyDelegate)Marshal.GetDelegateForFunctionPointer(msg.wParam, typeof(HotkeyDelegate));
                                registerdKeys.Add(hotkey, new HotkeyData() { HotkeyID = hotkeyCount, HotkeyDelegate = h });
                                RegisterHotKey(NULL, hotkeyCount, modifier, key);

                                Console.WriteLine("HotKey Registerd! (numberof : {0}, modifier : {1}, key : {2})", hotkeyCount, modifier, key);
                                hotkeyCount++;

                                break;
                            }
                        case MACRO_UNREG:  // Custom UnregisterHotKey message received
                            {
                                Keys key = (Keys)(((int)msg.lParam >> 16) & 0xFFFF);
                                KeyModifiers modifier = (KeyModifiers)((int)msg.lParam & 0xFFFF);

                                Hotkey hotkey = new() { keys = key, keyModifiers = modifier };
                                if (registerdKeys.TryGetValue(hotkey, out HotkeyData value))  // Get registered hotkey of id from Dictionary
                                {// Avoid duplicate unregistration
                                    _ = UnregisterHotKey(NULL, value.HotkeyID);
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


        private unsafe IntPtr WndProc(IntPtr hWnd, WindowMessage msg, void* wParam, void* lParam)
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

        public void AddHotkey(Keys key, KeyModifiers keyModifiers, HotkeyDelegate hotkeyDelegate)
        {
            _ = PostMessage(this.Handle,
                            (int)MACRO_REG,
                            Marshal.GetFunctionPointerForDelegate(hotkeyDelegate),
                            (IntPtr)MAKELPARAM((int)keyModifiers, (int)key));
        }

        public void RemoveHotkey(Keys key, KeyModifiers keyModifiers)
        {
            _ = PostMessage(this.Handle,
                            (int)MACRO_UNREG,
                            NULL,
                            MAKELPARAM((int)keyModifiers, (int)key));
        }
    }
}