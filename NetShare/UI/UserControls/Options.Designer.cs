
namespace NetShare.UI.UserControls
{
    partial class Options
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.BtnSave = new System.Windows.Forms.Button();
            this.BtnClose = new System.Windows.Forms.Button();
            this.AutoMountOnStartUp = new System.Windows.Forms.CheckBox();
            this.OnStartupMinimizeToTray = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // BtnSave
            // 
            this.BtnSave.Location = new System.Drawing.Point(459, 98);
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.Size = new System.Drawing.Size(75, 23);
            this.BtnSave.TabIndex = 0;
            this.BtnSave.Text = "Save";
            this.BtnSave.UseVisualStyleBackColor = true;
            this.BtnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // BtnClose
            // 
            this.BtnClose.Location = new System.Drawing.Point(378, 98);
            this.BtnClose.Name = "BtnClose";
            this.BtnClose.Size = new System.Drawing.Size(75, 23);
            this.BtnClose.TabIndex = 1;
            this.BtnClose.Text = "Cancel";
            this.BtnClose.UseVisualStyleBackColor = true;
            this.BtnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // AutoMountOnStartUp
            // 
            this.AutoMountOnStartUp.AutoSize = true;
            this.AutoMountOnStartUp.Location = new System.Drawing.Point(16, 15);
            this.AutoMountOnStartUp.Name = "AutoMountOnStartUp";
            this.AutoMountOnStartUp.Size = new System.Drawing.Size(203, 17);
            this.AutoMountOnStartUp.TabIndex = 2;
            this.AutoMountOnStartUp.Text = "automatically mount shares on startup";
            this.AutoMountOnStartUp.UseVisualStyleBackColor = true;
            this.AutoMountOnStartUp.Visible = false;
            // 
            // OnStartupMinimizeToTray
            // 
            this.OnStartupMinimizeToTray.AutoSize = true;
            this.OnStartupMinimizeToTray.Location = new System.Drawing.Point(16, 38);
            this.OnStartupMinimizeToTray.Name = "OnStartupMinimizeToTray";
            this.OnStartupMinimizeToTray.Size = new System.Drawing.Size(242, 17);
            this.OnStartupMinimizeToTray.TabIndex = 3;
            this.OnStartupMinimizeToTray.Text = "On startup minimize to tray icon (needs restart)";
            this.OnStartupMinimizeToTray.UseVisualStyleBackColor = true;
            // 
            // Options
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.OnStartupMinimizeToTray);
            this.Controls.Add(this.AutoMountOnStartUp);
            this.Controls.Add(this.BtnClose);
            this.Controls.Add(this.BtnSave);
            this.Name = "Options";
            this.Size = new System.Drawing.Size(537, 127);
            this.Load += new System.EventHandler(this.Options_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BtnSave;
        private System.Windows.Forms.Button BtnClose;
        private System.Windows.Forms.CheckBox AutoMountOnStartUp;
        private System.Windows.Forms.CheckBox OnStartupMinimizeToTray;
    }
}
