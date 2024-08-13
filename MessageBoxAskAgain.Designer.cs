namespace WaveMix
{
    partial class MessageBoxAskAgain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            labelText = new Label();
            checkBoxDontAskAgain = new CheckBox();
            buttonYes = new Button();
            buttonNo = new Button();
            SuspendLayout();
            // 
            // labelText
            // 
            labelText.AutoSize = true;
            labelText.Location = new Point(12, 9);
            labelText.Name = "labelText";
            labelText.Size = new Size(32, 15);
            labelText.TabIndex = 0;
            labelText.Text = "label";
            // 
            // checkBoxDontAskAgain
            // 
            checkBoxDontAskAgain.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            checkBoxDontAskAgain.AutoSize = true;
            checkBoxDontAskAgain.Location = new Point(12, 68);
            checkBoxDontAskAgain.Name = "checkBoxDontAskAgain";
            checkBoxDontAskAgain.Size = new Size(149, 19);
            checkBoxDontAskAgain.TabIndex = 1;
            checkBoxDontAskAgain.Text = "Don't ask me this again";
            checkBoxDontAskAgain.UseVisualStyleBackColor = true;
            // 
            // buttonYes
            // 
            buttonYes.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            buttonYes.DialogResult = DialogResult.Yes;
            buttonYes.Location = new Point(70, 102);
            buttonYes.Name = "buttonYes";
            buttonYes.Size = new Size(75, 23);
            buttonYes.TabIndex = 2;
            buttonYes.Text = "Yes";
            buttonYes.UseVisualStyleBackColor = true;
            // 
            // buttonNo
            // 
            buttonNo.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            buttonNo.DialogResult = DialogResult.No;
            buttonNo.Location = new Point(166, 102);
            buttonNo.Name = "buttonNo";
            buttonNo.Size = new Size(75, 23);
            buttonNo.TabIndex = 3;
            buttonNo.Text = "No";
            buttonNo.UseVisualStyleBackColor = true;
            // 
            // MessageBoxAskAgain
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(316, 137);
            Controls.Add(buttonNo);
            Controls.Add(buttonYes);
            Controls.Add(checkBoxDontAskAgain);
            Controls.Add(labelText);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "MessageBoxAskAgain";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label labelText;
        private CheckBox checkBoxDontAskAgain;
        private Button buttonYes;
        private Button buttonNo;
    }
}