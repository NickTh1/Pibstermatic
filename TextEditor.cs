using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WaveMix
{
    class TextPosition
    {
        public int m_Line;
        public int m_Column;

        public TextPosition(int line, int column)
        {
            m_Line = line;
            m_Column = column;
        }
    }

    class TextBoxUtils
    {
        const int EM_LINESCROLL = 0x00B6;

        [DllImport("user32.dll")]
        static extern int SetScrollPos(IntPtr hWnd, int nBar, int nPos, bool bRedraw);
        [DllImport("user32.dll")]
        static extern int SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);

        [DllImport("user32.dll")]
        static extern int GetScrollPos(IntPtr hWnd, int nBar);

        public static TextPosition DetermineTextPosition(string text, int caret_position)
        {
            int line = 0;
            int column = 0;
            for(int i = 0; i < caret_position; i++)
            {
                char ch = text[i];
                if (ch == '\r')
                {
                    line++;
                    column = 0;
                }
                else
                    column++;
            }
            return new TextPosition(line, column);
        }

        public static int DetermineCaretPosition(string text, TextPosition text_position)
        {
            int len = text.Length;
            int line = 0;
            int column = 0;
            for (int i = 0; i < len; i++)
            {
                if (line == text_position.m_Line && column >= text_position.m_Column)
                    return i;
                char ch = text[i];
                if (ch == '\r')
                {
                    if (line == text_position.m_Line)
                        return i;
                    line++;
                    column = 0;
                }
                else
                    column++;
            }
            return text.Length;
        }

        public static void SetScrollPos(System.Windows.Forms.TextBox textbox, int scroll_pos)
        {
            int curr_pos = GetScrollPos(textbox);
            SetScrollPos(textbox.Handle, 1, scroll_pos, true);
            SendMessage(textbox.Handle, EM_LINESCROLL, 0, scroll_pos - curr_pos);
        }

        public static int GetScrollPos(System.Windows.Forms.TextBox textbox)
        {
            return GetScrollPos(textbox.Handle, 1);
        }
    }

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

                TextPosition pos_before = TextBoxUtils.DetermineTextPosition(textBox.Text, textBox.SelectionStart);
                int scrollpos_before = TextBoxUtils.GetScrollPos(textBox);

                textBox.Text = value;

                int sel_after = TextBoxUtils.DetermineCaretPosition(value, pos_before);
                textBox.Select(sel_after, 0);
                TextBoxUtils.SetScrollPos(textBox, scrollpos_before);
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
