using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static PInvoke.User32;
using static PInvoke.User32.WindowMessage;
using static PInvokeExtension.User32;

namespace HotkeyMessageReceiver
{
    public partial class Form1 : Form
    {
        MessageReceiver win32;

        public Form1()
        {
            InitializeComponent();

            new Thread(new ThreadStart(() =>
            {
                // Thread do
                win32 = new();
                win32.MessageLoop();

            }))
            {
                // Thread Properties
                IsBackground = true
            }
            .Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            win32.AddHotkey(PInvokeExtension.User32.Keys.A, KeyModifiers.None);
            win32.AddHotkey(PInvokeExtension.User32.Keys.B, KeyModifiers.Control);
            win32.AddHotkey(PInvokeExtension.User32.Keys.C, KeyModifiers.Control | KeyModifiers.Shift);
        }

        

        private void button2_Click(object sender, EventArgs e)
        {// Register A
            win32.AddHotkey(PInvokeExtension.User32.Keys.A, KeyModifiers.None);
        }

        private void button3_Click(object sender, EventArgs e)
        {// Register Ctrl + B
            win32.AddHotkey(PInvokeExtension.User32.Keys.B, KeyModifiers.Control);
        }

        private void button4_Click(object sender, EventArgs e)
        {// Register Ctrl + Shift + C
            win32.AddHotkey(PInvokeExtension.User32.Keys.C, KeyModifiers.Control | KeyModifiers.Shift);
        }

        private void button5_Click(object sender, EventArgs e)
        {// Unregister A
            win32.RemoveHotkey(PInvokeExtension.User32.Keys.A, KeyModifiers.None);
        }

        private void button6_Click(object sender, EventArgs e)
        {// Unregister Ctrl + B
            win32.RemoveHotkey(PInvokeExtension.User32.Keys.B, KeyModifiers.Control);
        }

        private void button7_Click(object sender, EventArgs e)
        {// Unregister Ctrl + Shift + C
            win32.RemoveHotkey(PInvokeExtension.User32.Keys.C, KeyModifiers.Control | KeyModifiers.Shift);
        }
    }
}
