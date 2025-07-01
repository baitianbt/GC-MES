using static System.Net.Mime.MediaTypeNames;

namespace GC_MES.WinForm.Forms.SystemForm.SubForm
{

    partial class MenuEditForm
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
            lblTitle = new Label();
            pnlContent = new Panel();
            tabControl1 = new TabControl();
            tabPageBasic = new TabPage();
            cmbMenuType = new ComboBox();
            label9 = new Label();
            chkEnable = new CheckBox();
            nudOrderNo = new NumericUpDown();
            label8 = new Label();
            txtDescription = new TextBox();
            label7 = new Label();
            txtIcon = new TextBox();
            label6 = new Label();
            txtAuth = new TextBox();
            label5 = new Label();
            txtUrl = new TextBox();
            label4 = new Label();
            txtTableName = new TextBox();
            label3 = new Label();
            cmbParent = new ComboBox();
            label2 = new Label();
            txtMenuName = new TextBox();
            label1 = new Label();
            pnlButtons = new Panel();
            btnCancel = new Button();
            btnSave = new Button();
            pnlContent.SuspendLayout();
            tabControl1.SuspendLayout();
            tabPageBasic.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudOrderNo).BeginInit();
            pnlButtons.SuspendLayout();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.BackColor = Color.FromArgb(45, 45, 48);
            lblTitle.Dock = DockStyle.Top;
            lblTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 134);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(0, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(684, 50);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "编辑菜单";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pnlContent
            // 
            pnlContent.Controls.Add(tabControl1);
            pnlContent.Dock = DockStyle.Fill;
            pnlContent.Location = new Point(0, 50);
            pnlContent.Name = "pnlContent";
            pnlContent.Padding = new Padding(20);
            pnlContent.Size = new Size(684, 451);
            pnlContent.TabIndex = 1;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPageBasic);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(20, 20);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(644, 411);
            tabControl1.TabIndex = 0;
            // 
            // tabPageBasic
            // 
            tabPageBasic.BackColor = Color.FromArgb(250, 250, 250);
            tabPageBasic.Controls.Add(cmbMenuType);
            tabPageBasic.Controls.Add(label9);
            tabPageBasic.Controls.Add(chkEnable);
            tabPageBasic.Controls.Add(nudOrderNo);
            tabPageBasic.Controls.Add(label8);
            tabPageBasic.Controls.Add(txtDescription);
            tabPageBasic.Controls.Add(label7);
            tabPageBasic.Controls.Add(txtIcon);
            tabPageBasic.Controls.Add(label6);
            tabPageBasic.Controls.Add(txtAuth);
            tabPageBasic.Controls.Add(label5);
            tabPageBasic.Controls.Add(txtUrl);
            tabPageBasic.Controls.Add(label4);
            tabPageBasic.Controls.Add(txtTableName);
            tabPageBasic.Controls.Add(label3);
            tabPageBasic.Controls.Add(cmbParent);
            tabPageBasic.Controls.Add(label2);
            tabPageBasic.Controls.Add(txtMenuName);
            tabPageBasic.Controls.Add(label1);
            tabPageBasic.Location = new Point(4, 26);
            tabPageBasic.Name = "tabPageBasic";
            tabPageBasic.Padding = new Padding(3);
            tabPageBasic.Size = new Size(636, 381);
            tabPageBasic.TabIndex = 0;
            tabPageBasic.Text = "基本信息";
            // 
            // cmbMenuType
            // 
            cmbMenuType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbMenuType.FormattingEnabled = true;
            cmbMenuType.Location = new Point(120, 310);
            cmbMenuType.Name = "cmbMenuType";
            cmbMenuType.Size = new Size(150, 25);
            cmbMenuType.TabIndex = 18;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(20, 313);
            label9.Name = "label9";
            label9.Size = new Size(68, 17);
            label9.TabIndex = 17;
            label9.Text = "菜单类型：";
            // 
            // chkEnable
            // 
            chkEnable.AutoSize = true;
            chkEnable.Location = new Point(120, 280);
            chkEnable.Name = "chkEnable";
            chkEnable.Size = new Size(75, 21);
            chkEnable.TabIndex = 16;
            chkEnable.Text = "是否启用";
            chkEnable.UseVisualStyleBackColor = true;
            // 
            // nudOrderNo
            // 
            nudOrderNo.Location = new Point(120, 240);
            nudOrderNo.Maximum = new decimal(new int[] { 9999, 0, 0, 0 });
            nudOrderNo.Name = "nudOrderNo";
            nudOrderNo.Size = new Size(150, 23);
            nudOrderNo.TabIndex = 15;
            nudOrderNo.Value = new decimal(new int[] { 100, 0, 0, 0 });
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(20, 242);
            label8.Name = "label8";
            label8.Size = new Size(56, 17);
            label8.TabIndex = 14;
            label8.Text = "排序号：";
            // 
            // txtDescription
            // 
            txtDescription.Location = new Point(120, 210);
            txtDescription.Name = "txtDescription";
            txtDescription.Size = new Size(400, 23);
            txtDescription.TabIndex = 13;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(20, 213);
            label7.Name = "label7";
            label7.Size = new Size(44, 17);
            label7.TabIndex = 12;
            label7.Text = "描述：";
            // 
            // txtIcon
            // 
            txtIcon.Location = new Point(120, 180);
            txtIcon.Name = "txtIcon";
            txtIcon.Size = new Size(400, 23);
            txtIcon.TabIndex = 11;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(20, 183);
            label6.Name = "label6";
            label6.Size = new Size(44, 17);
            label6.TabIndex = 10;
            label6.Text = "图标：";
            // 
            // txtAuth
            // 
            txtAuth.Location = new Point(120, 150);
            txtAuth.Name = "txtAuth";
            txtAuth.Size = new Size(400, 23);
            txtAuth.TabIndex = 9;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(20, 153);
            label5.Name = "label5";
            label5.Size = new Size(44, 17);
            label5.TabIndex = 8;
            label5.Text = "权限：";
            // 
            // txtUrl
            // 
            txtUrl.Location = new Point(120, 120);
            txtUrl.Name = "txtUrl";
            txtUrl.Size = new Size(400, 23);
            txtUrl.TabIndex = 7;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(20, 123);
            label4.Name = "label4";
            label4.Size = new Size(42, 17);
            label4.TabIndex = 6;
            label4.Text = "URL：";
            // 
            // txtTableName
            // 
            txtTableName.Location = new Point(120, 90);
            txtTableName.Name = "txtTableName";
            txtTableName.Size = new Size(400, 23);
            txtTableName.TabIndex = 5;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(20, 93);
            label3.Name = "label3";
            label3.Size = new Size(44, 17);
            label3.TabIndex = 4;
            label3.Text = "表名：";
            // 
            // cmbParent
            // 
            cmbParent.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbParent.FormattingEnabled = true;
            cmbParent.Location = new Point(120, 60);
            cmbParent.Name = "cmbParent";
            cmbParent.Size = new Size(400, 25);
            cmbParent.TabIndex = 3;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(20, 63);
            label2.Name = "label2";
            label2.Size = new Size(68, 17);
            label2.TabIndex = 2;
            label2.Text = "父级菜单：";
            // 
            // txtMenuName
            // 
            txtMenuName.Location = new Point(120, 30);
            txtMenuName.Name = "txtMenuName";
            txtMenuName.Size = new Size(400, 23);
            txtMenuName.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(20, 33);
            label1.Name = "label1";
            label1.Size = new Size(68, 17);
            label1.TabIndex = 0;
            label1.Text = "菜单名称：";
            // 
            // pnlButtons
            // 
            pnlButtons.BackColor = Color.FromArgb(240, 240, 240);
            pnlButtons.Controls.Add(btnCancel);
            pnlButtons.Controls.Add(btnSave);
            pnlButtons.Dock = DockStyle.Bottom;
            pnlButtons.Location = new Point(0, 501);
            pnlButtons.Name = "pnlButtons";
            pnlButtons.Size = new Size(684, 60);
            pnlButtons.TabIndex = 2;
            // 
            // btnCancel
            // 
            btnCancel.BackColor = Color.FromArgb(180, 180, 180);
            btnCancel.FlatAppearance.BorderColor = Color.FromArgb(160, 160, 160);
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            btnCancel.Location = new Point(367, 15);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(90, 30);
            btnCancel.TabIndex = 1;
            btnCancel.Text = "取消";
            btnCancel.UseVisualStyleBackColor = false;
            btnCancel.Click += btnCancel_Click;
            // 
            // btnSave
            // 
            btnSave.BackColor = Color.FromArgb(45, 45, 48);
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            btnSave.ForeColor = Color.White;
            btnSave.Location = new Point(227, 15);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(90, 30);
            btnSave.TabIndex = 0;
            btnSave.Text = "保存";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;
            // 
            // MenuEditForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(684, 561);
            Controls.Add(pnlContent);
            Controls.Add(lblTitle);
            Controls.Add(pnlButtons);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "MenuEditForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "编辑菜单";
            Load += MenuEditForm_Load;
            pnlContent.ResumeLayout(false);
            tabControl1.ResumeLayout(false);
            tabPageBasic.ResumeLayout(false);
            tabPageBasic.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudOrderNo).EndInit();
            pnlButtons.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.Panel pnlButtons;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageBasic;
        private System.Windows.Forms.TextBox txtMenuName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbParent;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtTableName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtUrl;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtAuth;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtIcon;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown nudOrderNo;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox chkEnable;
        private System.Windows.Forms.ComboBox cmbMenuType;
        private System.Windows.Forms.Label label9;
    }

}