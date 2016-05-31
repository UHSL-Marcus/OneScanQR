namespace SignalRClient_test
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
            this.SendBtn = new System.Windows.Forms.Button();
            this.sendTxtbx = new System.Windows.Forms.TextBox();
            this.recieveTxtBx = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // SendBtn
            // 
            this.SendBtn.Location = new System.Drawing.Point(211, 196);
            this.SendBtn.Name = "SendBtn";
            this.SendBtn.Size = new System.Drawing.Size(75, 23);
            this.SendBtn.TabIndex = 0;
            this.SendBtn.Text = "Send";
            this.SendBtn.UseVisualStyleBackColor = true;
            this.SendBtn.Click += new System.EventHandler(this.SendBtn_Click);
            // 
            // sendTxtbx
            // 
            this.sendTxtbx.Location = new System.Drawing.Point(12, 198);
            this.sendTxtbx.Name = "sendTxtbx";
            this.sendTxtbx.Size = new System.Drawing.Size(193, 20);
            this.sendTxtbx.TabIndex = 1;
            // 
            // recieveTxtBx
            // 
            this.recieveTxtBx.Location = new System.Drawing.Point(12, 12);
            this.recieveTxtBx.Multiline = true;
            this.recieveTxtBx.Name = "recieveTxtBx";
            this.recieveTxtBx.Size = new System.Drawing.Size(274, 178);
            this.recieveTxtBx.TabIndex = 2;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(298, 230);
            this.Controls.Add(this.recieveTxtBx);
            this.Controls.Add(this.sendTxtbx);
            this.Controls.Add(this.SendBtn);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button SendBtn;
        private System.Windows.Forms.TextBox sendTxtbx;
        private System.Windows.Forms.TextBox recieveTxtBx;
    }
}

