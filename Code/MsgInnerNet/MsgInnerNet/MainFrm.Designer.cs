namespace MsgInnerNet
{
    partial class MainFrm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.msglogBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.serveripTxt = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.clearbtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // msglogBox
            // 
            this.msglogBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.msglogBox.Location = new System.Drawing.Point(9, 92);
            this.msglogBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.msglogBox.Multiline = true;
            this.msglogBox.Name = "msglogBox";
            this.msglogBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.msglogBox.Size = new System.Drawing.Size(710, 276);
            this.msglogBox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 54);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "消息记录：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 16);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "服务器地址：";
            // 
            // serveripTxt
            // 
            this.serveripTxt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.serveripTxt.Enabled = false;
            this.serveripTxt.Location = new System.Drawing.Point(88, 10);
            this.serveripTxt.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.serveripTxt.Name = "serveripTxt";
            this.serveripTxt.Size = new System.Drawing.Size(317, 21);
            this.serveripTxt.TabIndex = 4;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button1.Location = new System.Drawing.Point(430, 6);
            this.button1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(107, 25);
            this.button1.TabIndex = 5;
            this.button1.Text = "测试连通";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // clearbtn
            // 
            this.clearbtn.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.clearbtn.Location = new System.Drawing.Point(88, 46);
            this.clearbtn.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.clearbtn.Name = "clearbtn";
            this.clearbtn.Size = new System.Drawing.Size(94, 28);
            this.clearbtn.TabIndex = 6;
            this.clearbtn.Text = "清 理";
            this.clearbtn.UseVisualStyleBackColor = true;
            this.clearbtn.Click += new System.EventHandler(this.clearbtn_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(728, 376);
            this.Controls.Add(this.clearbtn);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.serveripTxt);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.msglogBox);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "控制命令内外网转发";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox msglogBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox serveripTxt;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button clearbtn;
    }
}

