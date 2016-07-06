namespace DoorLockDemo
{
    partial class Form1
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
            this.loadingPictBx = new System.Windows.Forms.PictureBox();
            this.qrPictBx = new System.Windows.Forms.PictureBox();
            this.padlockPictBx = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.getQrBtn = new System.Windows.Forms.Button();
            this.cancelBtn = new System.Windows.Forms.Button();
            this.btnPnl = new System.Windows.Forms.Panel();
            this.resetBtn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.loadingPictBx)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.qrPictBx)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.padlockPictBx)).BeginInit();
            this.btnPnl.SuspendLayout();
            this.SuspendLayout();
            // 
            // loadingPictBx
            // 
            this.loadingPictBx.BackColor = System.Drawing.Color.Transparent;
            this.loadingPictBx.Image = global::DoorLockDemo.Properties.Resources.loadingRoll;
            this.loadingPictBx.Location = new System.Drawing.Point(19, 49);
            this.loadingPictBx.Name = "loadingPictBx";
            this.loadingPictBx.Size = new System.Drawing.Size(443, 175);
            this.loadingPictBx.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.loadingPictBx.TabIndex = 2;
            this.loadingPictBx.TabStop = false;
            this.loadingPictBx.Visible = false;
            // 
            // qrPictBx
            // 
            this.qrPictBx.Dock = System.Windows.Forms.DockStyle.Fill;
            this.qrPictBx.Location = new System.Drawing.Point(0, 0);
            this.qrPictBx.Name = "qrPictBx";
            this.qrPictBx.Size = new System.Drawing.Size(443, 139);
            this.qrPictBx.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.qrPictBx.TabIndex = 1;
            this.qrPictBx.TabStop = false;
            // 
            // padlockPictBx
            // 
            this.padlockPictBx.Image = global::DoorLockDemo.Properties.Resources.locked_padlock;
            this.padlockPictBx.Location = new System.Drawing.Point(19, 49);
            this.padlockPictBx.Name = "padlockPictBx";
            this.padlockPictBx.Size = new System.Drawing.Size(443, 175);
            this.padlockPictBx.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.padlockPictBx.TabIndex = 0;
            this.padlockPictBx.TabStop = false;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(19, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(443, 37);
            this.label1.TabIndex = 3;
            this.label1.Text = "Door 34";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // getQrBtn
            // 
            this.getQrBtn.Location = new System.Drawing.Point(183, 60);
            this.getQrBtn.Name = "getQrBtn";
            this.getQrBtn.Size = new System.Drawing.Size(75, 23);
            this.getQrBtn.TabIndex = 4;
            this.getQrBtn.Text = "Get QR";
            this.getQrBtn.UseVisualStyleBackColor = true;
            this.getQrBtn.Click += new System.EventHandler(this.getQrBtn_Click);
            // 
            // cancelBtn
            // 
            this.cancelBtn.Location = new System.Drawing.Point(183, 60);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(75, 23);
            this.cancelBtn.TabIndex = 5;
            this.cancelBtn.Text = "Cancel";
            this.cancelBtn.UseVisualStyleBackColor = true;
            this.cancelBtn.Visible = false;
            this.cancelBtn.Click += new System.EventHandler(this.cancelBtn_Click);
            // 
            // btnPnl
            // 
            this.btnPnl.Controls.Add(this.resetBtn);
            this.btnPnl.Controls.Add(this.cancelBtn);
            this.btnPnl.Controls.Add(this.getQrBtn);
            this.btnPnl.Controls.Add(this.qrPictBx);
            this.btnPnl.Location = new System.Drawing.Point(19, 230);
            this.btnPnl.Name = "btnPnl";
            this.btnPnl.Size = new System.Drawing.Size(443, 139);
            this.btnPnl.TabIndex = 6;
            // 
            // resetBtn
            // 
            this.resetBtn.Location = new System.Drawing.Point(183, 60);
            this.resetBtn.Name = "resetBtn";
            this.resetBtn.Size = new System.Drawing.Size(75, 23);
            this.resetBtn.TabIndex = 6;
            this.resetBtn.Text = "Reset";
            this.resetBtn.UseVisualStyleBackColor = true;
            this.resetBtn.Visible = false;
            this.resetBtn.Click += new System.EventHandler(this.resetBtn_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(474, 381);
            this.Controls.Add(this.btnPnl);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.loadingPictBx);
            this.Controls.Add(this.padlockPictBx);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.loadingPictBx)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.qrPictBx)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.padlockPictBx)).EndInit();
            this.btnPnl.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox padlockPictBx;
        private System.Windows.Forms.PictureBox qrPictBx;
        private System.Windows.Forms.PictureBox loadingPictBx;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button getQrBtn;
        private System.Windows.Forms.Button cancelBtn;
        private System.Windows.Forms.Panel btnPnl;
        private System.Windows.Forms.Button resetBtn;
    }
}

