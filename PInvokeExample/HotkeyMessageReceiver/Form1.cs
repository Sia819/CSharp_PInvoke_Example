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
        MessageReceiver receiver;

        public Form1()
        {
            InitializeComponent();
            receiver = new();
            receiver.Run();
        }

        void Hello1() => Console.WriteLine("Hello 1");
        void Hello2() => Console.WriteLine("Hello 2");
        void Hello3()
        {
            receiver.RemoveHotkey(PInvokeExtension.User32.Keys.C, KeyModifiers.Control | KeyModifiers.Shift);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            receiver.AddHotkey(PInvokeExtension.User32.Keys.A, KeyModifiers.None, Hello1);
            receiver.AddHotkey(PInvokeExtension.User32.Keys.B, KeyModifiers.Control, Hello2);
            receiver.AddHotkey(PInvokeExtension.User32.Keys.C, KeyModifiers.Control | KeyModifiers.Shift, Hello3);
        }

        private void button2_Click(object sender, EventArgs e)
        {// Register A
            receiver.AddHotkey(PInvokeExtension.User32.Keys.A, KeyModifiers.None, Hello1);
        }

        private void button3_Click(object sender, EventArgs e)
        {// Register Ctrl + B
            receiver.AddHotkey(PInvokeExtension.User32.Keys.B, KeyModifiers.Control, Hello2);
        }

        private void button4_Click(object sender, EventArgs e)
        {// Register Ctrl + Shift + C
            receiver.AddHotkey(PInvokeExtension.User32.Keys.C, KeyModifiers.Control | KeyModifiers.Shift, Hello3);
        }

        private void button5_Click(object sender, EventArgs e)
        {// Unregister A
            receiver.RemoveHotkey(PInvokeExtension.User32.Keys.A, KeyModifiers.None);
        }

        private void button6_Click(object sender, EventArgs e)
        {// Unregister Ctrl + B
            receiver.RemoveHotkey(PInvokeExtension.User32.Keys.B, KeyModifiers.Control);
        }

        private void button7_Click(object sender, EventArgs e)
        {// Unregister Ctrl + Shift + C
            receiver.RemoveHotkey(PInvokeExtension.User32.Keys.C, KeyModifiers.Control | KeyModifiers.Shift);
        }
    }
}
