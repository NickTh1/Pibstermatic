namespace WaveMix
{
    partial class SCLEditor
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
            components = new System.ComponentModel.Container();
            comboBoxLayer = new ComboBox();
            label1 = new Label();
            listBoxSamples = new ListBox();
            buttonRemove = new Button();
            buttonAddAbove = new Button();
            buttonAddBelow = new Button();
            plotViewEnvelope = new OxyPlot.WindowsForms.PlotView();
            label2 = new Label();
            textBoxWav = new TextBox();
            buttonBrowse = new Button();
            panel1 = new Panel();
            label7 = new Label();
            trackBarReference = new TrackBar();
            textBoxMaxPitch = new TextBox();
            label6 = new Label();
            textBoxMinPitch = new TextBox();
            label5 = new Label();
            label4 = new Label();
            radioButtonManual = new RadioButton();
            textBoxReference = new TextBox();
            label3 = new Label();
            radioButtonProportional = new RadioButton();
            buttonMoveUp = new Button();
            buttonMoveDown = new Button();
            timer = new System.Windows.Forms.Timer(components);
            label8 = new Label();
            textBoxVolume = new TextBox();
            buttonSave = new Button();
            label9 = new Label();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)trackBarReference).BeginInit();
            SuspendLayout();
            // 
            // comboBoxLayer
            // 
            comboBoxLayer.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxLayer.FormattingEnabled = true;
            comboBoxLayer.Items.AddRange(new object[] { "0", "1" });
            comboBoxLayer.Location = new Point(56, 6);
            comboBoxLayer.Name = "comboBoxLayer";
            comboBoxLayer.Size = new Size(54, 23);
            comboBoxLayer.TabIndex = 0;
            comboBoxLayer.SelectedIndexChanged += comboBoxLayer_SelectedIndexChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(38, 15);
            label1.TabIndex = 1;
            label1.Text = "Layer:";
            // 
            // listBoxSamples
            // 
            listBoxSamples.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            listBoxSamples.FormattingEnabled = true;
            listBoxSamples.ItemHeight = 15;
            listBoxSamples.Location = new Point(12, 35);
            listBoxSamples.Name = "listBoxSamples";
            listBoxSamples.Size = new Size(166, 394);
            listBoxSamples.TabIndex = 2;
            listBoxSamples.SelectedIndexChanged += listBoxSamples_SelectedIndexChanged;
            // 
            // buttonRemove
            // 
            buttonRemove.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            buttonRemove.Location = new Point(12, 495);
            buttonRemove.Name = "buttonRemove";
            buttonRemove.Size = new Size(166, 23);
            buttonRemove.TabIndex = 3;
            buttonRemove.Text = "Remove";
            buttonRemove.UseVisualStyleBackColor = true;
            buttonRemove.Click += buttonRemove_Click;
            // 
            // buttonAddAbove
            // 
            buttonAddAbove.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            buttonAddAbove.Location = new Point(12, 524);
            buttonAddAbove.Name = "buttonAddAbove";
            buttonAddAbove.Size = new Size(166, 23);
            buttonAddAbove.TabIndex = 4;
            buttonAddAbove.Text = "Add above...";
            buttonAddAbove.UseVisualStyleBackColor = true;
            buttonAddAbove.Click += buttonAddAbove_Click;
            // 
            // buttonAddBelow
            // 
            buttonAddBelow.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            buttonAddBelow.Location = new Point(12, 553);
            buttonAddBelow.Name = "buttonAddBelow";
            buttonAddBelow.Size = new Size(166, 23);
            buttonAddBelow.TabIndex = 5;
            buttonAddBelow.Text = "Add below...";
            buttonAddBelow.UseVisualStyleBackColor = true;
            buttonAddBelow.Click += buttonAddBelow_Click;
            // 
            // plotViewEnvelope
            // 
            plotViewEnvelope.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            plotViewEnvelope.BackColor = Color.Black;
            plotViewEnvelope.Location = new Point(184, 235);
            plotViewEnvelope.Name = "plotViewEnvelope";
            plotViewEnvelope.PanCursor = Cursors.Hand;
            plotViewEnvelope.Size = new Size(805, 297);
            plotViewEnvelope.TabIndex = 6;
            plotViewEnvelope.Text = "plotView1";
            plotViewEnvelope.ZoomHorizontalCursor = Cursors.SizeWE;
            plotViewEnvelope.ZoomRectangleCursor = Cursors.SizeNWSE;
            plotViewEnvelope.ZoomVerticalCursor = Cursors.SizeNS;
            plotViewEnvelope.KeyDown += plotViewEnvelope_KeyDown;
            plotViewEnvelope.KeyUp += plotViewEnvelope_KeyUp;
            plotViewEnvelope.MouseDown += plotViewEnvelope_MouseDown;
            plotViewEnvelope.MouseMove += plotViewEnvelope_MouseMove;
            plotViewEnvelope.MouseUp += plotViewEnvelope_MouseUp;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(184, 35);
            label2.Name = "label2";
            label2.Size = new Size(33, 15);
            label2.TabIndex = 7;
            label2.Text = "Wav:";
            // 
            // textBoxWav
            // 
            textBoxWav.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            textBoxWav.Location = new Point(223, 32);
            textBoxWav.Name = "textBoxWav";
            textBoxWav.Size = new Size(685, 23);
            textBoxWav.TabIndex = 8;
            textBoxWav.TextChanged += textBoxWav_TextChanged;
            // 
            // buttonBrowse
            // 
            buttonBrowse.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonBrowse.Location = new Point(914, 31);
            buttonBrowse.Name = "buttonBrowse";
            buttonBrowse.Size = new Size(75, 23);
            buttonBrowse.TabIndex = 9;
            buttonBrowse.Text = "Browse";
            buttonBrowse.UseVisualStyleBackColor = true;
            buttonBrowse.Click += buttonBrowse_Click;
            // 
            // panel1
            // 
            panel1.Controls.Add(label7);
            panel1.Controls.Add(trackBarReference);
            panel1.Controls.Add(textBoxMaxPitch);
            panel1.Controls.Add(label6);
            panel1.Controls.Add(textBoxMinPitch);
            panel1.Controls.Add(label5);
            panel1.Controls.Add(label4);
            panel1.Controls.Add(radioButtonManual);
            panel1.Controls.Add(textBoxReference);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(radioButtonProportional);
            panel1.Location = new Point(184, 61);
            panel1.Name = "panel1";
            panel1.Size = new Size(805, 134);
            panel1.TabIndex = 10;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(188, 27);
            label7.Name = "label7";
            label7.Size = new Size(511, 15);
            label7.TabIndex = 10;
            label7.Text = "Should be the RPM the sample was taken at. Try to align peaks with known rates in the FFT view.";
            // 
            // trackBarReference
            // 
            trackBarReference.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            trackBarReference.Location = new Point(188, 50);
            trackBarReference.Maximum = 1000000;
            trackBarReference.Name = "trackBarReference";
            trackBarReference.Size = new Size(536, 45);
            trackBarReference.TabIndex = 9;
            trackBarReference.TickFrequency = 0;
            trackBarReference.TickStyle = TickStyle.None;
            trackBarReference.ValueChanged += trackBarReference_ValueChanged;
            // 
            // textBoxMaxPitch
            // 
            textBoxMaxPitch.Location = new Point(243, 98);
            textBoxMaxPitch.Name = "textBoxMaxPitch";
            textBoxMaxPitch.Size = new Size(67, 23);
            textBoxMaxPitch.TabIndex = 8;
            textBoxMaxPitch.TextChanged += textBoxMaxPitch_TextChanged;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(204, 101);
            label6.Name = "label6";
            label6.Size = new Size(33, 15);
            label6.TabIndex = 7;
            label6.Text = "Max:";
            // 
            // textBoxMinPitch
            // 
            textBoxMinPitch.Location = new Point(100, 98);
            textBoxMinPitch.Name = "textBoxMinPitch";
            textBoxMinPitch.Size = new Size(67, 23);
            textBoxMinPitch.TabIndex = 6;
            textBoxMinPitch.TextChanged += textBoxMinPitch_TextChanged;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(33, 101);
            label5.Name = "label5";
            label5.Size = new Size(31, 15);
            label5.TabIndex = 5;
            label5.Text = "Min:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(33, 53);
            label4.Name = "label4";
            label4.Size = new Size(62, 15);
            label4.TabIndex = 4;
            label4.Text = "Reference:";
            // 
            // radioButtonManual
            // 
            radioButtonManual.AutoSize = true;
            radioButtonManual.Location = new Point(14, 79);
            radioButtonManual.Name = "radioButtonManual";
            radioButtonManual.Size = new Size(65, 19);
            radioButtonManual.TabIndex = 3;
            radioButtonManual.TabStop = true;
            radioButtonManual.Text = "Manual";
            radioButtonManual.UseVisualStyleBackColor = true;
            radioButtonManual.CheckedChanged += radioButtonManual_CheckedChanged;
            // 
            // textBoxReference
            // 
            textBoxReference.Location = new Point(100, 50);
            textBoxReference.Name = "textBoxReference";
            textBoxReference.Size = new Size(67, 23);
            textBoxReference.TabIndex = 2;
            textBoxReference.TextChanged += textBoxReference_TextChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(3, 7);
            label3.Name = "label3";
            label3.Size = new Size(34, 15);
            label3.TabIndex = 1;
            label3.Text = "Pitch";
            // 
            // radioButtonProportional
            // 
            radioButtonProportional.AutoSize = true;
            radioButtonProportional.Location = new Point(14, 25);
            radioButtonProportional.Name = "radioButtonProportional";
            radioButtonProportional.Size = new Size(133, 19);
            radioButtonProportional.TabIndex = 0;
            radioButtonProportional.TabStop = true;
            radioButtonProportional.Text = "Proportional to RPM";
            radioButtonProportional.UseVisualStyleBackColor = true;
            radioButtonProportional.CheckedChanged += radioButtonProportional_CheckedChanged;
            // 
            // buttonMoveUp
            // 
            buttonMoveUp.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            buttonMoveUp.Location = new Point(12, 437);
            buttonMoveUp.Name = "buttonMoveUp";
            buttonMoveUp.Size = new Size(166, 23);
            buttonMoveUp.TabIndex = 11;
            buttonMoveUp.Text = "Move up";
            buttonMoveUp.UseVisualStyleBackColor = true;
            buttonMoveUp.Click += buttonMoveUp_Click;
            // 
            // buttonMoveDown
            // 
            buttonMoveDown.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            buttonMoveDown.Location = new Point(12, 466);
            buttonMoveDown.Name = "buttonMoveDown";
            buttonMoveDown.Size = new Size(166, 23);
            buttonMoveDown.TabIndex = 9;
            buttonMoveDown.Text = "Move down";
            buttonMoveDown.UseVisualStyleBackColor = true;
            buttonMoveDown.Click += buttonMoveDown_Click;
            // 
            // timer
            // 
            timer.Interval = 16;
            timer.Tick += timer_Tick;
            // 
            // label8
            // 
            label8.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label8.AutoSize = true;
            label8.Location = new Point(815, 212);
            label8.Name = "label8";
            label8.Size = new Size(93, 15);
            label8.TabIndex = 12;
            label8.Text = "Current Volume:";
            // 
            // textBoxVolume
            // 
            textBoxVolume.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            textBoxVolume.Location = new Point(917, 209);
            textBoxVolume.Name = "textBoxVolume";
            textBoxVolume.ReadOnly = true;
            textBoxVolume.Size = new Size(72, 23);
            textBoxVolume.TabIndex = 13;
            textBoxVolume.TextAlign = HorizontalAlignment.Right;
            // 
            // buttonSave
            // 
            buttonSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonSave.Location = new Point(914, 538);
            buttonSave.Name = "buttonSave";
            buttonSave.Size = new Size(75, 23);
            buttonSave.TabIndex = 14;
            buttonSave.Text = "Save";
            buttonSave.UseVisualStyleBackColor = true;
            buttonSave.Click += buttonSave_Click;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(187, 202);
            label9.Name = "label9";
            label9.Size = new Size(509, 30);
            label9.TabIndex = 15;
            label9.Text = "Use left mouse button to drag points. Shift: Lock volume, Ctrl: Lock RPM, Alt: Disable snapping.\r\nUse right mouse button to add and remove points.";
            // 
            // SCLEditor
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1001, 581);
            Controls.Add(label9);
            Controls.Add(buttonSave);
            Controls.Add(textBoxVolume);
            Controls.Add(label8);
            Controls.Add(buttonMoveDown);
            Controls.Add(buttonMoveUp);
            Controls.Add(panel1);
            Controls.Add(buttonBrowse);
            Controls.Add(textBoxWav);
            Controls.Add(label2);
            Controls.Add(plotViewEnvelope);
            Controls.Add(buttonAddBelow);
            Controls.Add(buttonAddAbove);
            Controls.Add(buttonRemove);
            Controls.Add(listBoxSamples);
            Controls.Add(label1);
            Controls.Add(comboBoxLayer);
            Name = "SCLEditor";
            Text = "SCLEditor";
            KeyDown += SCLEditor_KeyDown;
            KeyUp += SCLEditor_KeyUp;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)trackBarReference).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ComboBox comboBoxLayer;
        private Label label1;
        private ListBox listBoxSamples;
        private Button buttonRemove;
        private Button buttonAddAbove;
        private Button buttonAddBelow;
        private OxyPlot.WindowsForms.PlotView plotViewEnvelope;
        private Label label2;
        private TextBox textBoxWav;
        private Button buttonBrowse;
        private Panel panel1;
        private TextBox textBoxMinPitch;
        private Label label5;
        private Label label4;
        private RadioButton radioButtonManual;
        private TextBox textBoxReference;
        private Label label3;
        private RadioButton radioButtonProportional;
        private Label label6;
        private TextBox textBoxMaxPitch;
        private Button buttonMoveUp;
        private Button buttonMoveDown;
        private TrackBar trackBarReference;
        private Label label7;
        private System.Windows.Forms.Timer timer;
        private Label label8;
        private TextBox textBoxVolume;
        private Button buttonSave;
        private Label label9;
    }
}