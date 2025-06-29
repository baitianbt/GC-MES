namespace GC_MES.WinForm.Forms.SystemForm.SubForm
{
    partial class RoleEditForm
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
            tabControl1 = new TabControl();
            tabPageBasic = new TabPage();
            tableLayoutPanel1 = new TableLayoutPanel();
            label1 = new Label();
            txtRoleName = new TextBox();
            label2 = new Label();
            cmbParentRole = new ComboBox();
            label3 = new Label();
            cmbDepartment = new ComboBox();
            label4 = new Label();
            numOrder = new NumericUpDown();
            label5 = new Label();
            chkEnabled = new CheckBox();
            label6 = new Label();
            txtRoleDesc = new TextBox();
            tabPagePermission = new TabPage();
            trvPermissions = new TreeView();
            pnlTop.SuspendLayout();
            pnlBottom.SuspendLayout();
            pnlContent.SuspendLayout();
            tabControl1.SuspendLayout();
            tabPageBasic.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numOrder).BeginInit();
            tabPagePermission.SuspendLayout();
            SuspendLayout();
            // 
            // pnlTop
            // 
            pnlTop.BackColor = Color.FromArgb(45, 45, 48);
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
            lblTitle.Text = "编辑角色";
            // 
            // pnlBottom
            // 
            pnlBottom.BackColor = Color.FromArgb(240, 240, 240);
            pnlBottom.Controls.Add(btnCancel);
            pnlBottom.Controls.Add(btnSave);
            pnlBottom.Dock = DockStyle.Bottom;
            pnlBottom.Location = new Point(0, 637);
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
            btnSave.BackColor = Color.FromArgb(45, 45, 48);
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
            pnlContent.Controls.Add(tabControl1);
            pnlContent.Dock = DockStyle.Fill;
            pnlContent.Location = new Point(0, 71);
            pnlContent.Margin = new Padding(4, 4, 4, 4);
            pnlContent.Name = "pnlContent";
            pnlContent.Padding = new Padding(23, 28, 23, 28);
            pnlContent.Size = new Size(700, 566);
            pnlContent.TabIndex = 2;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPageBasic);
            tabControl1.Controls.Add(tabPagePermission);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            tabControl1.Location = new Point(23, 28);
            tabControl1.Margin = new Padding(4, 4, 4, 4);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(654, 510);
            tabControl1.TabIndex = 0;
            // 
            // tabPageBasic
            // 
            tabPageBasic.BackColor = Color.White;
            tabPageBasic.Controls.Add(tableLayoutPanel1);
            tabPageBasic.Location = new Point(4, 26);
            tabPageBasic.Margin = new Padding(4, 4, 4, 4);
            tabPageBasic.Name = "tabPageBasic";
            tabPageBasic.Padding = new Padding(18, 21, 18, 21);
            tabPageBasic.Size = new Size(646, 480);
            tabPageBasic.TabIndex = 0;
            tabPageBasic.Text = "基本信息";
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 93F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(label1, 0, 0);
            tableLayoutPanel1.Controls.Add(txtRoleName, 1, 0);
            tableLayoutPanel1.Controls.Add(label2, 0, 1);
            tableLayoutPanel1.Controls.Add(cmbParentRole, 1, 1);
            tableLayoutPanel1.Controls.Add(label3, 0, 2);
            tableLayoutPanel1.Controls.Add(cmbDepartment, 1, 2);
            tableLayoutPanel1.Controls.Add(label4, 0, 3);
            tableLayoutPanel1.Controls.Add(numOrder, 1, 3);
            tableLayoutPanel1.Controls.Add(label5, 0, 4);
            tableLayoutPanel1.Controls.Add(chkEnabled, 1, 4);
            tableLayoutPanel1.Controls.Add(label6, 0, 5);
            tableLayoutPanel1.Controls.Add(txtRoleDesc, 1, 5);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(18, 21);
            tableLayoutPanel1.Margin = new Padding(4, 4, 4, 4);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 7;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 64F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 64F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 64F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 64F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 64F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 64F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(610, 438);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Location = new Point(30, 23);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(59, 17);
            label1.TabIndex = 0;
            label1.Text = "角色名称:";
            // 
            // txtRoleName
            // 
            txtRoleName.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtRoleName.Location = new Point(97, 20);
            txtRoleName.Margin = new Padding(4, 4, 4, 4);
            txtRoleName.Name = "txtRoleName";
            txtRoleName.Size = new Size(509, 23);
            txtRoleName.TabIndex = 1;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Right;
            label2.AutoSize = true;
            label2.Location = new Point(30, 87);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(59, 17);
            label2.TabIndex = 2;
            label2.Text = "父级角色:";
            // 
            // cmbParentRole
            // 
            cmbParentRole.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            cmbParentRole.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbParentRole.FormattingEnabled = true;
            cmbParentRole.Location = new Point(97, 83);
            cmbParentRole.Margin = new Padding(4, 4, 4, 4);
            cmbParentRole.Name = "cmbParentRole";
            cmbParentRole.Size = new Size(509, 25);
            cmbParentRole.TabIndex = 3;
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.Right;
            label3.AutoSize = true;
            label3.Location = new Point(45, 151);
            label3.Margin = new Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new Size(44, 17);
            label3.TabIndex = 4;
            label3.Text = "部门：";
            // 
            // cmbDepartment
            // 
            cmbDepartment.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            cmbDepartment.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbDepartment.FormattingEnabled = true;
            cmbDepartment.Location = new Point(97, 147);
            cmbDepartment.Margin = new Padding(4, 4, 4, 4);
            cmbDepartment.Name = "cmbDepartment";
            cmbDepartment.Size = new Size(509, 25);
            cmbDepartment.TabIndex = 5;
            // 
            // label4
            // 
            label4.Anchor = AnchorStyles.Right;
            label4.AutoSize = true;
            label4.Location = new Point(45, 215);
            label4.Margin = new Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new Size(44, 17);
            label4.TabIndex = 6;
            label4.Text = "排序：";
            // 
            // numOrder
            // 
            numOrder.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            numOrder.Location = new Point(97, 212);
            numOrder.Margin = new Padding(4, 4, 4, 4);
            numOrder.Name = "numOrder";
            numOrder.Size = new Size(509, 23);
            numOrder.TabIndex = 7;
            // 
            // label5
            // 
            label5.Anchor = AnchorStyles.Right;
            label5.AutoSize = true;
            label5.Location = new Point(45, 279);
            label5.Margin = new Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new Size(44, 17);
            label5.TabIndex = 8;
            label5.Text = "启用：";
            // 
            // chkEnabled
            // 
            chkEnabled.Anchor = AnchorStyles.Left;
            chkEnabled.AutoSize = true;
            chkEnabled.Checked = true;
            chkEnabled.CheckState = CheckState.Checked;
            chkEnabled.Location = new Point(97, 281);
            chkEnabled.Margin = new Padding(4, 4, 4, 4);
            chkEnabled.Name = "chkEnabled";
            chkEnabled.Size = new Size(15, 14);
            chkEnabled.TabIndex = 9;
            chkEnabled.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            label6.Anchor = AnchorStyles.Right;
            label6.AutoSize = true;
            label6.Location = new Point(30, 343);
            label6.Margin = new Padding(4, 0, 4, 0);
            label6.Name = "label6";
            label6.Size = new Size(59, 17);
            label6.TabIndex = 10;
            label6.Text = "角色描述:";
            // 
            // txtRoleDesc
            // 
            txtRoleDesc.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtRoleDesc.Location = new Point(97, 340);
            txtRoleDesc.Margin = new Padding(4, 4, 4, 4);
            txtRoleDesc.Name = "txtRoleDesc";
            txtRoleDesc.Size = new Size(509, 23);
            txtRoleDesc.TabIndex = 11;
            // 
            // tabPagePermission
            // 
            tabPagePermission.BackColor = Color.White;
            tabPagePermission.Controls.Add(trvPermissions);
            tabPagePermission.Location = new Point(4, 26);
            tabPagePermission.Margin = new Padding(4, 4, 4, 4);
            tabPagePermission.Name = "tabPagePermission";
            tabPagePermission.Padding = new Padding(18, 21, 18, 21);
            tabPagePermission.Size = new Size(646, 480);
            tabPagePermission.TabIndex = 1;
            tabPagePermission.Text = "权限分配";
            // 
            // trvPermissions
            // 
            trvPermissions.CheckBoxes = true;
            trvPermissions.Dock = DockStyle.Fill;
            trvPermissions.Location = new Point(18, 21);
            trvPermissions.Margin = new Padding(4, 4, 4, 4);
            trvPermissions.Name = "trvPermissions";
            trvPermissions.Size = new Size(610, 438);
            trvPermissions.TabIndex = 0;
            // 
            // RoleEditForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(700, 722);
            Controls.Add(pnlContent);
            Controls.Add(pnlBottom);
            Controls.Add(pnlTop);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(4, 4, 4, 4);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "RoleEditForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "角色编辑";
            pnlTop.ResumeLayout(false);
            pnlTop.PerformLayout();
            pnlBottom.ResumeLayout(false);
            pnlContent.ResumeLayout(false);
            tabControl1.ResumeLayout(false);
            tabPageBasic.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numOrder).EndInit();
            tabPagePermission.ResumeLayout(false);
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageBasic;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtRoleName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbParentRole;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbDepartment;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numOrder;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox chkEnabled;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtRoleDesc;
        private System.Windows.Forms.TabPage tabPagePermission;
        private System.Windows.Forms.TreeView trvPermissions;
    }
}