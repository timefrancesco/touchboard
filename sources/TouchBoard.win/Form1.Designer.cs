namespace TouchBoardWinServer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.DebugBtn1 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.InterceptorHelpLabel = new System.Windows.Forms.Label();
            this.InterceptorDriverChkBox = new System.Windows.Forms.CheckBox();
            this.ViewAuthClientsBtn = new System.Windows.Forms.Button();
            this.statusLbl = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this._numClientsConn = new System.Windows.Forms.ToolStripStatusLabel();
            this.groupBox1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // DebugBtn1
            // 
            this.DebugBtn1.Location = new System.Drawing.Point(6, 29);
            this.DebugBtn1.Name = "DebugBtn1";
            this.DebugBtn1.Size = new System.Drawing.Size(120, 54);
            this.DebugBtn1.TabIndex = 0;
            this.DebugBtn1.Text = "Enable Discovery";
            this.DebugBtn1.UseVisualStyleBackColor = true;
            this.DebugBtn1.Click += new System.EventHandler(this.DebugBtn1_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.InterceptorHelpLabel);
            this.groupBox1.Controls.Add(this.InterceptorDriverChkBox);
            this.groupBox1.Controls.Add(this.ViewAuthClientsBtn);
            this.groupBox1.Controls.Add(this.statusLbl);
            this.groupBox1.Controls.Add(this.DebugBtn1);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(260, 139);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "TouchBoard";
            // 
            // InterceptorHelpLabel
            // 
            this.InterceptorHelpLabel.AutoSize = true;
            this.InterceptorHelpLabel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.InterceptorHelpLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InterceptorHelpLabel.ForeColor = System.Drawing.SystemColors.Highlight;
            this.InterceptorHelpLabel.Location = new System.Drawing.Point(167, 93);
            this.InterceptorHelpLabel.Name = "InterceptorHelpLabel";
            this.InterceptorHelpLabel.Size = new System.Drawing.Size(52, 13);
            this.InterceptorHelpLabel.TabIndex = 9;
            this.InterceptorHelpLabel.Text = "More Info";
            this.InterceptorHelpLabel.Click += new System.EventHandler(this.InterceptorHelpLabel_Click);
            // 
            // InterceptorDriverChkBox
            // 
            this.InterceptorDriverChkBox.AutoSize = true;
            this.InterceptorDriverChkBox.Location = new System.Drawing.Point(33, 92);
            this.InterceptorDriverChkBox.Name = "InterceptorDriverChkBox";
            this.InterceptorDriverChkBox.Size = new System.Drawing.Size(128, 17);
            this.InterceptorDriverChkBox.TabIndex = 7;
            this.InterceptorDriverChkBox.Text = "Use Interceptor driver";
            this.InterceptorDriverChkBox.UseVisualStyleBackColor = true;
            this.InterceptorDriverChkBox.CheckedChanged += new System.EventHandler(this.InterceptorDriverChkBox_CheckedChanged);
            // 
            // ViewAuthClientsBtn
            // 
            this.ViewAuthClientsBtn.Location = new System.Drawing.Point(132, 29);
            this.ViewAuthClientsBtn.Name = "ViewAuthClientsBtn";
            this.ViewAuthClientsBtn.Size = new System.Drawing.Size(122, 54);
            this.ViewAuthClientsBtn.TabIndex = 6;
            this.ViewAuthClientsBtn.Text = "Authorized Clients";
            this.ViewAuthClientsBtn.UseVisualStyleBackColor = true;
            this.ViewAuthClientsBtn.Click += new System.EventHandler(this.ViewAuthClientsBtn_Click);
            // 
            // statusLbl
            // 
            this.statusLbl.AutoSize = true;
            this.statusLbl.Location = new System.Drawing.Point(49, 123);
            this.statusLbl.Name = "statusLbl";
            this.statusLbl.Size = new System.Drawing.Size(84, 13);
            this.statusLbl.TabIndex = 5;
            this.statusLbl.Text = "Discovery Mode";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 123);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Status:";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this._numClientsConn});
            this.statusStrip1.Location = new System.Drawing.Point(0, 154);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(286, 22);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(107, 17);
            this.toolStripStatusLabel1.Text = "Clients Connected:";
            // 
            // _numClientsConn
            // 
            this._numClientsConn.Name = "_numClientsConn";
            this._numClientsConn.Size = new System.Drawing.Size(73, 17);
            this._numClientsConn.Text = "_numClients";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(286, 176);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "TouchBoard";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button DebugBtn1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label statusLbl;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel _numClientsConn;
        private System.Windows.Forms.Button ViewAuthClientsBtn;
        private System.Windows.Forms.Label InterceptorHelpLabel;
        private System.Windows.Forms.CheckBox InterceptorDriverChkBox;
    }
}

