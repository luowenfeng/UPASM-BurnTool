namespace BurnTool
{
    partial class FlashSaveDialog
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.beginAddrTB = new System.Windows.Forms.TextBox();
            this.rdSizeTB = new System.Windows.Forms.TextBox();
            this.okBtn = new System.Windows.Forms.Button();
            this.cancelBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(148, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "起始地址(4字节对齐)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(28, 78);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(148, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "读取大小(4字节对齐)";
            // 
            // beginAddrTB
            // 
            this.beginAddrTB.Location = new System.Drawing.Point(182, 36);
            this.beginAddrTB.Name = "beginAddrTB";
            this.beginAddrTB.Size = new System.Drawing.Size(175, 27);
            this.beginAddrTB.TabIndex = 2;
            this.beginAddrTB.Text = "0";
            // 
            // rdSizeTB
            // 
            this.rdSizeTB.Location = new System.Drawing.Point(182, 75);
            this.rdSizeTB.Name = "rdSizeTB";
            this.rdSizeTB.Size = new System.Drawing.Size(175, 27);
            this.rdSizeTB.TabIndex = 3;
            this.rdSizeTB.Text = "65536";
            // 
            // okBtn
            // 
            this.okBtn.Location = new System.Drawing.Point(202, 130);
            this.okBtn.Name = "okBtn";
            this.okBtn.Size = new System.Drawing.Size(94, 29);
            this.okBtn.TabIndex = 4;
            this.okBtn.Text = "开始读取";
            this.okBtn.UseVisualStyleBackColor = true;
            this.okBtn.Click += new System.EventHandler(this.okBtn_Click);
            // 
            // cancelBtn
            // 
            this.cancelBtn.Location = new System.Drawing.Point(102, 130);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(94, 29);
            this.cancelBtn.TabIndex = 5;
            this.cancelBtn.Text = "取消";
            this.cancelBtn.UseVisualStyleBackColor = true;
            this.cancelBtn.Click += new System.EventHandler(this.cancelBtn_Click);
            // 
            // FlashSaveDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(408, 184);
            this.Controls.Add(this.cancelBtn);
            this.Controls.Add(this.okBtn);
            this.Controls.Add(this.rdSizeTB);
            this.Controls.Add(this.beginAddrTB);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "FlashSaveDialog";
            this.Text = "flash读取配置";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox beginAddrTB;
        private System.Windows.Forms.TextBox rdSizeTB;
        private System.Windows.Forms.Button okBtn;
        private System.Windows.Forms.Button cancelBtn;
    }
}