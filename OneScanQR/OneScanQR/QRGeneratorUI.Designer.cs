namespace OneScanQR
{
    partial class QRGeneratorUI
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
            this.LoginActionCmbx = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.QRBox = new System.Windows.Forms.PictureBox();
            this.QRGenBtn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.QRBox)).BeginInit();
            this.SuspendLayout();
            // 
            // LoginActionCmbx
            // 
            this.LoginActionCmbx.FormattingEnabled = true;
            this.LoginActionCmbx.Location = new System.Drawing.Point(83, 12);
            this.LoginActionCmbx.Name = "LoginActionCmbx";
            this.LoginActionCmbx.Size = new System.Drawing.Size(121, 21);
            this.LoginActionCmbx.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Login Action";
            // 
            // QRBox
            // 
            this.QRBox.Location = new System.Drawing.Point(12, 78);
            this.QRBox.Name = "QRBox";
            this.QRBox.Size = new System.Drawing.Size(500, 303);
            this.QRBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.QRBox.TabIndex = 2;
            this.QRBox.TabStop = false;
            // 
            // QRGenBtn
            // 
            this.QRGenBtn.Location = new System.Drawing.Point(83, 49);
            this.QRGenBtn.Name = "QRGenBtn";
            this.QRGenBtn.Size = new System.Drawing.Size(75, 23);
            this.QRGenBtn.TabIndex = 3;
            this.QRGenBtn.Text = "Get QR";
            this.QRGenBtn.UseVisualStyleBackColor = true;
            this.QRGenBtn.Click += new System.EventHandler(this.QRGenBtn_Click);
            // 
            // QRGeneratorUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(524, 393);
            this.Controls.Add(this.QRGenBtn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.LoginActionCmbx);
            this.Controls.Add(this.QRBox);
            this.Name = "QRGeneratorUI";
            this.Text = "OneScan QR";
            ((System.ComponentModel.ISupportInitialize)(this.QRBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox LoginActionCmbx;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox QRBox;
        private System.Windows.Forms.Button QRGenBtn;
    }
}

