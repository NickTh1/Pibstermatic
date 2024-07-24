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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.buttonLiveEdit = new System.Windows.Forms.Button();
            this.tabControlControl = new System.Windows.Forms.TabControl();
            this.tabPageRaw = new System.Windows.Forms.TabPage();
            this.trackBarOn = new System.Windows.Forms.TrackBar();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxRPM = new System.Windows.Forms.TextBox();
            this.trackBarRPM = new System.Windows.Forms.TrackBar();
            this.tabPageSim = new System.Windows.Forms.TabPage();
            this.buttonBrake = new System.Windows.Forms.Button();
            this.checkBoxClutch = new System.Windows.Forms.CheckBox();
            this.textBoxOn = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.textBoxCurrentRPM = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.textBoxIdleRPM = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.trackBarGearing = new System.Windows.Forms.TrackBar();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.trackBarThrottle = new System.Windows.Forms.TrackBar();
            this.label7 = new System.Windows.Forms.Label();
            this.plotViewFFT = new OxyPlot.WindowsForms.PlotView();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.trackBarOverallVolume = new System.Windows.Forms.TrackBar();
            this.label3 = new System.Windows.Forms.Label();
            this.dataGridViewWavs = new System.Windows.Forms.DataGridView();
            this.columnWav = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnEnabled = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.columnExtendedRange = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.columnAutoMinPitch = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.columnRecommendedMinPitch = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnCurrentVolume = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.plotViewWave = new OxyPlot.WindowsForms.PlotView();
            this.buttonDisableAll = new System.Windows.Forms.Button();
            this.buttonEnableAll = new System.Windows.Forms.Button();
            this.textBoxRMS = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.comboBoxStroke = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.comboBoxCylinders = new System.Windows.Forms.ComboBox();
            this.buttonAutoMinPitchEnable = new System.Windows.Forms.Button();
            this.buttonAutoMinPitchDisable = new System.Windows.Forms.Button();
            this.tabControlControl.SuspendLayout();
            this.tabPageRaw.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarOn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarRPM)).BeginInit();
            this.tabPageSim.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarGearing)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarThrottle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarOverallVolume)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewWavs)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonLiveEdit
            // 
            this.buttonLiveEdit.Location = new System.Drawing.Point(12, 12);
            this.buttonLiveEdit.Name = "buttonLiveEdit";
            this.buttonLiveEdit.Size = new System.Drawing.Size(75, 23);
            this.buttonLiveEdit.TabIndex = 1;
            this.buttonLiveEdit.Text = "Live Edit";
            this.buttonLiveEdit.UseVisualStyleBackColor = true;
            this.buttonLiveEdit.Click += new System.EventHandler(this.buttonLiveEdit_Click);
            // 
            // tabControlControl
            // 
            this.tabControlControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlControl.Controls.Add(this.tabPageRaw);
            this.tabControlControl.Controls.Add(this.tabPageSim);
            this.tabControlControl.Location = new System.Drawing.Point(12, 199);
            this.tabControlControl.Name = "tabControlControl";
            this.tabControlControl.SelectedIndex = 0;
            this.tabControlControl.Size = new System.Drawing.Size(1100, 143);
            this.tabControlControl.TabIndex = 2;
            // 
            // tabPageRaw
            // 
            this.tabPageRaw.Controls.Add(this.trackBarOn);
            this.tabPageRaw.Controls.Add(this.label2);
            this.tabPageRaw.Controls.Add(this.label1);
            this.tabPageRaw.Controls.Add(this.textBoxRPM);
            this.tabPageRaw.Controls.Add(this.trackBarRPM);
            this.tabPageRaw.Location = new System.Drawing.Point(4, 24);
            this.tabPageRaw.Name = "tabPageRaw";
            this.tabPageRaw.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageRaw.Size = new System.Drawing.Size(1092, 115);
            this.tabPageRaw.TabIndex = 0;
            this.tabPageRaw.Text = "Raw";
            this.tabPageRaw.UseVisualStyleBackColor = true;
            // 
            // trackBarOn
            // 
            this.trackBarOn.Location = new System.Drawing.Point(70, 57);
            this.trackBarOn.Maximum = 1000000;
            this.trackBarOn.Name = "trackBarOn";
            this.trackBarOn.Size = new System.Drawing.Size(166, 45);
            this.trackBarOn.TabIndex = 5;
            this.trackBarOn.TickFrequency = 0;
            this.trackBarOn.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBarOn.Scroll += new System.EventHandler(this.trackBarOn_Scroll);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(26, 15);
            this.label2.TabIndex = 4;
            this.label2.Text = "On:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "RPM:";
            // 
            // textBoxRPM
            // 
            this.textBoxRPM.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxRPM.Location = new System.Drawing.Point(984, 6);
            this.textBoxRPM.Name = "textBoxRPM";
            this.textBoxRPM.Size = new System.Drawing.Size(100, 23);
            this.textBoxRPM.TabIndex = 1;
            this.textBoxRPM.TextChanged += new System.EventHandler(this.textBoxRPM_TextChanged);
            // 
            // trackBarRPM
            // 
            this.trackBarRPM.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBarRPM.Location = new System.Drawing.Point(70, 6);
            this.trackBarRPM.Maximum = 1000000;
            this.trackBarRPM.Name = "trackBarRPM";
            this.trackBarRPM.Size = new System.Drawing.Size(908, 45);
            this.trackBarRPM.TabIndex = 0;
            this.trackBarRPM.TickFrequency = 0;
            this.trackBarRPM.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBarRPM.Scroll += new System.EventHandler(this.trackBarRPM_Scroll);
            // 
            // tabPageSim
            // 
            this.tabPageSim.Controls.Add(this.buttonBrake);
            this.tabPageSim.Controls.Add(this.checkBoxClutch);
            this.tabPageSim.Controls.Add(this.textBoxOn);
            this.tabPageSim.Controls.Add(this.label12);
            this.tabPageSim.Controls.Add(this.textBoxCurrentRPM);
            this.tabPageSim.Controls.Add(this.label11);
            this.tabPageSim.Controls.Add(this.textBoxIdleRPM);
            this.tabPageSim.Controls.Add(this.label10);
            this.tabPageSim.Controls.Add(this.trackBarGearing);
            this.tabPageSim.Controls.Add(this.label9);
            this.tabPageSim.Controls.Add(this.label8);
            this.tabPageSim.Controls.Add(this.trackBarThrottle);
            this.tabPageSim.Controls.Add(this.label7);
            this.tabPageSim.Location = new System.Drawing.Point(4, 24);
            this.tabPageSim.Name = "tabPageSim";
            this.tabPageSim.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSim.Size = new System.Drawing.Size(1092, 115);
            this.tabPageSim.TabIndex = 1;
            this.tabPageSim.Text = "Sim";
            this.tabPageSim.UseVisualStyleBackColor = true;
            // 
            // buttonBrake
            // 
            this.buttonBrake.Location = new System.Drawing.Point(611, 17);
            this.buttonBrake.Name = "buttonBrake";
            this.buttonBrake.Size = new System.Drawing.Size(75, 23);
            this.buttonBrake.TabIndex = 12;
            this.buttonBrake.Text = "Brake";
            this.buttonBrake.UseVisualStyleBackColor = true;
            this.buttonBrake.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonBrake_MouseDown);
            this.buttonBrake.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonBrake_MouseUp);
            // 
            // checkBoxClutch
            // 
            this.checkBoxClutch.AutoSize = true;
            this.checkBoxClutch.Location = new System.Drawing.Point(540, 21);
            this.checkBoxClutch.Name = "checkBoxClutch";
            this.checkBoxClutch.Size = new System.Drawing.Size(61, 19);
            this.checkBoxClutch.TabIndex = 11;
            this.checkBoxClutch.Text = "Clutch";
            this.checkBoxClutch.UseVisualStyleBackColor = true;
            this.checkBoxClutch.CheckedChanged += new System.EventHandler(this.checkBoxClutch_CheckedChanged);
            // 
            // textBoxOn
            // 
            this.textBoxOn.Location = new System.Drawing.Point(540, 79);
            this.textBoxOn.Name = "textBoxOn";
            this.textBoxOn.ReadOnly = true;
            this.textBoxOn.Size = new System.Drawing.Size(61, 23);
            this.textBoxOn.TabIndex = 10;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(508, 82);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(26, 15);
            this.label12.TabIndex = 9;
            this.label12.Text = "On:";
            // 
            // textBoxCurrentRPM
            // 
            this.textBoxCurrentRPM.BackColor = System.Drawing.SystemColors.Control;
            this.textBoxCurrentRPM.Location = new System.Drawing.Point(384, 79);
            this.textBoxCurrentRPM.Name = "textBoxCurrentRPM";
            this.textBoxCurrentRPM.ReadOnly = true;
            this.textBoxCurrentRPM.Size = new System.Drawing.Size(80, 23);
            this.textBoxCurrentRPM.TabIndex = 8;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(327, 82);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(35, 15);
            this.label11.TabIndex = 7;
            this.label11.Text = "RPM:";
            // 
            // textBoxIdleRPM
            // 
            this.textBoxIdleRPM.Location = new System.Drawing.Point(63, 79);
            this.textBoxIdleRPM.Name = "textBoxIdleRPM";
            this.textBoxIdleRPM.Size = new System.Drawing.Size(66, 23);
            this.textBoxIdleRPM.TabIndex = 6;
            this.textBoxIdleRPM.TextChanged += new System.EventHandler(this.textBoxIdleRPM_TextChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 82);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(57, 15);
            this.label10.TabIndex = 5;
            this.label10.Text = "Idle RPM:";
            // 
            // trackBarGearing
            // 
            this.trackBarGearing.Location = new System.Drawing.Point(384, 21);
            this.trackBarGearing.Maximum = 1000000;
            this.trackBarGearing.Name = "trackBarGearing";
            this.trackBarGearing.Size = new System.Drawing.Size(150, 45);
            this.trackBarGearing.TabIndex = 4;
            this.trackBarGearing.TickFrequency = 0;
            this.trackBarGearing.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBarGearing.Scroll += new System.EventHandler(this.trackBarGearing_Scroll);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 3);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(516, 15);
            this.label9.TabIndex = 3;
            this.label9.Text = "Supremely inaccurate simulation. Do not use when planning stunt tricks or nuclear" +
    " power plants.";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(327, 21);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(51, 15);
            this.label8.TabIndex = 2;
            this.label8.Text = "Gearing:";
            // 
            // trackBarThrottle
            // 
            this.trackBarThrottle.Location = new System.Drawing.Point(63, 21);
            this.trackBarThrottle.Maximum = 1000000;
            this.trackBarThrottle.Name = "trackBarThrottle";
            this.trackBarThrottle.Size = new System.Drawing.Size(229, 45);
            this.trackBarThrottle.TabIndex = 1;
            this.trackBarThrottle.TickFrequency = 0;
            this.trackBarThrottle.TickStyle = System.Windows.Forms.TickStyle.None;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 21);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(51, 15);
            this.label7.TabIndex = 0;
            this.label7.Text = "Throttle:";
            // 
            // plotViewFFT
            // 
            this.plotViewFFT.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.plotViewFFT.BackColor = System.Drawing.Color.Black;
            this.plotViewFFT.ForeColor = System.Drawing.Color.White;
            this.plotViewFFT.Location = new System.Drawing.Point(16, 377);
            this.plotViewFFT.Name = "plotViewFFT";
            this.plotViewFFT.PanCursor = System.Windows.Forms.Cursors.Hand;
            this.plotViewFFT.Size = new System.Drawing.Size(790, 208);
            this.plotViewFFT.TabIndex = 4;
            this.plotViewFFT.Text = "plotView1";
            this.plotViewFFT.ZoomHorizontalCursor = System.Windows.Forms.Cursors.SizeWE;
            this.plotViewFFT.ZoomRectangleCursor = System.Windows.Forms.Cursors.SizeNWSE;
            this.plotViewFFT.ZoomVerticalCursor = System.Windows.Forms.Cursors.SizeNS;
            // 
            // trackBarOverallVolume
            // 
            this.trackBarOverallVolume.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBarOverallVolume.Location = new System.Drawing.Point(960, 12);
            this.trackBarOverallVolume.Maximum = 1000000;
            this.trackBarOverallVolume.Name = "trackBarOverallVolume";
            this.trackBarOverallVolume.Size = new System.Drawing.Size(152, 45);
            this.trackBarOverallVolume.TabIndex = 5;
            this.trackBarOverallVolume.TickFrequency = 0;
            this.trackBarOverallVolume.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBarOverallVolume.Value = 1000000;
            this.trackBarOverallVolume.Scroll += new System.EventHandler(this.trackBarOverallVolume_Scroll);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(867, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(90, 15);
            this.label3.TabIndex = 6;
            this.label3.Text = "Overall volume:";
            // 
            // dataGridViewWavs
            // 
            this.dataGridViewWavs.AllowUserToAddRows = false;
            this.dataGridViewWavs.AllowUserToDeleteRows = false;
            this.dataGridViewWavs.AllowUserToResizeColumns = false;
            this.dataGridViewWavs.AllowUserToResizeRows = false;
            this.dataGridViewWavs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewWavs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewWavs.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.columnWav,
            this.columnEnabled,
            this.columnExtendedRange,
            this.columnAutoMinPitch,
            this.columnRecommendedMinPitch,
            this.columnCurrentVolume});
            this.dataGridViewWavs.Location = new System.Drawing.Point(12, 41);
            this.dataGridViewWavs.Name = "dataGridViewWavs";
            this.dataGridViewWavs.RowHeadersVisible = false;
            this.dataGridViewWavs.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridViewWavs.RowTemplate.Height = 25;
            this.dataGridViewWavs.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridViewWavs.Size = new System.Drawing.Size(965, 152);
            this.dataGridViewWavs.TabIndex = 8;
            this.dataGridViewWavs.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewWavs_CellDoubleClick);
            this.dataGridViewWavs.CellStateChanged += new System.Windows.Forms.DataGridViewCellStateChangedEventHandler(this.dataGridViewWavs_CellStateChanged);
            this.dataGridViewWavs.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewWavs_CellValueChanged);
            this.dataGridViewWavs.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewWavs_ColumnHeaderMouseClick);
            this.dataGridViewWavs.CurrentCellDirtyStateChanged += new System.EventHandler(this.dataGridViewWavs_CurrentCellDirtyStateChanged);
            // 
            // columnWav
            // 
            this.columnWav.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.columnWav.HeaderText = "Wav";
            this.columnWav.Name = "columnWav";
            this.columnWav.ReadOnly = true;
            this.columnWav.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // columnEnabled
            // 
            this.columnEnabled.HeaderText = "Enabled";
            this.columnEnabled.Name = "columnEnabled";
            this.columnEnabled.Width = 60;
            // 
            // columnExtendedRange
            // 
            this.columnExtendedRange.HeaderText = "Extended Range";
            this.columnExtendedRange.Name = "columnExtendedRange";
            this.columnExtendedRange.Width = 60;
            // 
            // columnAutoMinPitch
            // 
            this.columnAutoMinPitch.HeaderText = "Auto MinPitch";
            this.columnAutoMinPitch.Name = "columnAutoMinPitch";
            this.columnAutoMinPitch.Width = 60;
            // 
            // columnRecommendedMinPitch
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.columnRecommendedMinPitch.DefaultCellStyle = dataGridViewCellStyle3;
            this.columnRecommendedMinPitch.HeaderText = "Recommended MinPitch";
            this.columnRecommendedMinPitch.Name = "columnRecommendedMinPitch";
            this.columnRecommendedMinPitch.ReadOnly = true;
            this.columnRecommendedMinPitch.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.columnRecommendedMinPitch.Width = 95;
            // 
            // columnCurrentVolume
            // 
            this.columnCurrentVolume.HeaderText = "Current Volume";
            this.columnCurrentVolume.Name = "columnCurrentVolume";
            this.columnCurrentVolume.ReadOnly = true;
            this.columnCurrentVolume.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.columnCurrentVolume.Width = 60;
            // 
            // plotViewWave
            // 
            this.plotViewWave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.plotViewWave.BackColor = System.Drawing.Color.Black;
            this.plotViewWave.Location = new System.Drawing.Point(832, 377);
            this.plotViewWave.Name = "plotViewWave";
            this.plotViewWave.PanCursor = System.Windows.Forms.Cursors.Hand;
            this.plotViewWave.Size = new System.Drawing.Size(268, 208);
            this.plotViewWave.TabIndex = 9;
            this.plotViewWave.Text = "plotViewWave";
            this.plotViewWave.ZoomHorizontalCursor = System.Windows.Forms.Cursors.SizeWE;
            this.plotViewWave.ZoomRectangleCursor = System.Windows.Forms.Cursors.SizeNWSE;
            this.plotViewWave.ZoomVerticalCursor = System.Windows.Forms.Cursors.SizeNS;
            // 
            // buttonDisableAll
            // 
            this.buttonDisableAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDisableAll.Location = new System.Drawing.Point(983, 70);
            this.buttonDisableAll.Name = "buttonDisableAll";
            this.buttonDisableAll.Size = new System.Drawing.Size(129, 23);
            this.buttonDisableAll.TabIndex = 10;
            this.buttonDisableAll.Text = "Disable All";
            this.buttonDisableAll.UseVisualStyleBackColor = true;
            this.buttonDisableAll.Click += new System.EventHandler(this.buttonDisableAll_Click);
            // 
            // buttonEnableAll
            // 
            this.buttonEnableAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonEnableAll.Location = new System.Drawing.Point(983, 41);
            this.buttonEnableAll.Name = "buttonEnableAll";
            this.buttonEnableAll.Size = new System.Drawing.Size(129, 23);
            this.buttonEnableAll.TabIndex = 11;
            this.buttonEnableAll.Text = "Enable All";
            this.buttonEnableAll.UseVisualStyleBackColor = true;
            this.buttonEnableAll.Click += new System.EventHandler(this.buttonEnableAll_Click);
            // 
            // textBoxRMS
            // 
            this.textBoxRMS.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxRMS.BackColor = System.Drawing.SystemColors.ControlLight;
            this.textBoxRMS.Location = new System.Drawing.Point(872, 348);
            this.textBoxRMS.Name = "textBoxRMS";
            this.textBoxRMS.Size = new System.Drawing.Size(58, 23);
            this.textBoxRMS.TabIndex = 12;
            this.textBoxRMS.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(832, 351);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(34, 15);
            this.label4.TabIndex = 13;
            this.label4.Text = "RMS:";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(16, 351);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(43, 15);
            this.label5.TabIndex = 14;
            this.label5.Text = "Stroke:";
            // 
            // comboBoxStroke
            // 
            this.comboBoxStroke.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.comboBoxStroke.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxStroke.FormattingEnabled = true;
            this.comboBoxStroke.Items.AddRange(new object[] {
            "2",
            "4"});
            this.comboBoxStroke.Location = new System.Drawing.Point(65, 348);
            this.comboBoxStroke.Name = "comboBoxStroke";
            this.comboBoxStroke.Size = new System.Drawing.Size(54, 23);
            this.comboBoxStroke.TabIndex = 15;
            this.comboBoxStroke.SelectedIndexChanged += new System.EventHandler(this.comboBoxStroke_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(139, 351);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(59, 15);
            this.label6.TabIndex = 16;
            this.label6.Text = "Cylinders:";
            // 
            // comboBoxCylinders
            // 
            this.comboBoxCylinders.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.comboBoxCylinders.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxCylinders.FormattingEnabled = true;
            this.comboBoxCylinders.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "6"});
            this.comboBoxCylinders.Location = new System.Drawing.Point(204, 348);
            this.comboBoxCylinders.Name = "comboBoxCylinders";
            this.comboBoxCylinders.Size = new System.Drawing.Size(61, 23);
            this.comboBoxCylinders.TabIndex = 17;
            this.comboBoxCylinders.SelectedIndexChanged += new System.EventHandler(this.comboBoxCylinders_SelectedIndexChanged);
            // 
            // buttonAutoMinPitchEnable
            // 
            this.buttonAutoMinPitchEnable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAutoMinPitchEnable.Location = new System.Drawing.Point(983, 99);
            this.buttonAutoMinPitchEnable.Name = "buttonAutoMinPitchEnable";
            this.buttonAutoMinPitchEnable.Size = new System.Drawing.Size(129, 23);
            this.buttonAutoMinPitchEnable.TabIndex = 18;
            this.buttonAutoMinPitchEnable.Text = "Auto MinPitch All";
            this.buttonAutoMinPitchEnable.UseVisualStyleBackColor = true;
            this.buttonAutoMinPitchEnable.Click += new System.EventHandler(this.buttonAutoMinPitchEnable_Click);
            // 
            // buttonAutoMinPitchDisable
            // 
            this.buttonAutoMinPitchDisable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAutoMinPitchDisable.Location = new System.Drawing.Point(983, 128);
            this.buttonAutoMinPitchDisable.Name = "buttonAutoMinPitchDisable";
            this.buttonAutoMinPitchDisable.Size = new System.Drawing.Size(129, 23);
            this.buttonAutoMinPitchDisable.TabIndex = 19;
            this.buttonAutoMinPitchDisable.Text = "Auto MinPitch None";
            this.buttonAutoMinPitchDisable.UseVisualStyleBackColor = true;
            this.buttonAutoMinPitchDisable.Click += new System.EventHandler(this.buttonAutoMinPitchDisable_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1124, 620);
            this.Controls.Add(this.buttonAutoMinPitchDisable);
            this.Controls.Add(this.buttonAutoMinPitchEnable);
            this.Controls.Add(this.comboBoxCylinders);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.comboBoxStroke);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBoxRMS);
            this.Controls.Add(this.buttonEnableAll);
            this.Controls.Add(this.buttonDisableAll);
            this.Controls.Add(this.plotViewWave);
            this.Controls.Add(this.dataGridViewWavs);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.trackBarOverallVolume);
            this.Controls.Add(this.plotViewFFT);
            this.Controls.Add(this.tabControlControl);
            this.Controls.Add(this.buttonLiveEdit);
            this.Name = "MainForm";
            this.Text = "Pibstermatic Sound 2000";
            this.tabControlControl.ResumeLayout(false);
            this.tabPageRaw.ResumeLayout(false);
            this.tabPageRaw.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarOn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarRPM)).EndInit();
            this.tabPageSim.ResumeLayout(false);
            this.tabPageSim.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarGearing)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarThrottle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarOverallVolume)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewWavs)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

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
        private DataGridViewTextBoxColumn columnWav;
        private DataGridViewCheckBoxColumn columnEnabled;
        private DataGridViewCheckBoxColumn columnExtendedRange;
        private DataGridViewCheckBoxColumn columnAutoMinPitch;
        private DataGridViewTextBoxColumn columnRecommendedMinPitch;
        private DataGridViewTextBoxColumn columnCurrentVolume;
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
    }
}