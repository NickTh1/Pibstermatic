namespace WaveMix
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            buttonLiveEdit = new Button();
            tabControlControl = new TabControl();
            tabPageRaw = new TabPage();
            trackBarOn = new TrackBar();
            label2 = new Label();
            label1 = new Label();
            textBoxRPM = new TextBox();
            trackBarRPM = new TrackBar();
            tabPageSim = new TabPage();
            buttonBrake = new Button();
            checkBoxClutch = new CheckBox();
            textBoxOn = new TextBox();
            label12 = new Label();
            textBoxCurrentRPM = new TextBox();
            label11 = new Label();
            textBoxIdleRPM = new TextBox();
            label10 = new Label();
            trackBarGearing = new TrackBar();
            label9 = new Label();
            label8 = new Label();
            trackBarThrottle = new TrackBar();
            label7 = new Label();
            plotViewFFT = new OxyPlot.WindowsForms.PlotView();
            timer = new System.Windows.Forms.Timer(components);
            trackBarOverallVolume = new TrackBar();
            label3 = new Label();
            dataGridViewWavs = new DataGridView();
            plotViewWave = new OxyPlot.WindowsForms.PlotView();
            buttonDisableAll = new Button();
            buttonEnableAll = new Button();
            textBoxRMS = new TextBox();
            label4 = new Label();
            label5 = new Label();
            comboBoxStroke = new ComboBox();
            label6 = new Label();
            comboBoxCylinders = new ComboBox();
            buttonAutoMinPitchEnable = new Button();
            buttonAutoMinPitchDisable = new Button();
            buttonEditEnvelopes = new Button();
            columnWav = new DataGridViewTextBoxColumn();
            columnEnabled = new DataGridViewCheckBoxColumn();
            columnExtendedRange = new DataGridViewCheckBoxColumn();
            columnAutoMinPitch = new DataGridViewCheckBoxColumn();
            columnRecommendedMinPitch = new DataGridViewTextBoxColumn();
            columnCurrentVolume = new DataGridViewTextBoxColumn();
            tabControlControl.SuspendLayout();
            tabPageRaw.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)trackBarOn).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackBarRPM).BeginInit();
            tabPageSim.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)trackBarGearing).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackBarThrottle).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackBarOverallVolume).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridViewWavs).BeginInit();
            SuspendLayout();
            // 
            // buttonLiveEdit
            // 
            buttonLiveEdit.Location = new Point(12, 12);
            buttonLiveEdit.Name = "buttonLiveEdit";
            buttonLiveEdit.Size = new Size(85, 23);
            buttonLiveEdit.TabIndex = 1;
            buttonLiveEdit.Text = "Live Edit Text";
            buttonLiveEdit.UseVisualStyleBackColor = true;
            buttonLiveEdit.Click += buttonLiveEdit_Click;
            // 
            // tabControlControl
            // 
            tabControlControl.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tabControlControl.Controls.Add(tabPageRaw);
            tabControlControl.Controls.Add(tabPageSim);
            tabControlControl.Location = new Point(12, 199);
            tabControlControl.Name = "tabControlControl";
            tabControlControl.SelectedIndex = 0;
            tabControlControl.Size = new Size(1100, 143);
            tabControlControl.TabIndex = 2;
            // 
            // tabPageRaw
            // 
            tabPageRaw.Controls.Add(trackBarOn);
            tabPageRaw.Controls.Add(label2);
            tabPageRaw.Controls.Add(label1);
            tabPageRaw.Controls.Add(textBoxRPM);
            tabPageRaw.Controls.Add(trackBarRPM);
            tabPageRaw.Location = new Point(4, 24);
            tabPageRaw.Name = "tabPageRaw";
            tabPageRaw.Padding = new Padding(3);
            tabPageRaw.Size = new Size(1092, 115);
            tabPageRaw.TabIndex = 0;
            tabPageRaw.Text = "Raw";
            tabPageRaw.UseVisualStyleBackColor = true;
            // 
            // trackBarOn
            // 
            trackBarOn.Location = new Point(70, 57);
            trackBarOn.Maximum = 1000000;
            trackBarOn.Name = "trackBarOn";
            trackBarOn.Size = new Size(166, 45);
            trackBarOn.TabIndex = 5;
            trackBarOn.TickFrequency = 0;
            trackBarOn.TickStyle = TickStyle.None;
            trackBarOn.Scroll += trackBarOn_Scroll;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(19, 57);
            label2.Name = "label2";
            label2.Size = new Size(26, 15);
            label2.TabIndex = 4;
            label2.Text = "On:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(19, 9);
            label1.Name = "label1";
            label1.Size = new Size(35, 15);
            label1.TabIndex = 3;
            label1.Text = "RPM:";
            // 
            // textBoxRPM
            // 
            textBoxRPM.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            textBoxRPM.Location = new Point(984, 6);
            textBoxRPM.Name = "textBoxRPM";
            textBoxRPM.Size = new Size(100, 23);
            textBoxRPM.TabIndex = 1;
            textBoxRPM.TextChanged += textBoxRPM_TextChanged;
            // 
            // trackBarRPM
            // 
            trackBarRPM.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            trackBarRPM.Location = new Point(70, 6);
            trackBarRPM.Maximum = 1000000;
            trackBarRPM.Name = "trackBarRPM";
            trackBarRPM.Size = new Size(908, 45);
            trackBarRPM.TabIndex = 0;
            trackBarRPM.TickFrequency = 0;
            trackBarRPM.TickStyle = TickStyle.None;
            trackBarRPM.Scroll += trackBarRPM_Scroll;
            // 
            // tabPageSim
            // 
            tabPageSim.Controls.Add(buttonBrake);
            tabPageSim.Controls.Add(checkBoxClutch);
            tabPageSim.Controls.Add(textBoxOn);
            tabPageSim.Controls.Add(label12);
            tabPageSim.Controls.Add(textBoxCurrentRPM);
            tabPageSim.Controls.Add(label11);
            tabPageSim.Controls.Add(textBoxIdleRPM);
            tabPageSim.Controls.Add(label10);
            tabPageSim.Controls.Add(trackBarGearing);
            tabPageSim.Controls.Add(label9);
            tabPageSim.Controls.Add(label8);
            tabPageSim.Controls.Add(trackBarThrottle);
            tabPageSim.Controls.Add(label7);
            tabPageSim.Location = new Point(4, 24);
            tabPageSim.Name = "tabPageSim";
            tabPageSim.Padding = new Padding(3);
            tabPageSim.Size = new Size(1092, 115);
            tabPageSim.TabIndex = 1;
            tabPageSim.Text = "Sim";
            tabPageSim.UseVisualStyleBackColor = true;
            // 
            // buttonBrake
            // 
            buttonBrake.Location = new Point(611, 17);
            buttonBrake.Name = "buttonBrake";
            buttonBrake.Size = new Size(75, 23);
            buttonBrake.TabIndex = 12;
            buttonBrake.Text = "Brake";
            buttonBrake.UseVisualStyleBackColor = true;
            buttonBrake.MouseDown += buttonBrake_MouseDown;
            buttonBrake.MouseUp += buttonBrake_MouseUp;
            // 
            // checkBoxClutch
            // 
            checkBoxClutch.AutoSize = true;
            checkBoxClutch.Location = new Point(540, 21);
            checkBoxClutch.Name = "checkBoxClutch";
            checkBoxClutch.Size = new Size(61, 19);
            checkBoxClutch.TabIndex = 11;
            checkBoxClutch.Text = "Clutch";
            checkBoxClutch.UseVisualStyleBackColor = true;
            checkBoxClutch.CheckedChanged += checkBoxClutch_CheckedChanged;
            // 
            // textBoxOn
            // 
            textBoxOn.Location = new Point(540, 79);
            textBoxOn.Name = "textBoxOn";
            textBoxOn.ReadOnly = true;
            textBoxOn.Size = new Size(61, 23);
            textBoxOn.TabIndex = 10;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(508, 82);
            label12.Name = "label12";
            label12.Size = new Size(26, 15);
            label12.TabIndex = 9;
            label12.Text = "On:";
            // 
            // textBoxCurrentRPM
            // 
            textBoxCurrentRPM.BackColor = SystemColors.Control;
            textBoxCurrentRPM.Location = new Point(384, 79);
            textBoxCurrentRPM.Name = "textBoxCurrentRPM";
            textBoxCurrentRPM.ReadOnly = true;
            textBoxCurrentRPM.Size = new Size(80, 23);
            textBoxCurrentRPM.TabIndex = 8;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(327, 82);
            label11.Name = "label11";
            label11.Size = new Size(35, 15);
            label11.TabIndex = 7;
            label11.Text = "RPM:";
            // 
            // textBoxIdleRPM
            // 
            textBoxIdleRPM.Location = new Point(63, 79);
            textBoxIdleRPM.Name = "textBoxIdleRPM";
            textBoxIdleRPM.Size = new Size(66, 23);
            textBoxIdleRPM.TabIndex = 6;
            textBoxIdleRPM.TextChanged += textBoxIdleRPM_TextChanged;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(6, 82);
            label10.Name = "label10";
            label10.Size = new Size(57, 15);
            label10.TabIndex = 5;
            label10.Text = "Idle RPM:";
            // 
            // trackBarGearing
            // 
            trackBarGearing.Location = new Point(384, 21);
            trackBarGearing.Maximum = 1000000;
            trackBarGearing.Name = "trackBarGearing";
            trackBarGearing.Size = new Size(150, 45);
            trackBarGearing.TabIndex = 4;
            trackBarGearing.TickFrequency = 0;
            trackBarGearing.TickStyle = TickStyle.None;
            trackBarGearing.Scroll += trackBarGearing_Scroll;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(6, 3);
            label9.Name = "label9";
            label9.Size = new Size(516, 15);
            label9.TabIndex = 3;
            label9.Text = "Supremely inaccurate simulation. Do not use when planning stunt tricks or nuclear power plants.";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(327, 21);
            label8.Name = "label8";
            label8.Size = new Size(51, 15);
            label8.TabIndex = 2;
            label8.Text = "Gearing:";
            // 
            // trackBarThrottle
            // 
            trackBarThrottle.Location = new Point(63, 21);
            trackBarThrottle.Maximum = 1000000;
            trackBarThrottle.Name = "trackBarThrottle";
            trackBarThrottle.Size = new Size(229, 45);
            trackBarThrottle.TabIndex = 1;
            trackBarThrottle.TickFrequency = 0;
            trackBarThrottle.TickStyle = TickStyle.None;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(6, 21);
            label7.Name = "label7";
            label7.Size = new Size(51, 15);
            label7.TabIndex = 0;
            label7.Text = "Throttle:";
            // 
            // plotViewFFT
            // 
            plotViewFFT.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            plotViewFFT.BackColor = Color.Black;
            plotViewFFT.ForeColor = Color.White;
            plotViewFFT.Location = new Point(16, 377);
            plotViewFFT.Name = "plotViewFFT";
            plotViewFFT.PanCursor = Cursors.Hand;
            plotViewFFT.Size = new Size(790, 208);
            plotViewFFT.TabIndex = 4;
            plotViewFFT.Text = "plotView1";
            plotViewFFT.ZoomHorizontalCursor = Cursors.SizeWE;
            plotViewFFT.ZoomRectangleCursor = Cursors.SizeNWSE;
            plotViewFFT.ZoomVerticalCursor = Cursors.SizeNS;
            // 
            // trackBarOverallVolume
            // 
            trackBarOverallVolume.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            trackBarOverallVolume.Location = new Point(960, 12);
            trackBarOverallVolume.Maximum = 1000000;
            trackBarOverallVolume.Name = "trackBarOverallVolume";
            trackBarOverallVolume.Size = new Size(152, 45);
            trackBarOverallVolume.TabIndex = 5;
            trackBarOverallVolume.TickFrequency = 0;
            trackBarOverallVolume.TickStyle = TickStyle.None;
            trackBarOverallVolume.Value = 1000000;
            trackBarOverallVolume.Scroll += trackBarOverallVolume_Scroll;
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label3.AutoSize = true;
            label3.Location = new Point(867, 16);
            label3.Name = "label3";
            label3.Size = new Size(90, 15);
            label3.TabIndex = 6;
            label3.Text = "Overall volume:";
            // 
            // dataGridViewWavs
            // 
            dataGridViewWavs.AllowUserToAddRows = false;
            dataGridViewWavs.AllowUserToDeleteRows = false;
            dataGridViewWavs.AllowUserToResizeColumns = false;
            dataGridViewWavs.AllowUserToResizeRows = false;
            dataGridViewWavs.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dataGridViewWavs.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewWavs.Columns.AddRange(new DataGridViewColumn[] { columnWav, columnEnabled, columnExtendedRange, columnAutoMinPitch, columnRecommendedMinPitch, columnCurrentVolume });
            dataGridViewWavs.Location = new Point(12, 41);
            dataGridViewWavs.Name = "dataGridViewWavs";
            dataGridViewWavs.RowHeadersVisible = false;
            dataGridViewWavs.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewWavs.RowTemplate.Height = 25;
            dataGridViewWavs.ScrollBars = ScrollBars.Vertical;
            dataGridViewWavs.Size = new Size(965, 152);
            dataGridViewWavs.TabIndex = 8;
            dataGridViewWavs.CellDoubleClick += dataGridViewWavs_CellDoubleClick;
            dataGridViewWavs.CellStateChanged += dataGridViewWavs_CellStateChanged;
            dataGridViewWavs.CellValueChanged += dataGridViewWavs_CellValueChanged;
            dataGridViewWavs.ColumnHeaderMouseClick += dataGridViewWavs_ColumnHeaderMouseClick;
            dataGridViewWavs.CurrentCellDirtyStateChanged += dataGridViewWavs_CurrentCellDirtyStateChanged;
            // 
            // plotViewWave
            // 
            plotViewWave.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            plotViewWave.BackColor = Color.Black;
            plotViewWave.Location = new Point(832, 377);
            plotViewWave.Name = "plotViewWave";
            plotViewWave.PanCursor = Cursors.Hand;
            plotViewWave.Size = new Size(268, 208);
            plotViewWave.TabIndex = 9;
            plotViewWave.Text = "plotViewWave";
            plotViewWave.ZoomHorizontalCursor = Cursors.SizeWE;
            plotViewWave.ZoomRectangleCursor = Cursors.SizeNWSE;
            plotViewWave.ZoomVerticalCursor = Cursors.SizeNS;
            // 
            // buttonDisableAll
            // 
            buttonDisableAll.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonDisableAll.Location = new Point(983, 70);
            buttonDisableAll.Name = "buttonDisableAll";
            buttonDisableAll.Size = new Size(129, 23);
            buttonDisableAll.TabIndex = 10;
            buttonDisableAll.Text = "Disable All";
            buttonDisableAll.UseVisualStyleBackColor = true;
            buttonDisableAll.Click += buttonDisableAll_Click;
            // 
            // buttonEnableAll
            // 
            buttonEnableAll.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonEnableAll.Location = new Point(983, 41);
            buttonEnableAll.Name = "buttonEnableAll";
            buttonEnableAll.Size = new Size(129, 23);
            buttonEnableAll.TabIndex = 11;
            buttonEnableAll.Text = "Enable All";
            buttonEnableAll.UseVisualStyleBackColor = true;
            buttonEnableAll.Click += buttonEnableAll_Click;
            // 
            // textBoxRMS
            // 
            textBoxRMS.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            textBoxRMS.BackColor = SystemColors.ControlLight;
            textBoxRMS.Location = new Point(872, 348);
            textBoxRMS.Name = "textBoxRMS";
            textBoxRMS.Size = new Size(58, 23);
            textBoxRMS.TabIndex = 12;
            textBoxRMS.TextAlign = HorizontalAlignment.Right;
            // 
            // label4
            // 
            label4.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            label4.AutoSize = true;
            label4.Location = new Point(832, 351);
            label4.Name = "label4";
            label4.Size = new Size(34, 15);
            label4.TabIndex = 13;
            label4.Text = "RMS:";
            // 
            // label5
            // 
            label5.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            label5.AutoSize = true;
            label5.Location = new Point(16, 351);
            label5.Name = "label5";
            label5.Size = new Size(43, 15);
            label5.TabIndex = 14;
            label5.Text = "Stroke:";
            // 
            // comboBoxStroke
            // 
            comboBoxStroke.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            comboBoxStroke.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxStroke.FormattingEnabled = true;
            comboBoxStroke.Items.AddRange(new object[] { "2", "4" });
            comboBoxStroke.Location = new Point(65, 348);
            comboBoxStroke.Name = "comboBoxStroke";
            comboBoxStroke.Size = new Size(54, 23);
            comboBoxStroke.TabIndex = 15;
            comboBoxStroke.SelectedIndexChanged += comboBoxStroke_SelectedIndexChanged;
            // 
            // label6
            // 
            label6.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            label6.AutoSize = true;
            label6.Location = new Point(139, 351);
            label6.Name = "label6";
            label6.Size = new Size(59, 15);
            label6.TabIndex = 16;
            label6.Text = "Cylinders:";
            // 
            // comboBoxCylinders
            // 
            comboBoxCylinders.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            comboBoxCylinders.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxCylinders.FormattingEnabled = true;
            comboBoxCylinders.Items.AddRange(new object[] { "1", "2", "3", "4", "6" });
            comboBoxCylinders.Location = new Point(204, 348);
            comboBoxCylinders.Name = "comboBoxCylinders";
            comboBoxCylinders.Size = new Size(61, 23);
            comboBoxCylinders.TabIndex = 17;
            comboBoxCylinders.SelectedIndexChanged += comboBoxCylinders_SelectedIndexChanged;
            // 
            // buttonAutoMinPitchEnable
            // 
            buttonAutoMinPitchEnable.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonAutoMinPitchEnable.Location = new Point(983, 99);
            buttonAutoMinPitchEnable.Name = "buttonAutoMinPitchEnable";
            buttonAutoMinPitchEnable.Size = new Size(129, 23);
            buttonAutoMinPitchEnable.TabIndex = 18;
            buttonAutoMinPitchEnable.Text = "Auto MinPitch All";
            buttonAutoMinPitchEnable.UseVisualStyleBackColor = true;
            buttonAutoMinPitchEnable.Click += buttonAutoMinPitchEnable_Click;
            // 
            // buttonAutoMinPitchDisable
            // 
            buttonAutoMinPitchDisable.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonAutoMinPitchDisable.Location = new Point(983, 128);
            buttonAutoMinPitchDisable.Name = "buttonAutoMinPitchDisable";
            buttonAutoMinPitchDisable.Size = new Size(129, 23);
            buttonAutoMinPitchDisable.TabIndex = 19;
            buttonAutoMinPitchDisable.Text = "Auto MinPitch None";
            buttonAutoMinPitchDisable.UseVisualStyleBackColor = true;
            buttonAutoMinPitchDisable.Click += buttonAutoMinPitchDisable_Click;
            // 
            // buttonEditEnvelopes
            // 
            buttonEditEnvelopes.Location = new Point(103, 12);
            buttonEditEnvelopes.Name = "buttonEditEnvelopes";
            buttonEditEnvelopes.Size = new Size(95, 23);
            buttonEditEnvelopes.TabIndex = 20;
            buttonEditEnvelopes.Text = "Edit Envelopes";
            buttonEditEnvelopes.UseVisualStyleBackColor = true;
            buttonEditEnvelopes.Click += buttonEditEnvelopes_Click;
            // 
            // columnWav
            // 
            columnWav.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            columnWav.HeaderText = "Wav";
            columnWav.Name = "columnWav";
            columnWav.ReadOnly = true;
            columnWav.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // columnEnabled
            // 
            columnEnabled.HeaderText = "Enabled";
            columnEnabled.Name = "columnEnabled";
            columnEnabled.Width = 60;
            // 
            // columnExtendedRange
            // 
            columnExtendedRange.HeaderText = "Extended Range";
            columnExtendedRange.Name = "columnExtendedRange";
            columnExtendedRange.Width = 60;
            // 
            // columnAutoMinPitch
            // 
            columnAutoMinPitch.HeaderText = "Auto MinPitch";
            columnAutoMinPitch.Name = "columnAutoMinPitch";
            columnAutoMinPitch.Width = 60;
            // 
            // columnRecommendedMinPitch
            // 
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleRight;
            columnRecommendedMinPitch.DefaultCellStyle = dataGridViewCellStyle1;
            columnRecommendedMinPitch.HeaderText = "Recommended MinPitch";
            columnRecommendedMinPitch.Name = "columnRecommendedMinPitch";
            columnRecommendedMinPitch.ReadOnly = true;
            columnRecommendedMinPitch.SortMode = DataGridViewColumnSortMode.NotSortable;
            columnRecommendedMinPitch.Width = 95;
            // 
            // columnCurrentVolume
            // 
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleRight;
            columnCurrentVolume.DefaultCellStyle = dataGridViewCellStyle2;
            columnCurrentVolume.HeaderText = "Current Volume";
            columnCurrentVolume.Name = "columnCurrentVolume";
            columnCurrentVolume.ReadOnly = true;
            columnCurrentVolume.SortMode = DataGridViewColumnSortMode.NotSortable;
            columnCurrentVolume.Width = 60;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1124, 620);
            Controls.Add(buttonEditEnvelopes);
            Controls.Add(buttonAutoMinPitchDisable);
            Controls.Add(buttonAutoMinPitchEnable);
            Controls.Add(comboBoxCylinders);
            Controls.Add(label6);
            Controls.Add(comboBoxStroke);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(textBoxRMS);
            Controls.Add(buttonEnableAll);
            Controls.Add(buttonDisableAll);
            Controls.Add(plotViewWave);
            Controls.Add(dataGridViewWavs);
            Controls.Add(label3);
            Controls.Add(trackBarOverallVolume);
            Controls.Add(plotViewFFT);
            Controls.Add(tabControlControl);
            Controls.Add(buttonLiveEdit);
            Name = "MainForm";
            Text = "Pibstermatic Sound 2000";
            tabControlControl.ResumeLayout(false);
            tabPageRaw.ResumeLayout(false);
            tabPageRaw.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)trackBarOn).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackBarRPM).EndInit();
            tabPageSim.ResumeLayout(false);
            tabPageSim.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)trackBarGearing).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackBarThrottle).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackBarOverallVolume).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridViewWavs).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button buttonLiveEdit;
        private TabControl tabControlControl;
        private TabPage tabPageRaw;
        private TrackBar trackBarOn;
        private Label label2;
        private Label label1;
        private TextBox textBoxRPM;
        private TrackBar trackBarRPM;
        private TabPage tabPageSim;
        private OxyPlot.WindowsForms.PlotView plotViewFFT;
        private System.Windows.Forms.Timer timer;
        private TrackBar trackBarOverallVolume;
        private Label label3;
        private DataGridView dataGridViewWavs;
        private OxyPlot.WindowsForms.PlotView plotViewWave;
        private Button buttonDisableAll;
        private Button buttonEnableAll;
        private TextBox textBoxRMS;
        private Label label4;
        private Label label5;
        private ComboBox comboBoxStroke;
        private Label label6;
        private ComboBox comboBoxCylinders;
        private Button buttonAutoMinPitchEnable;
        private Button buttonAutoMinPitchDisable;
        private TrackBar trackBarGearing;
        private Label label9;
        private Label label8;
        private TrackBar trackBarThrottle;
        private Label label7;
        private TextBox textBoxIdleRPM;
        private Label label10;
        private TextBox textBoxCurrentRPM;
        private Label label11;
        private TextBox textBoxOn;
        private Label label12;
        private CheckBox checkBoxClutch;
        private Button buttonBrake;
        private Button buttonEditEnvelopes;
        private DataGridViewTextBoxColumn columnWav;
        private DataGridViewCheckBoxColumn columnEnabled;
        private DataGridViewCheckBoxColumn columnExtendedRange;
        private DataGridViewCheckBoxColumn columnAutoMinPitch;
        private DataGridViewTextBoxColumn columnRecommendedMinPitch;
        private DataGridViewTextBoxColumn columnCurrentVolume;
    }
}