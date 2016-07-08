namespace DoorLockDemo
{
    partial class aboutForm
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
            this.aboutLnkLbl = new System.Windows.Forms.LinkLabel();
            this.aboutPnl = new System.Windows.Forms.Panel();
            this.aboutPnl.SuspendLayout();
            this.SuspendLayout();
            // 
            // aboutLnkLbl
            // 
            this.aboutLnkLbl.AutoSize = true;
            this.aboutLnkLbl.Location = new System.Drawing.Point(10, 10);
            this.aboutLnkLbl.Margin = new System.Windows.Forms.Padding(10);
            this.aboutLnkLbl.Name = "aboutLnkLbl";
            this.aboutLnkLbl.Size = new System.Drawing.Size(285, 13);
            this.aboutLnkLbl.TabIndex = 0;
            this.aboutLnkLbl.TabStop = true;
            this.aboutLnkLbl.Text = "Icons by Daniel Bruce and Freepik. Licensed by CC 3.0 BY";
            // 
            // aboutPnl
            // 
            this.aboutPnl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.aboutPnl.Controls.Add(this.aboutLnkLbl);
            this.aboutPnl.Location = new System.Drawing.Point(12, 12);
            this.aboutPnl.Name = "aboutPnl";
            this.aboutPnl.Size = new System.Drawing.Size(331, 42);
            this.aboutPnl.TabIndex = 1;
            // 
            // aboutForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(355, 66);
            this.Controls.Add(this.aboutPnl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "aboutForm";
            this.ShowInTaskbar = false;
            this.Text = "About";
            this.Load += new System.EventHandler(this.aboutForm_Load);
            this.aboutPnl.ResumeLayout(false);
            this.aboutPnl.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.LinkLabel aboutLnkLbl;
        private System.Windows.Forms.Panel aboutPnl;
    }
}