using System;
using System.Drawing;
using System.Windows.Forms;


namespace GC_MES.WinForm.Forms.SystemForm.SubForm
{
    partial class UserEditForm
    {
        /// <summary>
        /// 必需的设计器变量
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false</param>
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
        /// 此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            pnlTop = new Panel();
            lblTitle = new Label();
            pnlBottom = new Panel();
            btnCancel = new Button();
            btnSave = new Button();
            pnlContent = new Panel();
            groupBox2 = new GroupBox();
            txtRemark = new TextBox();
            groupBox1 = new GroupBox();
            tableLayoutPanel1 = new TableLayoutPanel();
            label9 = new Label();
            txtAddress = new TextBox();
            label8 = new Label();
            cmbDept = new ComboBox();
            label7 = new Label();
            cmbRole = new ComboBox();
            label6 = new Label();
            txtTel = new TextBox();
            label5 = new Label();
            txtEmail = new TextBox();
            label4 = new Label();
            txtMobile = new TextBox();
            label3 = new Label();
            pnlGender = new Panel();
            rbFemale = new RadioButton();
            rbMale = new RadioButton();
            label2 = new Label();
            txtTrueName = new TextBox();
            label1 = new Label();
            txtUserName = new TextBox();
            label10 = new Label();
            chkEnable = new CheckBox();
            pnlTop.SuspendLayout();
            pnlBottom.SuspendLayout();
            pnlContent.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox1.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            pnlGender.SuspendLayout();
            SuspendLayout();
            // 
            // pnlTop
            // 
            pnlTop.BackColor = Color.FromArgb(45, 63, 88);
            pnlTop.Controls.Add(lblTitle);
            pnlTop.Dock = DockStyle.Top;
            pnlTop.Location = new Point(0, 0);
            pnlTop.Margin = new Padding(4, 4, 4, 4);
            pnlTop.Name = "pnlTop";
            pnlTop.Size = new Size(700, 71);
            pnlTop.TabIndex = 0;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 134);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(23, 21);
            lblTitle.Margin = new Padding(4, 0, 4, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(74, 22);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "编辑用户";
            // 
            // pnlBottom
            // 
            pnlBottom.BackColor = Color.FromArgb(240, 240, 240);
            pnlBottom.Controls.Add(btnCancel);
            pnlBottom.Controls.Add(btnSave);
            pnlBottom.Dock = DockStyle.Bottom;
            pnlBottom.Location = new Point(0, 728);
            pnlBottom.Margin = new Padding(4, 4, 4, 4);
            pnlBottom.Name = "pnlBottom";
            pnlBottom.Size = new Size(700, 85);
            pnlBottom.TabIndex = 1;
            // 
            // btnCancel
            // 
            btnCancel.BackColor = Color.FromArgb(180, 180, 180);
            btnCancel.FlatAppearance.BorderColor = Color.FromArgb(160, 160, 160);
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 134);
            btnCancel.ForeColor = Color.Black;
            btnCancel.Location = new Point(578, 21);
            btnCancel.Margin = new Padding(4, 4, 4, 4);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(105, 42);
            btnCancel.TabIndex = 1;
            btnCancel.Text = "取消";
            btnCancel.UseVisualStyleBackColor = false;
            // 
            // btnSave
            // 
            btnSave.BackColor = Color.FromArgb(45, 63, 88);
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 134);
            btnSave.ForeColor = Color.White;
            btnSave.Location = new Point(461, 21);
            btnSave.Margin = new Padding(4, 4, 4, 4);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(105, 42);
            btnSave.TabIndex = 0;
            btnSave.Text = "保存";
            btnSave.UseVisualStyleBackColor = false;
            // 
            // pnlContent
            // 
            pnlContent.BackColor = Color.White;
            pnlContent.Controls.Add(groupBox2);
            pnlContent.Controls.Add(groupBox1);
            pnlContent.Dock = DockStyle.Fill;
            pnlContent.Location = new Point(0, 71);
            pnlContent.Margin = new Padding(4, 4, 4, 4);
            pnlContent.Name = "pnlContent";
            pnlContent.Padding = new Padding(23, 28, 23, 28);
            pnlContent.Size = new Size(700, 657);
            pnlContent.TabIndex = 2;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(txtRemark);
            groupBox2.Dock = DockStyle.Fill;
            groupBox2.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            groupBox2.Location = new Point(23, 496);
            groupBox2.Margin = new Padding(4, 4, 4, 4);
            groupBox2.Name = "groupBox2";
            groupBox2.Padding = new Padding(18, 21, 18, 21);
            groupBox2.Size = new Size(654, 133);
            groupBox2.TabIndex = 1;
            groupBox2.TabStop = false;
            groupBox2.Text = "备注";
            // 
            // txtRemark
            // 
            txtRemark.Dock = DockStyle.Fill;
            txtRemark.Location = new Point(18, 37);
            txtRemark.Margin = new Padding(4, 4, 4, 4);
            txtRemark.Multiline = true;
            txtRemark.Name = "txtRemark";
            txtRemark.Size = new Size(618, 75);
            txtRemark.TabIndex = 0;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(tableLayoutPanel1);
            groupBox1.Dock = DockStyle.Top;
            groupBox1.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            groupBox1.Location = new Point(23, 28);
            groupBox1.Margin = new Padding(4, 4, 4, 4);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new Padding(18, 21, 18, 21);
            groupBox1.Size = new Size(654, 468);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "基本信息";
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 4;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 93F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 93F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(label9, 2, 4);
            tableLayoutPanel1.Controls.Add(txtAddress, 3, 4);
            tableLayoutPanel1.Controls.Add(label8, 0, 4);
            tableLayoutPanel1.Controls.Add(cmbDept, 1, 4);
            tableLayoutPanel1.Controls.Add(label7, 2, 3);
            tableLayoutPanel1.Controls.Add(cmbRole, 3, 3);
            tableLayoutPanel1.Controls.Add(label6, 0, 3);
            tableLayoutPanel1.Controls.Add(txtTel, 1, 3);
            tableLayoutPanel1.Controls.Add(label5, 2, 2);
            tableLayoutPanel1.Controls.Add(txtEmail, 3, 2);
            tableLayoutPanel1.Controls.Add(label4, 0, 2);
            tableLayoutPanel1.Controls.Add(txtMobile, 1, 2);
            tableLayoutPanel1.Controls.Add(label3, 2, 1);
            tableLayoutPanel1.Controls.Add(pnlGender, 3, 1);
            tableLayoutPanel1.Controls.Add(label2, 0, 1);
            tableLayoutPanel1.Controls.Add(txtTrueName, 1, 1);
            tableLayoutPanel1.Controls.Add(label1, 0, 0);
            tableLayoutPanel1.Controls.Add(txtUserName, 1, 0);
            tableLayoutPanel1.Controls.Add(label10, 2, 0);
            tableLayoutPanel1.Controls.Add(chkEnable, 3, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(18, 37);
            tableLayoutPanel1.Margin = new Padding(4, 4, 4, 4);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 6;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 64F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 64F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 64F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 64F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 64F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(618, 410);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // label9
            // 
            label9.Anchor = AnchorStyles.Right;
            label9.AutoSize = true;
            label9.Location = new Point(354, 279);
            label9.Margin = new Padding(4, 0, 4, 0);
            label9.Name = "label9";
            label9.Size = new Size(44, 17);
            label9.TabIndex = 18;
            label9.Text = "地址：";
            // 
            // txtAddress
            // 
            txtAddress.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtAddress.Location = new Point(406, 276);
            txtAddress.Margin = new Padding(4, 4, 4, 4);
            txtAddress.Name = "txtAddress";
            txtAddress.Size = new Size(208, 23);
            txtAddress.TabIndex = 19;
            // 
            // label8
            // 
            label8.Anchor = AnchorStyles.Right;
            label8.AutoSize = true;
            label8.Location = new Point(45, 279);
            label8.Margin = new Padding(4, 0, 4, 0);
            label8.Name = "label8";
            label8.Size = new Size(44, 17);
            label8.TabIndex = 16;
            label8.Text = "部门：";
            // 
            // cmbDept
            // 
            cmbDept.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            cmbDept.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbDept.FormattingEnabled = true;
            cmbDept.Location = new Point(97, 275);
            cmbDept.Margin = new Padding(4, 4, 4, 4);
            cmbDept.Name = "cmbDept";
            cmbDept.Size = new Size(208, 25);
            cmbDept.TabIndex = 17;
            // 
            // label7
            // 
            label7.Anchor = AnchorStyles.Right;
            label7.AutoSize = true;
            label7.Location = new Point(354, 215);
            label7.Margin = new Padding(4, 0, 4, 0);
            label7.Name = "label7";
            label7.Size = new Size(44, 17);
            label7.TabIndex = 14;
            label7.Text = "角色：";
            // 
            // cmbRole
            // 
            cmbRole.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            cmbRole.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbRole.FormattingEnabled = true;
            cmbRole.Location = new Point(406, 211);
            cmbRole.Margin = new Padding(4, 4, 4, 4);
            cmbRole.Name = "cmbRole";
            cmbRole.Size = new Size(208, 25);
            cmbRole.TabIndex = 15;
            // 
            // label6
            // 
            label6.Anchor = AnchorStyles.Right;
            label6.AutoSize = true;
            label6.Location = new Point(45, 215);
            label6.Margin = new Padding(4, 0, 4, 0);
            label6.Name = "label6";
            label6.Size = new Size(44, 17);
            label6.TabIndex = 12;
            label6.Text = "电话：";
            // 
            // txtTel
            // 
            txtTel.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtTel.Location = new Point(97, 212);
            txtTel.Margin = new Padding(4, 4, 4, 4);
            txtTel.Name = "txtTel";
            txtTel.Size = new Size(208, 23);
            txtTel.TabIndex = 13;
            // 
            // label5
            // 
            label5.Anchor = AnchorStyles.Right;
            label5.AutoSize = true;
            label5.Location = new Point(354, 151);
            label5.Margin = new Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new Size(44, 17);
            label5.TabIndex = 10;
            label5.Text = "邮箱：";
            // 
            // txtEmail
            // 
            txtEmail.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtEmail.Location = new Point(406, 148);
            txtEmail.Margin = new Padding(4, 4, 4, 4);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(208, 23);
            txtEmail.TabIndex = 11;
            // 
            // label4
            // 
            label4.Anchor = AnchorStyles.Right;
            label4.AutoSize = true;
            label4.Location = new Point(45, 151);
            label4.Margin = new Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new Size(44, 17);
            label4.TabIndex = 8;
            label4.Text = "手机：";
            // 
            // txtMobile
            // 
            txtMobile.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtMobile.Location = new Point(97, 148);
            txtMobile.Margin = new Padding(4, 4, 4, 4);
            txtMobile.Name = "txtMobile";
            txtMobile.Size = new Size(208, 23);
            txtMobile.TabIndex = 9;
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.Right;
            label3.AutoSize = true;
            label3.Location = new Point(354, 87);
            label3.Margin = new Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new Size(44, 17);
            label3.TabIndex = 6;
            label3.Text = "性别：";
            // 
            // pnlGender
            // 
            pnlGender.Anchor = AnchorStyles.Left;
            pnlGender.Controls.Add(rbFemale);
            pnlGender.Controls.Add(rbMale);
            pnlGender.Location = new Point(406, 71);
            pnlGender.Margin = new Padding(4, 4, 4, 4);
            pnlGender.Name = "pnlGender";
            pnlGender.Size = new Size(158, 50);
            pnlGender.TabIndex = 7;
            // 
            // rbFemale
            // 
            rbFemale.AutoSize = true;
            rbFemale.Location = new Point(82, 11);
            rbFemale.Margin = new Padding(4, 4, 4, 4);
            rbFemale.Name = "rbFemale";
            rbFemale.Size = new Size(38, 21);
            rbFemale.TabIndex = 1;
            rbFemale.Text = "女";
            rbFemale.UseVisualStyleBackColor = true;
            // 
            // rbMale
            // 
            rbMale.AutoSize = true;
            rbMale.Checked = true;
            rbMale.Location = new Point(18, 11);
            rbMale.Margin = new Padding(4, 4, 4, 4);
            rbMale.Name = "rbMale";
            rbMale.Size = new Size(38, 21);
            rbMale.TabIndex = 0;
            rbMale.TabStop = true;
            rbMale.Text = "男";
            rbMale.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Right;
            label2.AutoSize = true;
            label2.Location = new Point(45, 87);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(44, 17);
            label2.TabIndex = 2;
            label2.Text = "姓名：";
            // 
            // txtTrueName
            // 
            txtTrueName.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtTrueName.Location = new Point(97, 84);
            txtTrueName.Margin = new Padding(4, 4, 4, 4);
            txtTrueName.Name = "txtTrueName";
            txtTrueName.Size = new Size(208, 23);
            txtTrueName.TabIndex = 3;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Location = new Point(33, 23);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(56, 17);
            label1.TabIndex = 0;
            label1.Text = "用户名：";
            // 
            // txtUserName
            // 
            txtUserName.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtUserName.Location = new Point(97, 20);
            txtUserName.Margin = new Padding(4, 4, 4, 4);
            txtUserName.Name = "txtUserName";
            txtUserName.Size = new Size(208, 23);
            txtUserName.TabIndex = 1;
            // 
            // label10
            // 
            label10.Anchor = AnchorStyles.Right;
            label10.AutoSize = true;
            label10.Location = new Point(354, 23);
            label10.Margin = new Padding(4, 0, 4, 0);
            label10.Name = "label10";
            label10.Size = new Size(44, 17);
            label10.TabIndex = 20;
            label10.Text = "启用：";
            // 
            // chkEnable
            // 
            chkEnable.Anchor = AnchorStyles.Left;
            chkEnable.AutoSize = true;
            chkEnable.Checked = true;
            chkEnable.CheckState = CheckState.Checked;
            chkEnable.Location = new Point(406, 25);
            chkEnable.Margin = new Padding(4, 4, 4, 4);
            chkEnable.Name = "chkEnable";
            chkEnable.Size = new Size(15, 14);
            chkEnable.TabIndex = 21;
            chkEnable.UseVisualStyleBackColor = true;
            // 
            // UserEditForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(700, 813);
            Controls.Add(pnlContent);
            Controls.Add(pnlBottom);
            Controls.Add(pnlTop);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(4, 4, 4, 4);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "UserEditForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "用户编辑";
            pnlTop.ResumeLayout(false);
            pnlTop.PerformLayout();
            pnlBottom.ResumeLayout(false);
            pnlContent.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox1.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            pnlGender.ResumeLayout(false);
            pnlGender.PerformLayout();
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtRemark;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtTrueName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel pnlGender;
        private System.Windows.Forms.RadioButton rbFemale;
        private System.Windows.Forms.RadioButton rbMale;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtMobile;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtTel;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cmbRole;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cmbDept;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtAddress;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox chkEnable;
    }
}