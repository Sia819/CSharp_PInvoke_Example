using System;
using System.Runtime.InteropServices;
using PInvokeExtension;
using static PInvokeExtension.User32Ex.Icons;
using static PInvoke.Gdi32;
using static PInvoke.User32;
using static PInvoke.User32.ClassStyles;
using static PInvoke.User32.Cursors;
using static PInvoke.User32.WindowStyles;
using static PInvoke.User32.WindowMessage;

namespace DesktopApplication
{
    /// <summary>
    /// C++ Invoked Windows Desktop Application
    /// </summary>
    public class Window
    {
        private IntPtr g_hInst;

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

        public unsafe Window()
        {
            // primary cpp param is "WinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPSTR lpszCmdParam, int nCmdShow)"
            // Primary param init
            IntPtr hInstance = Marshal.GetHINSTANCE(typeof(Window).Module);
            g_hInst = hInstance;
            WindowShowStyle nCmdShow = WindowShowStyle.SW_SHOWDEFAULT;

            // Initialization
            IntPtr hWnd;
            MSG _message = new MSG(); // point referenced of MSG structor
            IntPtr Message = (IntPtr)(&_message);
            WNDCLASS WndClass = new WNDCLASS();
            
            WndClass.cbClsExtra = 0;
            WndClass.cbWndExtra = 0;
            WndClass.hbrBackground = GetStockObject(StockObject.WHITE_BRUSH);
            WndClass.hCursor = User32Ex.LoadCursor(IntPtr.Zero, Cursors.IDC_ARROW);
            WndClass.hIcon = LoadIcon(IntPtr.Zero, nameof(IDI_APPLICATION));
            WndClass.hInstance = hInstance;
            WndClass.lpfnWndProc = WndProc;
            WndClass.lpszClassName = Name.ToCharPointer();
            WndClass.lpszMenuName = null;
            WndClass.style = CS_HREDRAW | CS_VREDRAW;
            RegisterClass(ref WndClass);

            hWnd = CreateWindow(Name,
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
            ShowWindow(hWnd, nCmdShow);

            while (GetMessage(Message, IntPtr.Zero, WM_NULL, WM_NULL) > 0)
            {
                TranslateMessage(Message);
                DispatchMessage(Message);
            }
        }

        public Window(string windowName) : this()
        {
            Name = windowName;
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
                    PostQuitMessage(0);
                    return IntPtr.Zero;
                default:
                    return DefWindowProc(hWnd, msg, (IntPtr)wParam, (IntPtr)lParam);
            }
        }
    }
}
