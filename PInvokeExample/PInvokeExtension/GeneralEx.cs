using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PInvokeExtension
{
    /// <summary>
    /// General of Extension
    /// </summary>
    public static class GeneralEx
    {
        /// <summary>
        /// String to char pointer
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static unsafe char* ToCharPointer(this string value)
        {
            fixed (char* strPtr = value) return strPtr;
        }

        public static unsafe string ToString(char* value)
        {
            return Marshal.PtrToStringAnsi((IntPtr)value);
        }
    }
}
