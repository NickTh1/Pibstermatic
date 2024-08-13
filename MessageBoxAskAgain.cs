using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WaveMix
{
    public partial class MessageBoxAskAgain : Form
    {
        public MessageBoxAskAgain()
        {
            InitializeComponent();
        }

        public static DialogResult Show(string title, string message, out bool out_dont_ask_again)
        {
            MessageBoxAskAgain form = new MessageBoxAskAgain();
            form.Text = title;
            form.labelText.Text = message;

            DialogResult result = form.ShowDialog();

            out_dont_ask_again = form.checkBoxDontAskAgain.Checked;
            return result;
        }
    }
}
