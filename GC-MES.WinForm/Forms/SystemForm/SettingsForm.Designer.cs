using System.Windows.Forms;

namespace GC_MES.WinForm.Forms.SystemForm
{
    partial class SettingsForm
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
            pnlHeader = new Panel();
            btnClose = new Button();
            lblTitle = new Label();
            tabControl = new TabControl();
            tabPageGeneral = new TabPage();
            lblRestartHint = new Label();
            cmbTheme = new ComboBox();
            lblTheme = new Label();
            tabPageUser = new TabPage();
            chkAutoLogin = new CheckBox();
            chkRememberPassword = new CheckBox();
            lblPasswordPolicy = new Label();
            tabPageDisplay = new TabPage();
            chkShowStatusBar = new CheckBox();
            chkFullScreen = new CheckBox();
            pnlBottom = new Panel();
            btnCancel = new Button();
            btnSave = new Button();
            pnlHeader.SuspendLayout();
            tabControl.SuspendLayout();
            tabPageGeneral.SuspendLayout();
            tabPageUser.SuspendLayout();
            tabPageDisplay.SuspendLayout();
            pnlBottom.SuspendLayout();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = System.Drawing.Color.FromArgb(45, 45, 48);
            pnlHeader.Controls.Add(btnClose);
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new System.Drawing.Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new System.Drawing.Size(584, 50);
            pnlHeader.TabIndex = 0;
            // 
            // btnClose
            // 
            btnClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            btnClose.ForeColor = System.Drawing.Color.White;
            btnClose.Location = new System.Drawing.Point(537, 0);
            btnClose.Name = "btnClose";
            btnClose.Size = new System.Drawing.Size(47, 50);
            btnClose.TabIndex = 1;
            btnClose.Text = "×";
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += btnCancel_Click;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lblTitle.ForeColor = System.Drawing.Color.White;
            lblTitle.Location = new System.Drawing.Point(14, 14);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new System.Drawing.Size(74, 22);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "系统设置";
            // 
            // tabControl
            // 
            tabControl.Controls.Add(tabPageGeneral);
            tabControl.Controls.Add(tabPageUser);
            tabControl.Controls.Add(tabPageDisplay);
            tabControl.Dock = DockStyle.Fill;
            tabControl.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            tabControl.Location = new System.Drawing.Point(0, 50);
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new System.Drawing.Size(584, 371);
            tabControl.TabIndex = 1;
            // 
            // tabPageGeneral
            // 
            tabPageGeneral.Controls.Add(lblRestartHint);
            tabPageGeneral.Controls.Add(cmbTheme);
            tabPageGeneral.Controls.Add(lblTheme);
            tabPageGeneral.Location = new System.Drawing.Point(4, 26);
            tabPageGeneral.Name = "tabPageGeneral";
            tabPageGeneral.Padding = new Padding(3);
            tabPageGeneral.Size = new System.Drawing.Size(576, 341);
            tabPageGeneral.TabIndex = 0;
            tabPageGeneral.Text = "常规";
            tabPageGeneral.UseVisualStyleBackColor = true;
            // 
            // lblRestartHint
            // 
            lblRestartHint.AutoSize = true;
            lblRestartHint.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point);
            lblRestartHint.ForeColor = System.Drawing.Color.FromArgb(159, 68, 74);
            lblRestartHint.Location = new System.Drawing.Point(304, 30);
            lblRestartHint.Name = "lblRestartHint";
            lblRestartHint.Size = new System.Drawing.Size(224, 17);
            lblRestartHint.TabIndex = 2;
            lblRestartHint.Text = "主题变更将在应用程序重启后完全生效";
            lblRestartHint.Visible = false;
            // 
            // cmbTheme
            // 
            cmbTheme.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbTheme.FormattingEnabled = true;
            cmbTheme.Location = new System.Drawing.Point(95, 28);
            cmbTheme.Name = "cmbTheme";
            cmbTheme.Size = new System.Drawing.Size(185, 25);
            cmbTheme.TabIndex = 1;
            cmbTheme.SelectedIndexChanged += cmbTheme_SelectedIndexChanged;
            // 
            // lblTheme
            // 
            lblTheme.AutoSize = true;
            lblTheme.Location = new System.Drawing.Point(20, 31);
            lblTheme.Name = "lblTheme";
            lblTheme.Size = new System.Drawing.Size(68, 17);
            lblTheme.TabIndex = 0;
            lblTheme.Text = "系统主题：";
            // 
            // tabPageUser
            // 
            tabPageUser.Controls.Add(chkAutoLogin);
            tabPageUser.Controls.Add(chkRememberPassword);
            tabPageUser.Controls.Add(lblPasswordPolicy);
            tabPageUser.Location = new System.Drawing.Point(4, 26);
            tabPageUser.Name = "tabPageUser";
            tabPageUser.Padding = new Padding(3);
            tabPageUser.Size = new System.Drawing.Size(576, 341);
            tabPageUser.TabIndex = 1;
            tabPageUser.Text = "用户设置";
            tabPageUser.UseVisualStyleBackColor = true;
            // 
            // chkAutoLogin
            // 
            chkAutoLogin.AutoSize = true;
            chkAutoLogin.Location = new System.Drawing.Point(20, 65);
            chkAutoLogin.Name = "chkAutoLogin";
            chkAutoLogin.Size = new System.Drawing.Size(123, 21);
            chkAutoLogin.TabIndex = 2;
            chkAutoLogin.Text = "启动时自动登录";
            chkAutoLogin.UseVisualStyleBackColor = true;
            // 
            // chkRememberPassword
            // 
            chkRememberPassword.AutoSize = true;
            chkRememberPassword.Location = new System.Drawing.Point(20, 28);
            chkRememberPassword.Name = "chkRememberPassword";
            chkRememberPassword.Size = new System.Drawing.Size(75, 21);
            chkRememberPassword.TabIndex = 1;
            chkRememberPassword.Text = "记住密码";
            chkRememberPassword.UseVisualStyleBackColor = true;
            // 
            // lblPasswordPolicy
            // 
            lblPasswordPolicy.AutoSize = true;
            lblPasswordPolicy.Location = new System.Drawing.Point(20, 108);
            lblPasswordPolicy.Name = "lblPasswordPolicy";
            lblPasswordPolicy.Size = new System.Drawing.Size(224, 17);
            lblPasswordPolicy.TabIndex = 0;
            lblPasswordPolicy.Text = "密码策略: 密码长度不少于8位，包含数字";
            // 
            // tabPageDisplay
            // 
            tabPageDisplay.Controls.Add(chkShowStatusBar);
            tabPageDisplay.Controls.Add(chkFullScreen);
            tabPageDisplay.Location = new System.Drawing.Point(4, 26);
            tabPageDisplay.Name = "tabPageDisplay";
            tabPageDisplay.Size = new System.Drawing.Size(576, 341);
            tabPageDisplay.TabIndex = 2;
            tabPageDisplay.Text = "显示";
            tabPageDisplay.UseVisualStyleBackColor = true;
            // 
            // chkShowStatusBar
            // 
            chkShowStatusBar.AutoSize = true;
            chkShowStatusBar.Checked = true;
            chkShowStatusBar.CheckState = CheckState.Checked;
            chkShowStatusBar.Location = new System.Drawing.Point(20, 65);
            chkShowStatusBar.Name = "chkShowStatusBar";
            chkShowStatusBar.Size = new System.Drawing.Size(99, 21);
            chkShowStatusBar.TabIndex = 4;
            chkShowStatusBar.Text = "显示状态栏";
            chkShowStatusBar.UseVisualStyleBackColor = true;
            // 
            // chkFullScreen
            // 
            chkFullScreen.AutoSize = true;
            chkFullScreen.Location = new System.Drawing.Point(20, 28);
            chkFullScreen.Name = "chkFullScreen";
            chkFullScreen.Size = new System.Drawing.Size(171, 21);
            chkFullScreen.TabIndex = 3;
            chkFullScreen.Text = "启动时以全屏模式显示";
            chkFullScreen.UseVisualStyleBackColor = true;
            // 
            // pnlBottom
            // 
            pnlBottom.BackColor = System.Drawing.Color.FromArgb(240, 240, 240);
            pnlBottom.Controls.Add(btnCancel);
            pnlBottom.Controls.Add(btnSave);
            pnlBottom.Dock = DockStyle.Bottom;
            pnlBottom.Location = new System.Drawing.Point(0, 371);
            pnlBottom.Name = "pnlBottom";
            pnlBottom.Size = new System.Drawing.Size(584, 60);
            pnlBottom.TabIndex = 2;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCancel.BackColor = System.Drawing.Color.FromArgb(180, 180, 180);
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            btnCancel.ForeColor = System.Drawing.Color.White;
            btnCancel.Location = new System.Drawing.Point(484, 15);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(88, 33);
            btnCancel.TabIndex = 1;
            btnCancel.Text = "取消";
            btnCancel.UseVisualStyleBackColor = false;
            btnCancel.Click += btnCancel_Click;
            // 
            // btnSave
            // 
            btnSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnSave.BackColor = System.Drawing.Color.FromArgb(45, 45, 48);
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            btnSave.ForeColor = System.Drawing.Color.White;
            btnSave.Location = new System.Drawing.Point(376, 15);
            btnSave.Name = "btnSave";
            btnSave.Size = new System.Drawing.Size(88, 33);
            btnSave.TabIndex = 0;
            btnSave.Text = "保存";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;
            // 
            // SettingsForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(584, 431);
            Controls.Add(tabControl);
            Controls.Add(pnlBottom);
            Controls.Add(pnlHeader);
            FormBorderStyle = FormBorderStyle.None;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "SettingsForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "系统设置";
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            tabControl.ResumeLayout(false);
            tabPageGeneral.ResumeLayout(false);
            tabPageGeneral.PerformLayout();
            tabPageUser.ResumeLayout(false);
            tabPageUser.PerformLayout();
            tabPageDisplay.ResumeLayout(false);
            tabPageDisplay.PerformLayout();
            pnlBottom.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlHeader;
        private Label lblTitle;
        private Button btnClose;
        private TabControl tabControl;
        private TabPage tabPageGeneral;
        private TabPage tabPageUser;
        private Panel pnlBottom;
        private Button btnCancel;
        private Button btnSave;
        private Label lblTheme;
        private ComboBox cmbTheme;
        private Label lblPasswordPolicy;
        private CheckBox chkRememberPassword;
        private CheckBox chkAutoLogin;
        private TabPage tabPageDisplay;
        private CheckBox chkShowStatusBar;
        private CheckBox chkFullScreen;
        private Label lblRestartHint;
    }
} 