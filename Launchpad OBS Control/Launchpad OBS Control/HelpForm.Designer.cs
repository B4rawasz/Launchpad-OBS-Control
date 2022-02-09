namespace Launchpad_OBS_Control
{
    partial class HelpForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HelpForm));
            this.panel1 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.StartStopCheckBox = new System.Windows.Forms.CheckBox();
            this.SceneCheckBox = new System.Windows.Forms.CheckBox();
            this.MediaCheckBox = new System.Windows.Forms.CheckBox();
            this.MuteCheckBox = new System.Windows.Forms.CheckBox();
            this.SoundCheckBox = new System.Windows.Forms.CheckBox();
            this.ClearCheckBox = new System.Windows.Forms.CheckBox();
            this.HelpTextBox = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel1.BackgroundImage")));
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel1.Location = new System.Drawing.Point(13, 229);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(405, 209);
            this.panel1.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.ForeColor = System.Drawing.Color.Black;
            this.button1.Location = new System.Drawing.Point(424, 415);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Ok";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 24);
            this.label1.TabIndex = 2;
            this.label1.Text = "Help";
            // 
            // StartStopCheckBox
            // 
            this.StartStopCheckBox.AutoSize = true;
            this.StartStopCheckBox.Checked = true;
            this.StartStopCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.StartStopCheckBox.Location = new System.Drawing.Point(13, 75);
            this.StartStopCheckBox.Name = "StartStopCheckBox";
            this.StartStopCheckBox.Size = new System.Drawing.Size(120, 17);
            this.StartStopCheckBox.TabIndex = 3;
            this.StartStopCheckBox.Text = "Stream/Rec/V Cam";
            this.StartStopCheckBox.UseVisualStyleBackColor = true;
            this.StartStopCheckBox.CheckedChanged += new System.EventHandler(this.CheckBox_CheckedChanged);
            // 
            // SceneCheckBox
            // 
            this.SceneCheckBox.AutoSize = true;
            this.SceneCheckBox.Location = new System.Drawing.Point(13, 98);
            this.SceneCheckBox.Name = "SceneCheckBox";
            this.SceneCheckBox.Size = new System.Drawing.Size(57, 17);
            this.SceneCheckBox.TabIndex = 4;
            this.SceneCheckBox.Text = "Scene";
            this.SceneCheckBox.UseVisualStyleBackColor = true;
            this.SceneCheckBox.CheckedChanged += new System.EventHandler(this.CheckBox_CheckedChanged);
            // 
            // MediaCheckBox
            // 
            this.MediaCheckBox.AutoSize = true;
            this.MediaCheckBox.Location = new System.Drawing.Point(13, 121);
            this.MediaCheckBox.Name = "MediaCheckBox";
            this.MediaCheckBox.Size = new System.Drawing.Size(55, 17);
            this.MediaCheckBox.TabIndex = 5;
            this.MediaCheckBox.Text = "Media";
            this.MediaCheckBox.UseVisualStyleBackColor = true;
            this.MediaCheckBox.CheckedChanged += new System.EventHandler(this.CheckBox_CheckedChanged);
            // 
            // MuteCheckBox
            // 
            this.MuteCheckBox.AutoSize = true;
            this.MuteCheckBox.Location = new System.Drawing.Point(13, 144);
            this.MuteCheckBox.Name = "MuteCheckBox";
            this.MuteCheckBox.Size = new System.Drawing.Size(50, 17);
            this.MuteCheckBox.TabIndex = 6;
            this.MuteCheckBox.Text = "Mute";
            this.MuteCheckBox.UseVisualStyleBackColor = true;
            this.MuteCheckBox.CheckedChanged += new System.EventHandler(this.CheckBox_CheckedChanged);
            // 
            // SoundCheckBox
            // 
            this.SoundCheckBox.AutoSize = true;
            this.SoundCheckBox.Location = new System.Drawing.Point(13, 167);
            this.SoundCheckBox.Name = "SoundCheckBox";
            this.SoundCheckBox.Size = new System.Drawing.Size(57, 17);
            this.SoundCheckBox.TabIndex = 7;
            this.SoundCheckBox.Text = "Sound";
            this.SoundCheckBox.UseVisualStyleBackColor = true;
            this.SoundCheckBox.CheckedChanged += new System.EventHandler(this.CheckBox_CheckedChanged);
            // 
            // ClearCheckBox
            // 
            this.ClearCheckBox.AutoSize = true;
            this.ClearCheckBox.Location = new System.Drawing.Point(13, 190);
            this.ClearCheckBox.Name = "ClearCheckBox";
            this.ClearCheckBox.Size = new System.Drawing.Size(50, 17);
            this.ClearCheckBox.TabIndex = 8;
            this.ClearCheckBox.Text = "Clear";
            this.ClearCheckBox.UseVisualStyleBackColor = true;
            this.ClearCheckBox.CheckedChanged += new System.EventHandler(this.CheckBox_CheckedChanged);
            // 
            // HelpTextBox
            // 
            this.HelpTextBox.Location = new System.Drawing.Point(174, 75);
            this.HelpTextBox.Name = "HelpTextBox";
            this.HelpTextBox.ReadOnly = true;
            this.HelpTextBox.Size = new System.Drawing.Size(320, 132);
            this.HelpTextBox.TabIndex = 9;
            this.HelpTextBox.Text = "";
            // 
            // HelpForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(61)))), ((int)(((byte)(61)))), ((int)(((byte)(61)))));
            this.ClientSize = new System.Drawing.Size(506, 450);
            this.Controls.Add(this.HelpTextBox);
            this.Controls.Add(this.ClearCheckBox);
            this.Controls.Add(this.SoundCheckBox);
            this.Controls.Add(this.MuteCheckBox);
            this.Controls.Add(this.MediaCheckBox);
            this.Controls.Add(this.SceneCheckBox);
            this.Controls.Add(this.StartStopCheckBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.panel1);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "HelpForm";
            this.Text = "HelpForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox StartStopCheckBox;
        private System.Windows.Forms.CheckBox SceneCheckBox;
        private System.Windows.Forms.CheckBox MediaCheckBox;
        private System.Windows.Forms.CheckBox MuteCheckBox;
        private System.Windows.Forms.CheckBox SoundCheckBox;
        private System.Windows.Forms.CheckBox ClearCheckBox;
        private System.Windows.Forms.RichTextBox HelpTextBox;
    }
}