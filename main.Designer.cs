﻿namespace iplant_BarCodePrint
{
    partial class mainUI
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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(mainUI));
            this.textBox_adder = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_port = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.log = new System.Windows.Forms.TextBox();
            this.bt_connectServer = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.lab_FileName = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lab_UnFinishLabCnt = new System.Windows.Forms.Label();
            this.lab_FinishLabCnt = new System.Windows.Forms.Label();
            this.btnPrintTest = new System.Windows.Forms.Button();
            this.btnClearLog = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox_adder
            // 
            this.textBox_adder.Enabled = false;
            this.textBox_adder.Location = new System.Drawing.Point(110, 13);
            this.textBox_adder.Name = "textBox_adder";
            this.textBox_adder.ReadOnly = true;
            this.textBox_adder.Size = new System.Drawing.Size(157, 21);
            this.textBox_adder.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "服务器地址：";
            // 
            // textBox_port
            // 
            this.textBox_port.Enabled = false;
            this.textBox_port.Location = new System.Drawing.Point(346, 13);
            this.textBox_port.Name = "textBox_port";
            this.textBox_port.ReadOnly = true;
            this.textBox_port.Size = new System.Drawing.Size(63, 21);
            this.textBox_port.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(276, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "服务器端口：";
            // 
            // log
            // 
            this.log.CausesValidation = false;
            this.log.Location = new System.Drawing.Point(7, 105);
            this.log.Multiline = true;
            this.log.Name = "log";
            this.log.ReadOnly = true;
            this.log.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.log.Size = new System.Drawing.Size(555, 294);
            this.log.TabIndex = 10;
            // 
            // bt_connectServer
            // 
            this.bt_connectServer.Location = new System.Drawing.Point(415, 12);
            this.bt_connectServer.Name = "bt_connectServer";
            this.bt_connectServer.Size = new System.Drawing.Size(143, 23);
            this.bt_connectServer.TabIndex = 11;
            this.bt_connectServer.Text = "连接";
            this.bt_connectServer.UseVisualStyleBackColor = true;
            this.bt_connectServer.Click += new System.EventHandler(this.bt_connectServer_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 42);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 12);
            this.label3.TabIndex = 12;
            this.label3.Text = "当前打印文件：";
            // 
            // lab_FileName
            // 
            this.lab_FileName.AutoSize = true;
            this.lab_FileName.Location = new System.Drawing.Point(123, 42);
            this.lab_FileName.Name = "lab_FileName";
            this.lab_FileName.Size = new System.Drawing.Size(53, 12);
            this.lab_FileName.TabIndex = 13;
            this.lab_FileName.Text = "filename";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(16, 64);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(101, 12);
            this.label5.TabIndex = 14;
            this.label5.Text = "待打印标签个数：";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(238, 64);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(101, 12);
            this.label6.TabIndex = 15;
            this.label6.Text = "已打印标签个数：";
            // 
            // lab_UnFinishLabCnt
            // 
            this.lab_UnFinishLabCnt.AutoSize = true;
            this.lab_UnFinishLabCnt.Location = new System.Drawing.Point(124, 64);
            this.lab_UnFinishLabCnt.Name = "lab_UnFinishLabCnt";
            this.lab_UnFinishLabCnt.Size = new System.Drawing.Size(89, 12);
            this.lab_UnFinishLabCnt.TabIndex = 16;
            this.lab_UnFinishLabCnt.Text = "labUnFinishCnt";
            // 
            // lab_FinishLabCnt
            // 
            this.lab_FinishLabCnt.AutoSize = true;
            this.lab_FinishLabCnt.Location = new System.Drawing.Point(345, 64);
            this.lab_FinishLabCnt.Name = "lab_FinishLabCnt";
            this.lab_FinishLabCnt.Size = new System.Drawing.Size(77, 12);
            this.lab_FinishLabCnt.TabIndex = 17;
            this.lab_FinishLabCnt.Text = "labFinishCnt";
            // 
            // btnPrintTest
            // 
            this.btnPrintTest.Location = new System.Drawing.Point(507, 62);
            this.btnPrintTest.Name = "btnPrintTest";
            this.btnPrintTest.Size = new System.Drawing.Size(51, 23);
            this.btnPrintTest.TabIndex = 20;
            this.btnPrintTest.Text = "测试";
            this.btnPrintTest.UseVisualStyleBackColor = true;
            this.btnPrintTest.Click += new System.EventHandler(this.btnPrintTest_Click);
            // 
            // btnClearLog
            // 
            this.btnClearLog.Location = new System.Drawing.Point(506, 37);
            this.btnClearLog.Name = "btnClearLog";
            this.btnClearLog.Size = new System.Drawing.Size(52, 23);
            this.btnClearLog.TabIndex = 21;
            this.btnClearLog.Text = "清日志";
            this.btnClearLog.UseVisualStyleBackColor = true;
            this.btnClearLog.Click += new System.EventHandler(this.btnClearLog_Click);
            // 
            // mainUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(570, 425);
            this.Controls.Add(this.btnClearLog);
            this.Controls.Add(this.btnPrintTest);
            this.Controls.Add(this.lab_FinishLabCnt);
            this.Controls.Add(this.lab_UnFinishLabCnt);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lab_FileName);
            this.Controls.Add(this.textBox_adder);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.bt_connectServer);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.log);
            this.Controls.Add(this.textBox_port);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "mainUI";
            this.Text = "标签打印 V2.0.20181228";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.closeingWindow);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox_adder;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_port;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox log;
        private System.Windows.Forms.Button bt_connectServer;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lab_FileName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lab_UnFinishLabCnt;
        private System.Windows.Forms.Label lab_FinishLabCnt;
        private System.Windows.Forms.Button btnPrintTest;
        private System.Windows.Forms.Button btnClearLog;
    }
}

