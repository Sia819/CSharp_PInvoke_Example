using System;
using System.Runtime.InteropServices;
using PInvoke;
using static PInvoke.Gdi32;
using static PInvoke.User32;

namespace PInvokeExtension
{

    /// <summary>
    /// Functions
    /// </summary>
    public static partial class User32Ex
    {
        #region LoadCursor

        /// <summary>
        /// return <see cref="SafeCursorHandle"/> instead of <see cref="IntPtr"/>.
        /// </summary>
        /// <param name="hInstance"></param>
        /// <param name="lpCursorName"></param>
        /// <returns></returns>
        public static unsafe IntPtr LoadCursor(IntPtr hInstance, char* lpCursorName)
        {
            Cursors value = (Cursors)Enum.Parse(typeof(Cursors), GeneralEx.ToString(lpCursorName));
            return User32ExImport.LoadCursor(hInstance, (int)value);
        }

        public static unsafe IntPtr LoadCursor(IntPtr hInstance, ReadOnlySpan<char> lpCursorName)
        {
            Cursors value = (Cursors)Enum.Parse(typeof(Cursors), lpCursorName.ToString());
            return User32ExImport.LoadCursor(hInstance, (int)value);
        }

        public static unsafe IntPtr LoadCursor(IntPtr hInstance, int lpCursorName)
        {
            return User32ExImport.LoadCursor(hInstance, lpCursorName);
        }

        public static unsafe IntPtr LoadCursor(IntPtr hInstance, Cursors cursor)
        {
            return User32ExImport.LoadCursor(hInstance, (int)cursor);
        }

        #endregion



    }

    /// <summary>
    /// Enums
    /// </summary>
    public static partial class User32Ex
    {
        public enum Icons
        {
            IDI_APPLICATION     = 32512,
            IDI_HAND            = 32513,
            IDI_QUESTION        = 32514,
            IDI_EXCLAMATION     = 32515,
            IDI_ASTERISK        = 32516,
        }

    }

    /// <summary>
    /// DLL Import
    /// </summary>
    internal static class User32ExImport
    {
        [DllImport("user32.dll")]
        public static extern IntPtr LoadCursor(IntPtr hInstance, int lpCursorName);
    }

}
