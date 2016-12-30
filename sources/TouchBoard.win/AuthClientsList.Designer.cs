namespace TouchBoardWinServer
{
    partial class AuthClientsList
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
            this.components = new System.ComponentModel.Container();
            this.ClientsListBox = new System.Windows.Forms.ListBox();
            this.authClientBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.RemoveSelBtn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.authClientBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // ClientsListBox
            // 
            this.ClientsListBox.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.authClientBindingSource, "IPaddress", true));
            this.ClientsListBox.DataSource = this.authClientBindingSource;
            this.ClientsListBox.DisplayMember = "IPaddress";
            this.ClientsListBox.FormattingEnabled = true;
            this.ClientsListBox.Location = new System.Drawing.Point(12, 12);
            this.ClientsListBox.Name = "ClientsListBox";
            this.ClientsListBox.Size = new System.Drawing.Size(322, 121);
            this.ClientsListBox.TabIndex = 0;
            this.ClientsListBox.ValueMember = "HostName";
            // 
            // authClientBindingSource
            // 
            this.authClientBindingSource.DataSource = typeof(TouchBoardServerComms.AuthClient);
            // 
            // RemoveSelBtn
            // 
            this.RemoveSelBtn.Location = new System.Drawing.Point(223, 139);
            this.RemoveSelBtn.Name = "RemoveSelBtn";
            this.RemoveSelBtn.Size = new System.Drawing.Size(111, 23);
            this.RemoveSelBtn.TabIndex = 1;
            this.RemoveSelBtn.Text = "Remove Selected";
            this.RemoveSelBtn.UseVisualStyleBackColor = true;
            this.RemoveSelBtn.Click += new System.EventHandler(this.RemoveSelBtn_Click);
            // 
            // AuthClientsList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(346, 168);
            this.Controls.Add(this.RemoveSelBtn);
            this.Controls.Add(this.ClientsListBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AuthClientsList";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Authorized Clients";
            this.Load += new System.EventHandler(this.AuthClientsList_Load);
            ((System.ComponentModel.ISupportInitialize)(this.authClientBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox ClientsListBox;
        private System.Windows.Forms.Button RemoveSelBtn;
        private System.Windows.Forms.BindingSource authClientBindingSource;
    }
}