
namespace BurnTool
{
    partial class BurnToolForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BurnToolForm));
            this.textFile = new System.Windows.Forms.TextBox();
            this.btnBurn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.productCombo = new System.Windows.Forms.ComboBox();
            this.btnView = new System.Windows.Forms.Button();
            this.textLog = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.btnRead = new System.Windows.Forms.Button();
            this.createBtn = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.clearBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textFile
            // 
            this.textFile.Location = new System.Drawing.Point(88, 44);
            this.textFile.Name = "textFile";
            this.textFile.ReadOnly = true;
            this.textFile.Size = new System.Drawing.Size(474, 27);
            this.textFile.TabIndex = 0;
            // 
            // btnBurn
            // 
            this.btnBurn.Enabled = false;
            this.btnBurn.Location = new System.Drawing.Point(256, 77);
            this.btnBurn.Name = "btnBurn";
            this.btnBurn.Size = new System.Drawing.Size(94, 29);
            this.btnBurn.TabIndex = 1;
            this.btnBurn.Text = "烧录";
            this.btnBurn.UseVisualStyleBackColor = true;
            this.btnBurn.Click += new System.EventHandler(this.btnBurn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "分区文件";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "产品型号";
            // 
            // productCombo
            // 
            this.productCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.productCombo.FormattingEnabled = true;
            this.productCombo.Location = new System.Drawing.Point(88, 10);
            this.productCombo.Name = "productCombo";
            this.productCombo.Size = new System.Drawing.Size(151, 28);
            this.productCombo.TabIndex = 4;
            // 
            // btnView
            // 
            this.btnView.Location = new System.Drawing.Point(568, 44);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(94, 29);
            this.btnView.TabIndex = 5;
            this.btnView.Text = "浏览";
            this.btnView.UseVisualStyleBackColor = true;
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // textLog
            // 
            this.textLog.Location = new System.Drawing.Point(13, 123);
            this.textLog.Multiline = true;
            this.textLog.Name = "textLog";
            this.textLog.ReadOnly = true;
            this.textLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textLog.Size = new System.Drawing.Size(649, 539);
            this.textLog.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 100);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 20);
            this.label3.TabIndex = 7;
            this.label3.Text = "烧录日志";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(13, 668);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(649, 29);
            this.progressBar1.TabIndex = 8;
            // 
            // btnRead
            // 
            this.btnRead.Location = new System.Drawing.Point(356, 77);
            this.btnRead.Name = "btnRead";
            this.btnRead.Size = new System.Drawing.Size(94, 29);
            this.btnRead.TabIndex = 9;
            this.btnRead.Text = "读取";
            this.btnRead.UseVisualStyleBackColor = true;
            this.btnRead.Click += new System.EventHandler(this.btnRead_Click);
            // 
            // createBtn
            // 
            this.createBtn.Location = new System.Drawing.Point(156, 77);
            this.createBtn.Name = "createBtn";
            this.createBtn.Size = new System.Drawing.Size(94, 29);
            this.createBtn.TabIndex = 10;
            this.createBtn.Text = "生成";
            this.createBtn.UseVisualStyleBackColor = true;
            this.createBtn.Click += new System.EventHandler(this.createBtn_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(258, 13);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(217, 20);
            this.label4.TabIndex = 11;
            this.label4.Text = "版本：V1.0.0_20230711_Beta";
            // 
            // clearBtn
            // 
            this.clearBtn.Location = new System.Drawing.Point(456, 77);
            this.clearBtn.Name = "clearBtn";
            this.clearBtn.Size = new System.Drawing.Size(94, 29);
            this.clearBtn.TabIndex = 12;
            this.clearBtn.Text = "清除日志";
            this.clearBtn.UseVisualStyleBackColor = true;
            this.clearBtn.Click += new System.EventHandler(this.clearBtn_Click);
            // 
            // BurnToolForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(670, 709);
            this.Controls.Add(this.clearBtn);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.createBtn);
            this.Controls.Add(this.btnRead);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textLog);
            this.Controls.Add(this.btnView);
            this.Controls.Add(this.productCombo);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnBurn);
            this.Controls.Add(this.textFile);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(688, 756);
            this.MinimumSize = new System.Drawing.Size(688, 756);
            this.Name = "BurnToolForm";
            this.Text = "优象烧录工具";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textFile;
        private System.Windows.Forms.Button btnBurn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox productCombo;
        private System.Windows.Forms.Button btnView;
        private System.Windows.Forms.TextBox textLog;
        private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ProgressBar progressBar1;
		private System.Windows.Forms.Button btnRead;
        private System.Windows.Forms.Button createBtn;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button clearBtn;
    }
}

