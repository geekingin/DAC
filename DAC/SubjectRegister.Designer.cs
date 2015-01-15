namespace DAC
{
    partial class SubjectRegister
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SubjectRegister));
            this.btn_enter = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.pwd = new System.Windows.Forms.TextBox();
            this.btn_exit = new System.Windows.Forms.Button();
            this.user = new System.Windows.Forms.TextBox();
            this.rpwd = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pwdWarning = new System.Windows.Forms.Label();
            this.userWaring = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btn_enter
            // 
            this.btn_enter.BackColor = System.Drawing.Color.Lime;
            this.btn_enter.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btn_enter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_enter.Font = new System.Drawing.Font("SimSun", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_enter.Location = new System.Drawing.Point(251, 27);
            this.btn_enter.Name = "btn_enter";
            this.btn_enter.Size = new System.Drawing.Size(87, 28);
            this.btn_enter.TabIndex = 4;
            this.btn_enter.TabStop = false;
            this.btn_enter.Text = "注册(&Y)";
            this.btn_enter.UseVisualStyleBackColor = false;
            this.btn_enter.Click += new System.EventHandler(this.enter_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft YaHei", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(33, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 20);
            this.label2.TabIndex = 5;
            this.label2.Text = "账号";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft YaHei", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(33, 69);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 20);
            this.label3.TabIndex = 6;
            this.label3.Text = "密码";
            // 
            // pwd
            // 
            this.pwd.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.pwd.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pwd.Font = new System.Drawing.Font("SimSun", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.pwd.ForeColor = System.Drawing.SystemColors.WindowText;
            this.pwd.Location = new System.Drawing.Point(73, 69);
            this.pwd.Multiline = true;
            this.pwd.Name = "pwd";
            this.pwd.PasswordChar = '*';
            this.pwd.Size = new System.Drawing.Size(166, 22);
            this.pwd.TabIndex = 2;
            this.pwd.TextChanged += new System.EventHandler(this.pwd_TextChanged);
            // 
            // btn_exit
            // 
            this.btn_exit.BackColor = System.Drawing.Color.Red;
            this.btn_exit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_exit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_exit.Font = new System.Drawing.Font("SimSun", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_exit.Location = new System.Drawing.Point(251, 105);
            this.btn_exit.Name = "btn_exit";
            this.btn_exit.Size = new System.Drawing.Size(87, 28);
            this.btn_exit.TabIndex = 5;
            this.btn_exit.TabStop = false;
            this.btn_exit.Text = "退 出 (&E)";
            this.btn_exit.UseVisualStyleBackColor = false;
            this.btn_exit.Click += new System.EventHandler(this.exit_Click);
            // 
            // user
            // 
            this.user.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.user.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.user.Font = new System.Drawing.Font("SimSun", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.user.Location = new System.Drawing.Point(73, 27);
            this.user.Multiline = true;
            this.user.Name = "user";
            this.user.Size = new System.Drawing.Size(166, 22);
            this.user.TabIndex = 1;
            this.user.TextChanged += new System.EventHandler(this.user_TextChanged);
            // 
            // rpwd
            // 
            this.rpwd.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.rpwd.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rpwd.Font = new System.Drawing.Font("SimSun", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rpwd.Location = new System.Drawing.Point(73, 110);
            this.rpwd.Multiline = true;
            this.rpwd.Name = "rpwd";
            this.rpwd.PasswordChar = '*';
            this.rpwd.Size = new System.Drawing.Size(166, 22);
            this.rpwd.TabIndex = 3;
            this.rpwd.TextChanged += new System.EventHandler(this.rpwd_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft YaHei", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(5, 112);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 20);
            this.label1.TabIndex = 9;
            this.label1.Text = "确认密码";
            // 
            // pwdWarning
            // 
            this.pwdWarning.AutoSize = true;
            this.pwdWarning.Font = new System.Drawing.Font("Microsoft YaHei", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.pwdWarning.ForeColor = System.Drawing.Color.Red;
            this.pwdWarning.Location = new System.Drawing.Point(97, 135);
            this.pwdWarning.Name = "pwdWarning";
            this.pwdWarning.Size = new System.Drawing.Size(107, 20);
            this.pwdWarning.TabIndex = 10;
            this.pwdWarning.Text = "两次密码不一致";
            this.pwdWarning.Visible = false;
            // 
            // userWaring
            // 
            this.userWaring.AutoSize = true;
            this.userWaring.BackColor = System.Drawing.Color.Transparent;
            this.userWaring.Font = new System.Drawing.Font("Microsoft YaHei", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.userWaring.ForeColor = System.Drawing.Color.Red;
            this.userWaring.Location = new System.Drawing.Point(70, 49);
            this.userWaring.Name = "userWaring";
            this.userWaring.Size = new System.Drawing.Size(122, 20);
            this.userWaring.TabIndex = 11;
            // 
            // SubjectRegister
            // 
            this.AcceptButton = this.btn_enter;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.CancelButton = this.btn_exit;
            this.ClientSize = new System.Drawing.Size(359, 164);
            this.Controls.Add(this.userWaring);
            this.Controls.Add(this.pwdWarning);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.rpwd);
            this.Controls.Add(this.user);
            this.Controls.Add(this.pwd);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btn_exit);
            this.Controls.Add(this.btn_enter);
            this.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SubjectRegister";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "注册账号";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.subjectRegister_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_enter;
        //private System.Windows.Forms.Button btn_12enter;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox pwd;
        private System.Windows.Forms.Button btn_exit;
        private System.Windows.Forms.TextBox user;
        private System.Windows.Forms.TextBox rpwd;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label pwdWarning;
        private System.Windows.Forms.Label userWaring;
    }
}