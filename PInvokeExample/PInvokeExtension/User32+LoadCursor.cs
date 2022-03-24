using System;
using static PInvoke.User32;

namespace PInvokeExtension
{
    public static partial class User32
    {
        /// <summary>
        /// return <see cref="SafeCursorHandle"/> instead of <see cref="IntPtr"/>.
        /// </summary>
        /// <param name="hInstance"></param>
        /// <param name="lpCursorName"></param>
        /// <returns></returns>
        public static unsafe IntPtr LoadCursor(IntPtr hInstance, char* lpCursorName)
        {
            Cursors value = (Cursors)Enum.Parse(typeof(Cursors), GeneralEx.ToString(lpCursorName));
            return User32Import.LoadCursor(hInstance, (int)value);
        }

        public static unsafe IntPtr LoadCursor(IntPtr hInstance, ReadOnlySpan<char> lpCursorName)
        {
            Cursors value = (Cursors)Enum.Parse(typeof(Cursors), lpCursorName.ToString());
            return User32Import.LoadCursor(hInstance, (int)value);
        }

        public static unsafe IntPtr LoadCursor(IntPtr hInstance, int lpCursorName)
        {
            return User32Import.LoadCursor(hInstance, lpCursorName);
        }

        public static unsafe IntPtr LoadCursor(IntPtr hInstance, Cursors cursor)
        {
            return User32Import.LoadCursor(hInstance, (int)cursor);
        }

        
    }
}
