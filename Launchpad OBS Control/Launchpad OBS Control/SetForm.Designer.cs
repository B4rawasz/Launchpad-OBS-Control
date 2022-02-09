namespace Launchpad_OBS_Control
{
    partial class SetForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SetForm));
            this.applyButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.idLabel = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.FadeLightCheckBox = new System.Windows.Forms.CheckBox();
            this.BlinkLightCheckBox = new System.Windows.Forms.CheckBox();
            this.StaticLightCheckBox = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.Color2NumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.Color1NumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.HelpButton = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.VirtualCamCheckBox = new System.Windows.Forms.CheckBox();
            this.RecCheckBox = new System.Windows.Forms.CheckBox();
            this.StreamCheckBox = new System.Windows.Forms.CheckBox();
            this.SceneListBox = new System.Windows.Forms.ListBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.SceneCheckBox = new System.Windows.Forms.CheckBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.StopMediaCheckBox = new System.Windows.Forms.CheckBox();
            this.RestartMediaCheckBox = new System.Windows.Forms.CheckBox();
            this.PlayPauseCheckBox = new System.Windows.Forms.CheckBox();
            this.VisibilityOFFCheckBox = new System.Windows.Forms.CheckBox();
            this.VisibilityONCheckBox = new System.Windows.Forms.CheckBox();
            this.MediaCheckBox = new System.Windows.Forms.CheckBox();
            this.MediaTabControl = new System.Windows.Forms.TabControl();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.MuteListBox = new System.Windows.Forms.ListBox();
            this.MuteCheckBox = new System.Windows.Forms.CheckBox();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.VolumeNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.VolumeTrackBar = new System.Windows.Forms.TrackBar();
            this.TestSoundButton = new System.Windows.Forms.Button();
            this.FileTextBox = new System.Windows.Forms.TextBox();
            this.FileButton = new System.Windows.Forms.Button();
            this.SoundCheckBox = new System.Windows.Forms.CheckBox();
            this.ClearButton = new System.Windows.Forms.Button();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Color2NumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Color1NumericUpDown)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.VolumeNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.VolumeTrackBar)).BeginInit();
            this.groupBox9.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // applyButton
            // 
            this.applyButton.Location = new System.Drawing.Point(753, 460);
            this.applyButton.Name = "applyButton";
            this.applyButton.Size = new System.Drawing.Size(75, 23);
            this.applyButton.TabIndex = 0;
            this.applyButton.Text = "Apply";
            this.applyButton.UseVisualStyleBackColor = true;
            this.applyButton.Click += new System.EventHandler(this.applyButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(753, 489);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 1;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // idLabel
            // 
            this.idLabel.AutoSize = true;
            this.idLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.idLabel.Location = new System.Drawing.Point(63, 16);
            this.idLabel.Name = "idLabel";
            this.idLabel.Size = new System.Drawing.Size(74, 17);
            this.idLabel.TabIndex = 2;
            this.idLabel.Text = "Pad ID: 00";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.FadeLightCheckBox);
            this.panel3.Controls.Add(this.BlinkLightCheckBox);
            this.panel3.Controls.Add(this.StaticLightCheckBox);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Location = new System.Drawing.Point(7, 74);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(193, 83);
            this.panel3.TabIndex = 5;
            // 
            // FadeLightCheckBox
            // 
            this.FadeLightCheckBox.AutoSize = true;
            this.FadeLightCheckBox.Location = new System.Drawing.Point(3, 62);
            this.FadeLightCheckBox.Name = "FadeLightCheckBox";
            this.FadeLightCheckBox.Size = new System.Drawing.Size(50, 17);
            this.FadeLightCheckBox.TabIndex = 8;
            this.FadeLightCheckBox.Text = "Fade";
            this.FadeLightCheckBox.UseVisualStyleBackColor = true;
            this.FadeLightCheckBox.CheckedChanged += new System.EventHandler(this.LightCheckBox_CheckedChanged);
            // 
            // BlinkLightCheckBox
            // 
            this.BlinkLightCheckBox.AutoSize = true;
            this.BlinkLightCheckBox.Location = new System.Drawing.Point(3, 39);
            this.BlinkLightCheckBox.Name = "BlinkLightCheckBox";
            this.BlinkLightCheckBox.Size = new System.Drawing.Size(49, 17);
            this.BlinkLightCheckBox.TabIndex = 7;
            this.BlinkLightCheckBox.Text = "Blink";
            this.BlinkLightCheckBox.UseVisualStyleBackColor = true;
            this.BlinkLightCheckBox.CheckedChanged += new System.EventHandler(this.LightCheckBox_CheckedChanged);
            // 
            // StaticLightCheckBox
            // 
            this.StaticLightCheckBox.AutoSize = true;
            this.StaticLightCheckBox.Checked = true;
            this.StaticLightCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.StaticLightCheckBox.Location = new System.Drawing.Point(3, 16);
            this.StaticLightCheckBox.Name = "StaticLightCheckBox";
            this.StaticLightCheckBox.Size = new System.Drawing.Size(53, 17);
            this.StaticLightCheckBox.TabIndex = 5;
            this.StaticLightCheckBox.Text = "Static";
            this.StaticLightCheckBox.UseVisualStyleBackColor = true;
            this.StaticLightCheckBox.CheckedChanged += new System.EventHandler(this.LightCheckBox_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Mode";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.idLabel);
            this.panel2.Location = new System.Drawing.Point(6, 19);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(194, 48);
            this.panel2.TabIndex = 4;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.panel3);
            this.groupBox1.Controls.Add(this.panel2);
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(207, 162);
            this.groupBox1.TabIndex = 23;
            this.groupBox1.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.Color2NumericUpDown);
            this.groupBox2.Controls.Add(this.Color1NumericUpDown);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.panel1);
            this.groupBox2.ForeColor = System.Drawing.Color.White;
            this.groupBox2.Location = new System.Drawing.Point(225, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(497, 162);
            this.groupBox2.TabIndex = 24;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Color";
            // 
            // Color2NumericUpDown
            // 
            this.Color2NumericUpDown.Location = new System.Drawing.Point(73, 95);
            this.Color2NumericUpDown.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.Color2NumericUpDown.Name = "Color2NumericUpDown";
            this.Color2NumericUpDown.Size = new System.Drawing.Size(120, 20);
            this.Color2NumericUpDown.TabIndex = 40;
            // 
            // Color1NumericUpDown
            // 
            this.Color1NumericUpDown.Location = new System.Drawing.Point(73, 47);
            this.Color1NumericUpDown.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.Color1NumericUpDown.Name = "Color1NumericUpDown";
            this.Color1NumericUpDown.Size = new System.Drawing.Size(120, 20);
            this.Color1NumericUpDown.TabIndex = 39;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(27, 97);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(40, 13);
            this.label4.TabIndex = 38;
            this.label4.Text = "Color 2";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(27, 50);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 13);
            this.label3.TabIndex = 36;
            this.label3.Text = "Color 1";
            // 
            // panel1
            // 
            this.panel1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel1.BackgroundImage")));
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel1.Location = new System.Drawing.Point(206, 19);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(285, 137);
            this.panel1.TabIndex = 34;
            // 
            // HelpButton
            // 
            this.HelpButton.ForeColor = System.Drawing.Color.Black;
            this.HelpButton.Location = new System.Drawing.Point(6, 69);
            this.HelpButton.Name = "HelpButton";
            this.HelpButton.Size = new System.Drawing.Size(87, 23);
            this.HelpButton.TabIndex = 41;
            this.HelpButton.Text = "?";
            this.HelpButton.UseVisualStyleBackColor = true;
            this.HelpButton.Click += new System.EventHandler(this.HelpButton_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.VirtualCamCheckBox);
            this.groupBox4.Controls.Add(this.RecCheckBox);
            this.groupBox4.Controls.Add(this.StreamCheckBox);
            this.groupBox4.ForeColor = System.Drawing.Color.White;
            this.groupBox4.Location = new System.Drawing.Point(12, 180);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(208, 162);
            this.groupBox4.TabIndex = 26;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Start/Stop";
            // 
            // VirtualCamCheckBox
            // 
            this.VirtualCamCheckBox.AutoSize = true;
            this.VirtualCamCheckBox.Location = new System.Drawing.Point(6, 102);
            this.VirtualCamCheckBox.Name = "VirtualCamCheckBox";
            this.VirtualCamCheckBox.Size = new System.Drawing.Size(79, 17);
            this.VirtualCamCheckBox.TabIndex = 2;
            this.VirtualCamCheckBox.Text = "Virtual Cam";
            this.VirtualCamCheckBox.UseVisualStyleBackColor = true;
            this.VirtualCamCheckBox.CheckedChanged += new System.EventHandler(this.ListCheckBox_CheckedChanged);
            // 
            // RecCheckBox
            // 
            this.RecCheckBox.AutoSize = true;
            this.RecCheckBox.Location = new System.Drawing.Point(7, 79);
            this.RecCheckBox.Name = "RecCheckBox";
            this.RecCheckBox.Size = new System.Drawing.Size(75, 17);
            this.RecCheckBox.TabIndex = 1;
            this.RecCheckBox.Text = "Recording";
            this.RecCheckBox.UseVisualStyleBackColor = true;
            this.RecCheckBox.CheckedChanged += new System.EventHandler(this.ListCheckBox_CheckedChanged);
            // 
            // StreamCheckBox
            // 
            this.StreamCheckBox.AutoSize = true;
            this.StreamCheckBox.Checked = true;
            this.StreamCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.StreamCheckBox.Location = new System.Drawing.Point(6, 56);
            this.StreamCheckBox.Name = "StreamCheckBox";
            this.StreamCheckBox.Size = new System.Drawing.Size(59, 17);
            this.StreamCheckBox.TabIndex = 0;
            this.StreamCheckBox.Text = "Stream";
            this.StreamCheckBox.UseVisualStyleBackColor = true;
            this.StreamCheckBox.CheckedChanged += new System.EventHandler(this.ListCheckBox_CheckedChanged);
            // 
            // SceneListBox
            // 
            this.SceneListBox.FormattingEnabled = true;
            this.SceneListBox.Location = new System.Drawing.Point(6, 43);
            this.SceneListBox.Name = "SceneListBox";
            this.SceneListBox.Size = new System.Drawing.Size(120, 108);
            this.SceneListBox.TabIndex = 27;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.SceneCheckBox);
            this.groupBox5.Controls.Add(this.SceneListBox);
            this.groupBox5.ForeColor = System.Drawing.Color.White;
            this.groupBox5.Location = new System.Drawing.Point(231, 180);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(133, 162);
            this.groupBox5.TabIndex = 28;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Scene";
            // 
            // SceneCheckBox
            // 
            this.SceneCheckBox.AutoSize = true;
            this.SceneCheckBox.Location = new System.Drawing.Point(7, 20);
            this.SceneCheckBox.Name = "SceneCheckBox";
            this.SceneCheckBox.Size = new System.Drawing.Size(57, 17);
            this.SceneCheckBox.TabIndex = 0;
            this.SceneCheckBox.Text = "Scene";
            this.SceneCheckBox.UseVisualStyleBackColor = true;
            this.SceneCheckBox.CheckedChanged += new System.EventHandler(this.ListCheckBox_CheckedChanged);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.StopMediaCheckBox);
            this.groupBox6.Controls.Add(this.RestartMediaCheckBox);
            this.groupBox6.Controls.Add(this.PlayPauseCheckBox);
            this.groupBox6.Controls.Add(this.VisibilityOFFCheckBox);
            this.groupBox6.Controls.Add(this.VisibilityONCheckBox);
            this.groupBox6.Controls.Add(this.MediaCheckBox);
            this.groupBox6.Controls.Add(this.MediaTabControl);
            this.groupBox6.ForeColor = System.Drawing.Color.White;
            this.groupBox6.Location = new System.Drawing.Point(370, 181);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(333, 162);
            this.groupBox6.TabIndex = 29;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Media";
            // 
            // StopMediaCheckBox
            // 
            this.StopMediaCheckBox.AutoSize = true;
            this.StopMediaCheckBox.Location = new System.Drawing.Point(213, 133);
            this.StopMediaCheckBox.Name = "StopMediaCheckBox";
            this.StopMediaCheckBox.Size = new System.Drawing.Size(80, 17);
            this.StopMediaCheckBox.TabIndex = 6;
            this.StopMediaCheckBox.Text = "Stop Media";
            this.StopMediaCheckBox.UseVisualStyleBackColor = true;
            this.StopMediaCheckBox.CheckedChanged += new System.EventHandler(this.MediaCheckBox_CheckedChanged);
            // 
            // RestartMediaCheckBox
            // 
            this.RestartMediaCheckBox.AutoSize = true;
            this.RestartMediaCheckBox.Location = new System.Drawing.Point(212, 110);
            this.RestartMediaCheckBox.Name = "RestartMediaCheckBox";
            this.RestartMediaCheckBox.Size = new System.Drawing.Size(92, 17);
            this.RestartMediaCheckBox.TabIndex = 5;
            this.RestartMediaCheckBox.Text = "Restart Media";
            this.RestartMediaCheckBox.UseVisualStyleBackColor = true;
            this.RestartMediaCheckBox.CheckedChanged += new System.EventHandler(this.MediaCheckBox_CheckedChanged);
            // 
            // PlayPauseCheckBox
            // 
            this.PlayPauseCheckBox.AutoSize = true;
            this.PlayPauseCheckBox.Location = new System.Drawing.Point(212, 87);
            this.PlayPauseCheckBox.Name = "PlayPauseCheckBox";
            this.PlayPauseCheckBox.Size = new System.Drawing.Size(113, 17);
            this.PlayPauseCheckBox.TabIndex = 4;
            this.PlayPauseCheckBox.Text = "Play/Pause Media";
            this.PlayPauseCheckBox.UseVisualStyleBackColor = true;
            this.PlayPauseCheckBox.CheckedChanged += new System.EventHandler(this.MediaCheckBox_CheckedChanged);
            // 
            // VisibilityOFFCheckBox
            // 
            this.VisibilityOFFCheckBox.AutoSize = true;
            this.VisibilityOFFCheckBox.Location = new System.Drawing.Point(212, 64);
            this.VisibilityOFFCheckBox.Name = "VisibilityOFFCheckBox";
            this.VisibilityOFFCheckBox.Size = new System.Drawing.Size(85, 17);
            this.VisibilityOFFCheckBox.TabIndex = 3;
            this.VisibilityOFFCheckBox.Text = "Visibility OFF";
            this.VisibilityOFFCheckBox.UseVisualStyleBackColor = true;
            this.VisibilityOFFCheckBox.CheckedChanged += new System.EventHandler(this.MediaCheckBox_CheckedChanged);
            // 
            // VisibilityONCheckBox
            // 
            this.VisibilityONCheckBox.AutoSize = true;
            this.VisibilityONCheckBox.Location = new System.Drawing.Point(212, 41);
            this.VisibilityONCheckBox.Name = "VisibilityONCheckBox";
            this.VisibilityONCheckBox.Size = new System.Drawing.Size(81, 17);
            this.VisibilityONCheckBox.TabIndex = 2;
            this.VisibilityONCheckBox.Text = "Visibility ON";
            this.VisibilityONCheckBox.UseVisualStyleBackColor = true;
            this.VisibilityONCheckBox.CheckedChanged += new System.EventHandler(this.MediaCheckBox_CheckedChanged);
            // 
            // MediaCheckBox
            // 
            this.MediaCheckBox.AutoSize = true;
            this.MediaCheckBox.Location = new System.Drawing.Point(6, 19);
            this.MediaCheckBox.Name = "MediaCheckBox";
            this.MediaCheckBox.Size = new System.Drawing.Size(55, 17);
            this.MediaCheckBox.TabIndex = 1;
            this.MediaCheckBox.Text = "Media";
            this.MediaCheckBox.UseVisualStyleBackColor = true;
            this.MediaCheckBox.CheckedChanged += new System.EventHandler(this.ListCheckBox_CheckedChanged);
            // 
            // MediaTabControl
            // 
            this.MediaTabControl.Location = new System.Drawing.Point(6, 42);
            this.MediaTabControl.Name = "MediaTabControl";
            this.MediaTabControl.SelectedIndex = 0;
            this.MediaTabControl.Size = new System.Drawing.Size(200, 110);
            this.MediaTabControl.TabIndex = 0;
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.MuteListBox);
            this.groupBox7.Controls.Add(this.MuteCheckBox);
            this.groupBox7.ForeColor = System.Drawing.Color.White;
            this.groupBox7.Location = new System.Drawing.Point(12, 349);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(134, 162);
            this.groupBox7.TabIndex = 30;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Mute";
            // 
            // MuteListBox
            // 
            this.MuteListBox.FormattingEnabled = true;
            this.MuteListBox.Location = new System.Drawing.Point(7, 43);
            this.MuteListBox.Name = "MuteListBox";
            this.MuteListBox.Size = new System.Drawing.Size(120, 95);
            this.MuteListBox.TabIndex = 8;
            // 
            // MuteCheckBox
            // 
            this.MuteCheckBox.AutoSize = true;
            this.MuteCheckBox.Location = new System.Drawing.Point(7, 20);
            this.MuteCheckBox.Name = "MuteCheckBox";
            this.MuteCheckBox.Size = new System.Drawing.Size(50, 17);
            this.MuteCheckBox.TabIndex = 7;
            this.MuteCheckBox.Text = "Mute";
            this.MuteCheckBox.UseVisualStyleBackColor = true;
            this.MuteCheckBox.CheckedChanged += new System.EventHandler(this.ListCheckBox_CheckedChanged);
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.VolumeNumericUpDown);
            this.groupBox8.Controls.Add(this.label2);
            this.groupBox8.Controls.Add(this.VolumeTrackBar);
            this.groupBox8.Controls.Add(this.TestSoundButton);
            this.groupBox8.Controls.Add(this.FileTextBox);
            this.groupBox8.Controls.Add(this.FileButton);
            this.groupBox8.Controls.Add(this.SoundCheckBox);
            this.groupBox8.ForeColor = System.Drawing.Color.White;
            this.groupBox8.Location = new System.Drawing.Point(153, 349);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(443, 162);
            this.groupBox8.TabIndex = 31;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Sound";
            // 
            // VolumeNumericUpDown
            // 
            this.VolumeNumericUpDown.Location = new System.Drawing.Point(6, 90);
            this.VolumeNumericUpDown.Name = "VolumeNumericUpDown";
            this.VolumeNumericUpDown.Size = new System.Drawing.Size(75, 20);
            this.VolumeNumericUpDown.TabIndex = 15;
            this.VolumeNumericUpDown.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.VolumeNumericUpDown.ValueChanged += new System.EventHandler(this.VolumeNumericUpDown_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 73);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "Volume";
            // 
            // VolumeTrackBar
            // 
            this.VolumeTrackBar.Location = new System.Drawing.Point(87, 73);
            this.VolumeTrackBar.Maximum = 100;
            this.VolumeTrackBar.Name = "VolumeTrackBar";
            this.VolumeTrackBar.Size = new System.Drawing.Size(350, 45);
            this.VolumeTrackBar.TabIndex = 13;
            this.VolumeTrackBar.TickFrequency = 5;
            this.VolumeTrackBar.Value = 100;
            this.VolumeTrackBar.Scroll += new System.EventHandler(this.VolumeTrackBar_Scroll);
            // 
            // TestSoundButton
            // 
            this.TestSoundButton.ForeColor = System.Drawing.Color.Black;
            this.TestSoundButton.Location = new System.Drawing.Point(362, 133);
            this.TestSoundButton.Name = "TestSoundButton";
            this.TestSoundButton.Size = new System.Drawing.Size(75, 23);
            this.TestSoundButton.TabIndex = 12;
            this.TestSoundButton.Text = "Test";
            this.TestSoundButton.UseVisualStyleBackColor = true;
            this.TestSoundButton.Click += new System.EventHandler(this.TestSoundButton_Click);
            // 
            // FileTextBox
            // 
            this.FileTextBox.Location = new System.Drawing.Point(87, 45);
            this.FileTextBox.Name = "FileTextBox";
            this.FileTextBox.Size = new System.Drawing.Size(350, 20);
            this.FileTextBox.TabIndex = 11;
            // 
            // FileButton
            // 
            this.FileButton.ForeColor = System.Drawing.Color.Black;
            this.FileButton.Location = new System.Drawing.Point(6, 43);
            this.FileButton.Name = "FileButton";
            this.FileButton.Size = new System.Drawing.Size(75, 23);
            this.FileButton.TabIndex = 10;
            this.FileButton.Text = "Search";
            this.FileButton.UseVisualStyleBackColor = true;
            this.FileButton.Click += new System.EventHandler(this.FileButton_Click);
            // 
            // SoundCheckBox
            // 
            this.SoundCheckBox.AutoSize = true;
            this.SoundCheckBox.Location = new System.Drawing.Point(6, 19);
            this.SoundCheckBox.Name = "SoundCheckBox";
            this.SoundCheckBox.Size = new System.Drawing.Size(57, 17);
            this.SoundCheckBox.TabIndex = 9;
            this.SoundCheckBox.Text = "Sound";
            this.SoundCheckBox.UseVisualStyleBackColor = true;
            this.SoundCheckBox.CheckedChanged += new System.EventHandler(this.ListCheckBox_CheckedChanged);
            // 
            // ClearButton
            // 
            this.ClearButton.ForeColor = System.Drawing.Color.Black;
            this.ClearButton.Location = new System.Drawing.Point(6, 69);
            this.ClearButton.Name = "ClearButton";
            this.ClearButton.Size = new System.Drawing.Size(89, 23);
            this.ClearButton.TabIndex = 32;
            this.ClearButton.Text = "Clear";
            this.ClearButton.UseVisualStyleBackColor = true;
            this.ClearButton.Click += new System.EventHandler(this.ClearButton_Click);
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.ClearButton);
            this.groupBox9.ForeColor = System.Drawing.Color.White;
            this.groupBox9.Location = new System.Drawing.Point(602, 349);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(101, 162);
            this.groupBox9.TabIndex = 33;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "DANGER ZONE";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.HelpButton);
            this.groupBox3.ForeColor = System.Drawing.Color.White;
            this.groupBox3.Location = new System.Drawing.Point(729, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(99, 162);
            this.groupBox3.TabIndex = 42;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Help";
            // 
            // SetForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(61)))), ((int)(((byte)(61)))), ((int)(((byte)(61)))));
            this.ClientSize = new System.Drawing.Size(840, 524);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox9);
            this.Controls.Add(this.groupBox8);
            this.Controls.Add(this.groupBox7);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.applyButton);
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SetForm";
            this.Text = "SetForm";
            this.Load += new System.EventHandler(this.SetForm_Load);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Color2NumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Color1NumericUpDown)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.VolumeNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.VolumeTrackBar)).EndInit();
            this.groupBox9.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button applyButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label idLabel;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.CheckBox StaticLightCheckBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.CheckBox FadeLightCheckBox;
        private System.Windows.Forms.CheckBox BlinkLightCheckBox;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.CheckBox VirtualCamCheckBox;
        private System.Windows.Forms.CheckBox RecCheckBox;
        private System.Windows.Forms.CheckBox StreamCheckBox;
        private System.Windows.Forms.ListBox SceneListBox;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.CheckBox SceneCheckBox;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.TabControl MediaTabControl;
        private System.Windows.Forms.CheckBox MediaCheckBox;
        private System.Windows.Forms.CheckBox StopMediaCheckBox;
        private System.Windows.Forms.CheckBox RestartMediaCheckBox;
        private System.Windows.Forms.CheckBox PlayPauseCheckBox;
        private System.Windows.Forms.CheckBox VisibilityOFFCheckBox;
        private System.Windows.Forms.CheckBox VisibilityONCheckBox;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.CheckBox MuteCheckBox;
        private System.Windows.Forms.ListBox MuteListBox;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.CheckBox SoundCheckBox;
        private System.Windows.Forms.TextBox FileTextBox;
        private System.Windows.Forms.Button FileButton;
        private System.Windows.Forms.Button TestSoundButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TrackBar VolumeTrackBar;
        private System.Windows.Forms.NumericUpDown VolumeNumericUpDown;
        private System.Windows.Forms.Button ClearButton;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown Color2NumericUpDown;
        private System.Windows.Forms.NumericUpDown Color1NumericUpDown;
        private System.Windows.Forms.Button HelpButton;
        private System.Windows.Forms.GroupBox groupBox3;
    }
}