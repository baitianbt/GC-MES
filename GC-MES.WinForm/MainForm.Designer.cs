using System.Windows.Forms;
using GC_MES.WinForm.Controls;

namespace GC_MES.WinForm.Forms
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
       
        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pnlHeader = new Panel();
            labChildrenFormName = new Label();
            lblUserName = new Label();
            btnSetting = new Button();
            btnMinimize = new Button();
            btnMaximize = new Button();
            btnClose = new Button();
            lblTitle = new Label();
            pnlStatus = new Panel();
            lblStatus = new Label();
            menu = new DMenu();
            pnlContent = new Panel();
            pnlHeader.SuspendLayout();
            pnlStatus.SuspendLayout();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.FromArgb(45, 45, 48);
            pnlHeader.Controls.Add(labChildrenFormName);
            pnlHeader.Controls.Add(lblUserName);
            pnlHeader.Controls.Add(btnSetting);
            pnlHeader.Controls.Add(btnMinimize);
            pnlHeader.Controls.Add(btnMaximize);
            pnlHeader.Controls.Add(btnClose);
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Margin = new Padding(4);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(1556, 65);
            pnlHeader.TabIndex = 0;
            // 
            // labChildrenFormName
            // 
            labChildrenFormName.AutoSize = true;
            labChildrenFormName.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 134);
            labChildrenFormName.ForeColor = Color.White;
            labChildrenFormName.Location = new Point(177, 20);
            labChildrenFormName.Margin = new Padding(4, 0, 4, 0);
            labChildrenFormName.Name = "labChildrenFormName";
            labChildrenFormName.Size = new Size(0, 22);
            labChildrenFormName.TabIndex = 5;
            // 
            // lblUserName
            // 
            lblUserName.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblUserName.AutoSize = true;
            lblUserName.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            lblUserName.ForeColor = Color.White;
            lblUserName.Location = new Point(1299, 24);
            lblUserName.Margin = new Padding(4, 0, 4, 0);
            lblUserName.Name = "lblUserName";
            lblUserName.Size = new Size(44, 17);
            lblUserName.TabIndex = 4;
            lblUserName.Text = "用户名";
            // 
            // btnSetting
            // 
            btnSetting.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSetting.FlatAppearance.BorderSize = 0;
            btnSetting.FlatStyle = FlatStyle.Flat;
            btnSetting.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 134);
            btnSetting.ForeColor = Color.White;
            btnSetting.Location = new Point(1369, 0);
            btnSetting.Margin = new Padding(4);
            btnSetting.Name = "btnSetting";
            btnSetting.Size = new Size(47, 52);
            btnSetting.TabIndex = 3;
            btnSetting.Text = "⚙︎";
            btnSetting.UseVisualStyleBackColor = false;
            btnSetting.Click += btnSetting_Click;
            // 
            // btnMinimize
            // 
            btnMinimize.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnMinimize.FlatAppearance.BorderSize = 0;
            btnMinimize.FlatStyle = FlatStyle.Flat;
            btnMinimize.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 134);
            btnMinimize.ForeColor = Color.White;
            btnMinimize.Location = new Point(1416, 0);
            btnMinimize.Margin = new Padding(4);
            btnMinimize.Name = "btnMinimize";
            btnMinimize.Size = new Size(47, 52);
            btnMinimize.TabIndex = 3;
            btnMinimize.Text = "_";
            btnMinimize.UseVisualStyleBackColor = false;
            btnMinimize.Click += btnMinimize_Click;
            // 
            // btnMaximize
            // 
            btnMaximize.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnMaximize.FlatAppearance.BorderSize = 0;
            btnMaximize.FlatStyle = FlatStyle.Flat;
            btnMaximize.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 134);
            btnMaximize.ForeColor = Color.White;
            btnMaximize.Location = new Point(1463, 0);
            btnMaximize.Margin = new Padding(4);
            btnMaximize.Name = "btnMaximize";
            btnMaximize.Size = new Size(47, 52);
            btnMaximize.TabIndex = 2;
            btnMaximize.Text = "□";
            btnMaximize.UseVisualStyleBackColor = false;
            btnMaximize.Click += btnMaximize_Click;
            // 
            // btnClose
            // 
            btnClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 134);
            btnClose.ForeColor = Color.White;
            btnClose.Location = new Point(1510, 0);
            btnClose.Margin = new Padding(4);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(47, 52);
            btnClose.TabIndex = 1;
            btnClose.Text = "×";
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += btnClose_Click;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 134);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(14, 18);
            lblTitle.Margin = new Padding(4, 0, 4, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(112, 22);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "GC-MES 系统";
            // 
            // pnlStatus
            // 
            pnlStatus.BackColor = Color.FromArgb(45, 45, 48);
            pnlStatus.Controls.Add(lblStatus);
            pnlStatus.Dock = DockStyle.Bottom;
            pnlStatus.Location = new Point(0, 969);
            pnlStatus.Margin = new Padding(4);
            pnlStatus.Name = "pnlStatus";
            pnlStatus.Size = new Size(1556, 26);
            pnlStatus.TabIndex = 3;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Font = new Font("Microsoft YaHei UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 134);
            lblStatus.ForeColor = Color.White;
            lblStatus.Location = new Point(14, 4);
            lblStatus.Margin = new Padding(4, 0, 4, 0);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(106, 16);
            lblStatus.TabIndex = 0;
            lblStatus.Text = "GC-MES 系统 | 就绪";
            // 
            // menu
            // 
            menu.BackColor = Color.FromArgb(45, 45, 48);
            menu.Dock = DockStyle.Left;
            menu.Font = new Font("Microsoft YaHei UI", 9F);
            menu.ForeColor = Color.White;
            menu.IconSize = new Size(24, 24);
            menu.Location = new Point(0, 65);
            menu.MenuBackColor = Color.FromArgb(45, 45, 48);
            menu.MenuItemHoverColor = Color.FromArgb(55, 55, 58);
            menu.MenuItemSelectedColor = Color.FromArgb(62, 62, 64);
            menu.MenuTextColor = Color.White;
            menu.Name = "menu";
            menu.Size = new Size(177, 904);
            menu.SubMenuIndicatorColor = Color.LightGray;
            menu.TabIndex = 5;
            menu.Text = "dMenu1";
            // 
            // pnlContent
            // 
            pnlContent.BackColor = Color.White;
            pnlContent.Dock = DockStyle.Fill;
            pnlContent.Location = new Point(177, 65);
            pnlContent.Margin = new Padding(4);
            pnlContent.Name = "pnlContent";
            pnlContent.Size = new Size(1379, 904);
            pnlContent.TabIndex = 6;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1556, 995);
            Controls.Add(pnlContent);
            Controls.Add(menu);
            Controls.Add(pnlStatus);
            Controls.Add(pnlHeader);
            FormBorderStyle = FormBorderStyle.None;
            IsMdiContainer = true;
            Margin = new Padding(4);
            MinimumSize = new Size(931, 643);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "GC-MES 系统";
            Load += MainForm_Load;
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            pnlStatus.ResumeLayout(false);
            pnlStatus.PerformLayout();
            ResumeLayout(false);
        }



        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Button btnSetting;
        private System.Windows.Forms.Button btnMinimize;
        private System.Windows.Forms.Button btnMaximize;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel pnlStatus;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblUserName;
        private DMenu menu;
        private Panel pnlContent;
        private Label labChildrenFormName;
    }
}