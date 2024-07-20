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
    public partial class TextEditor : Form
    {
        string m_EnginePath;

        public event EventHandler? OnEditorTextChanged;
        public event EventHandler? OnSave;

        public TextEditor(string engine_path)
        {
            m_EnginePath = engine_path;

            InitializeComponent();

            this.Text = engine_path;

            textBox.Text = File.ReadAllText(engine_path);
        }

        public string EditorText
        {
            get { return textBox.Text; }
            set {
                if (value.Equals(textBox))
                    return;
                textBox.Text = value; 
            }
        }

        private void textBox_TextChanged(object sender, EventArgs e)
        {
            EventHandler? handler = OnEditorTextChanged;

            if (handler != null)
                handler(this, e);
        }

        public void SetCursor(int position)
        {
            this.Focus();
            textBox.Focus();

            // Scroll to bottom, so that the final cursor ends up at the top of the view.
            int len = textBox.Text.Length;
            textBox.Select(len, 0);
            textBox.ScrollToCaret();

            textBox.Select(position, 0);
            textBox.ScrollToCaret();
        }

        void Save()
        {
            EventHandler? handler = OnSave;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void textBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.S && e.Control)
                Save();
        }

        private void buttonReload_Click(object sender, EventArgs e)
        {
            try
            {
                string text = File.ReadAllText(m_EnginePath);
                this.EditorText = text;
            } catch (Exception)
            {
            }
        }
    }
}
