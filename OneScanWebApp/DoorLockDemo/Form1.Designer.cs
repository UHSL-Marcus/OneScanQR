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
            this.qrPctrBx = new System.Windows.Forms.PictureBox();
            this.padlockPctrBx = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.getQrBtn = new System.Windows.Forms.Button();
            this.cancelBtn = new System.Windows.Forms.Button();
            this.btnPnl = new System.Windows.Forms.Panel();
            this.loadingRadioPctrBx = new System.Windows.Forms.PictureBox();
            this.resetBtn = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.qrPctrBx)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.padlockPctrBx)).BeginInit();
            this.btnPnl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.loadingRadioPctrBx)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // qrPctrBx
            // 
            this.qrPctrBx.BackColor = System.Drawing.Color.Transparent;
            this.qrPctrBx.Location = new System.Drawing.Point(15, 12);
            this.qrPctrBx.Name = "qrPctrBx";
            this.qrPctrBx.Size = new System.Drawing.Size(93, 66);
            this.qrPctrBx.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.qrPctrBx.TabIndex = 1;
            this.qrPctrBx.TabStop = false;
            // 
            // padlockPctrBx
            // 
            this.padlockPctrBx.Dock = System.Windows.Forms.DockStyle.Fill;
            this.padlockPctrBx.Image = global::DoorLockDemo.Properties.Resources.locked_padlock;
            this.padlockPctrBx.Location = new System.Drawing.Point(3, 40);
            this.padlockPctrBx.Name = "padlockPctrBx";
            this.padlockPctrBx.Size = new System.Drawing.Size(367, 220);
            this.padlockPctrBx.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.padlockPctrBx.TabIndex = 0;
            this.padlockPctrBx.TabStop = false;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(367, 37);
            this.label1.TabIndex = 3;
            this.label1.Text = "Door 34";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // getQrBtn
            // 
            this.getQrBtn.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.getQrBtn.Location = new System.Drawing.Point(139, 66);
            this.getQrBtn.Name = "getQrBtn";
            this.getQrBtn.Size = new System.Drawing.Size(73, 21);
            this.getQrBtn.TabIndex = 4;
            this.getQrBtn.Text = "Get QR";
            this.getQrBtn.UseVisualStyleBackColor = true;
            this.getQrBtn.Click += new System.EventHandler(this.getQrBtn_Click);
            // 
            // cancelBtn
            // 
            this.cancelBtn.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cancelBtn.Location = new System.Drawing.Point(139, 12);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(73, 21);
            this.cancelBtn.TabIndex = 5;
            this.cancelBtn.Text = "Cancel";
            this.cancelBtn.UseVisualStyleBackColor = true;
            this.cancelBtn.Visible = false;
            this.cancelBtn.Click += new System.EventHandler(this.cancelBtn_Click);
            // 
            // btnPnl
            // 
            this.btnPnl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPnl.Controls.Add(this.resetBtn);
            this.btnPnl.Controls.Add(this.qrPctrBx);
            this.btnPnl.Controls.Add(this.cancelBtn);
            this.btnPnl.Controls.Add(this.loadingRadioPctrBx);
            this.btnPnl.Controls.Add(this.getQrBtn);
            this.btnPnl.Location = new System.Drawing.Point(3, 266);
            this.btnPnl.Name = "btnPnl";
            this.btnPnl.Size = new System.Drawing.Size(367, 108);
            this.btnPnl.TabIndex = 6;
            // 
            // loadingRadioPctrBx
            // 
            this.loadingRadioPctrBx.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.loadingRadioPctrBx.Image = global::DoorLockDemo.Properties.Resources.loadingRadio;
            this.loadingRadioPctrBx.Location = new System.Drawing.Point(236, 24);
            this.loadingRadioPctrBx.Name = "loadingRadioPctrBx";
            this.loadingRadioPctrBx.Size = new System.Drawing.Size(73, 36);
            this.loadingRadioPctrBx.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.loadingRadioPctrBx.TabIndex = 7;
            this.loadingRadioPctrBx.TabStop = false;
            // 
            // resetBtn
            // 
            this.resetBtn.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.resetBtn.Location = new System.Drawing.Point(139, 39);
            this.resetBtn.Name = "resetBtn";
            this.resetBtn.Size = new System.Drawing.Size(73, 21);
            this.resetBtn.TabIndex = 6;
            this.resetBtn.Text = "Reset";
            this.resetBtn.UseVisualStyleBackColor = true;
            this.resetBtn.Visible = false;
            this.resetBtn.Click += new System.EventHandler(this.resetBtn_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.padlockPctrBx, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnPnl, 0, 2);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 12);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(373, 377);
            this.tableLayoutPanel1.TabIndex = 8;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(397, 401);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.qrPctrBx)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.padlockPctrBx)).EndInit();
            this.btnPnl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.loadingRadioPctrBx)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox padlockPctrBx;
        private System.Windows.Forms.PictureBox qrPctrBx;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button getQrBtn;
        private System.Windows.Forms.Button cancelBtn;
        private System.Windows.Forms.Panel btnPnl;
        private System.Windows.Forms.Button resetBtn;
        private System.Windows.Forms.PictureBox loadingRadioPctrBx;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}

