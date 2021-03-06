using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PInvokeExtension
{
    public static partial class User32
    {
        [Flags]
        public enum KeyModifiers
        {
            None = 0,
            Alt = 1,
            Control = 2,
            Shift = 4,
            // Either WINDOWS key was held down. These keys are labeled with the Windows logo.
            // Keyboard shortcuts that involve the WINDOWS key are reserved for use by the
            // operating system.
            Windows = 8
        }
    }
}