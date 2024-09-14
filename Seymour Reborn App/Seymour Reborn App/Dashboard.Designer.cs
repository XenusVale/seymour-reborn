namespace Seymour_Reborn_App
{
    partial class Dashboard
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Dashboard));
            this.enabledLabel = new System.Windows.Forms.Label();
            this.sinRebornCheckBox = new System.Windows.Forms.CheckBox();
            this.runMods = new System.Windows.Forms.Timer(this.components);
            this.toolTips = new System.Windows.Forms.ToolTip(this.components);
            this.trayNotifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.SuspendLayout();
            // 
            // enabledLabel
            // 
            this.enabledLabel.AutoSize = true;
            this.enabledLabel.BackColor = System.Drawing.Color.Transparent;
            this.enabledLabel.Font = new System.Drawing.Font("Constantia", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.enabledLabel.ForeColor = System.Drawing.Color.White;
            this.enabledLabel.Location = new System.Drawing.Point(20, 9);
            this.enabledLabel.Name = "enabledLabel";
            this.enabledLabel.Size = new System.Drawing.Size(292, 26);
            this.enabledLabel.TabIndex = 0;
            this.enabledLabel.Text = "Seymour Reborn is Enabled.";
            // 
            // sinRebornCheckBox
            // 
            this.sinRebornCheckBox.AutoSize = true;
            this.sinRebornCheckBox.BackColor = System.Drawing.Color.Transparent;
            this.sinRebornCheckBox.Font = new System.Drawing.Font("Constantia", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sinRebornCheckBox.ForeColor = System.Drawing.Color.White;
            this.sinRebornCheckBox.Location = new System.Drawing.Point(39, 38);
            this.sinRebornCheckBox.Name = "sinRebornCheckBox";
            this.sinRebornCheckBox.Size = new System.Drawing.Size(273, 17);
            this.sinRebornCheckBox.TabIndex = 1;
            this.sinRebornCheckBox.Text = "Automatically open Sin Reborn Difficulty Mod";
            this.sinRebornCheckBox.UseVisualStyleBackColor = false;
            this.sinRebornCheckBox.CheckedChanged += new System.EventHandler(this.SinRebornCheckBox_CheckedChanged);
            // 
            // runMods
            // 
            this.runMods.Enabled = true;
            this.runMods.Interval = 10;
            this.runMods.Tick += new System.EventHandler(this.RunMods_Tick);
            // 
            // toolTips
            // 
            this.toolTips.AutomaticDelay = 50;
            this.toolTips.AutoPopDelay = 10000;
            this.toolTips.InitialDelay = 50;
            this.toolTips.ReshowDelay = 10;
            // 
            // trayNotifyIcon
            // 
            this.trayNotifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("trayNotifyIcon.Icon")));
            this.trayNotifyIcon.Text = "Seymour Reborn Mod";
            this.trayNotifyIcon.Visible = true;
            this.trayNotifyIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(this.TrayNotifyIcon_MouseClick);
            // 
            // Dashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(330, 60);
            this.Controls.Add(this.sinRebornCheckBox);
            this.Controls.Add(this.enabledLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Location = new System.Drawing.Point(1107, 815);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(346, 99);
            this.MinimumSize = new System.Drawing.Size(346, 99);
            this.Name = "Dashboard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "FFX: Seymour Reborn Mod";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Dashboard_FormClosing);
            this.Load += new System.EventHandler(this.Dashboard_Load);
            this.Resize += new System.EventHandler(this.Dashboard_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label enabledLabel;
        private System.Windows.Forms.CheckBox sinRebornCheckBox;
        private System.Windows.Forms.Timer runMods;
        private System.Windows.Forms.ToolTip toolTips;
        private System.Windows.Forms.NotifyIcon trayNotifyIcon;
    }
}

