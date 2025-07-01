namespace GC_MES.WinForm.Forms.SystemForm
{
    partial class LoginInfoManagementForm
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
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle7 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle8 = new DataGridViewCellStyle();
            pnlSearch = new Panel();
            btnClear = new Button();
            btnSearch = new Button();
            txtUserName = new TextBox();
            lblUserName = new Label();
            txtPassword = new TextBox();
            lblPassword = new Label();
            txtVerificationCode = new TextBox();
            lblVerificationCode = new Label();
            pnlToolbar = new Panel();
            btnExportPDF = new Button();
            btnExport = new Button();
            btnImport = new Button();
            btnDelete = new Button();
            btnEdit = new Button();
            btnAdd = new Button();
            dgvLoginInfos = new DataGridView();
            colUserName = new DataGridViewTextBoxColumn();
            colPassword = new DataGridViewTextBoxColumn();
            colVerificationCode = new DataGridViewTextBoxColumn();
            colUUID = new DataGridViewTextBoxColumn();
            pnlPager = new Panel();
            lblPageInfo = new Label();
            btnLastPage = new Button();
            btnNextPage = new Button();
            btnPrevPage = new Button();
            btnFirstPage = new Button();
            pnlSearch.SuspendLayout();
            pnlToolbar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvLoginInfos).BeginInit();
            pnlPager.SuspendLayout();
            SuspendLayout();
            // 
            // pnlSearch
            // 
            pnlSearch.BackColor = Color.FromArgb(250, 250, 250);
            pnlSearch.Controls.Add(btnClear);
            pnlSearch.Controls.Add(btnSearch);
            pnlSearch.Controls.Add(txtUserName);
            pnlSearch.Controls.Add(lblUserName);
            pnlSearch.Controls.Add(txtPassword);
            pnlSearch.Controls.Add(lblPassword);
            pnlSearch.Controls.Add(txtVerificationCode);
            pnlSearch.Controls.Add(lblVerificationCode);
            pnlSearch.Dock = DockStyle.Top;
            pnlSearch.Location = new Point(10, 10);
            pnlSearch.Margin = new Padding(4);
            pnlSearch.Name = "pnlSearch";
            pnlSearch.Size = new Size(1473, 152);
            pnlSearch.TabIndex = 1;
            // 
            // btnClear
            // 
            btnClear.BackColor = Color.FromArgb(180, 180, 180);
            btnClear.FlatAppearance.BorderColor = Color.FromArgb(160, 160, 160);
            btnClear.FlatStyle = FlatStyle.Flat;
            btnClear.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            btnClear.Location = new Point(688, 42);
            btnClear.Margin = new Padding(4);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(88, 40);
            btnClear.TabIndex = 5;
            btnClear.Text = "清空";
            btnClear.UseVisualStyleBackColor = false;
            // 
            // btnSearch
            // 
            btnSearch.BackColor = Color.FromArgb(45, 45, 48);
            btnSearch.FlatAppearance.BorderSize = 0;
            btnSearch.FlatStyle = FlatStyle.Flat;
            btnSearch.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            btnSearch.ForeColor = Color.White;
            btnSearch.Location = new Point(583, 42);
            btnSearch.Margin = new Padding(4);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new Size(88, 40);
            btnSearch.TabIndex = 4;
            btnSearch.Text = "搜索";
            btnSearch.UseVisualStyleBackColor = false;
            // 
            // txtUserName
            // 
            txtUserName.BorderStyle = BorderStyle.FixedSingle;
            txtUserName.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            txtUserName.Location = new Point(128, 42);
            txtUserName.Margin = new Padding(4);
            txtUserName.Name = "txtUserName";
            txtUserName.Size = new Size(175, 23);
            txtUserName.TabIndex = 1;
            // 
            // lblUserName
            // 
            lblUserName.AutoSize = true;
            lblUserName.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            lblUserName.Location = new Point(23, 42);
            lblUserName.Margin = new Padding(4, 0, 4, 0);
            lblUserName.Name = "lblUserName";
            lblUserName.Size = new Size(56, 17);
            lblUserName.TabIndex = 0;
            lblUserName.Text = "用户名：";
            // 
            // txtPassword
            // 
            txtPassword.BorderStyle = BorderStyle.FixedSingle;
            txtPassword.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            txtPassword.Location = new Point(373, 72);
            txtPassword.Margin = new Padding(4);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(175, 23);
            txtPassword.TabIndex = 3;
            // 
            // lblPassword
            // 
            lblPassword.AutoSize = true;
            lblPassword.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            lblPassword.Location = new Point(327, 72);
            lblPassword.Margin = new Padding(4, 0, 4, 0);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(44, 17);
            lblPassword.TabIndex = 2;
            lblPassword.Text = "密码：";
            // 
            // txtVerificationCode
            // 
            txtVerificationCode.BorderStyle = BorderStyle.FixedSingle;
            txtVerificationCode.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            txtVerificationCode.Location = new Point(128, 102);
            txtVerificationCode.Margin = new Padding(4);
            txtVerificationCode.Name = "txtVerificationCode";
            txtVerificationCode.Size = new Size(175, 23);
            txtVerificationCode.TabIndex = 5;
            // 
            // lblVerificationCode
            // 
            lblVerificationCode.AutoSize = true;
            lblVerificationCode.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            lblVerificationCode.Location = new Point(23, 102);
            lblVerificationCode.Margin = new Padding(4, 0, 4, 0);
            lblVerificationCode.Name = "lblVerificationCode";
            lblVerificationCode.Size = new Size(56, 17);
            lblVerificationCode.TabIndex = 4;
            lblVerificationCode.Text = "验证码：";
            // 
            // pnlToolbar
            // 
            pnlToolbar.BackColor = Color.FromArgb(240, 240, 240);
            pnlToolbar.Controls.Add(btnExportPDF);
            pnlToolbar.Controls.Add(btnExport);
            pnlToolbar.Controls.Add(btnImport);
            pnlToolbar.Controls.Add(btnDelete);
            pnlToolbar.Controls.Add(btnEdit);
            pnlToolbar.Controls.Add(btnAdd);
            pnlToolbar.Dock = DockStyle.Top;
            pnlToolbar.Location = new Point(10, 162);
            pnlToolbar.Margin = new Padding(4);
            pnlToolbar.Name = "pnlToolbar";
            pnlToolbar.Size = new Size(1473, 71);
            pnlToolbar.TabIndex = 2;
            // 
            // btnExportPDF
            // 
            btnExportPDF.BackColor = Color.FromArgb(80, 80, 85);
            btnExportPDF.FlatAppearance.BorderSize = 0;
            btnExportPDF.FlatStyle = FlatStyle.Flat;
            btnExportPDF.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            btnExportPDF.ForeColor = Color.White;
            btnExportPDF.Location = new Point(595, 17);
            btnExportPDF.Margin = new Padding(4);
            btnExportPDF.Name = "btnExportPDF";
            btnExportPDF.Size = new Size(105, 40);
            btnExportPDF.TabIndex = 5;
            btnExportPDF.Text = "导出PDF";
            btnExportPDF.UseVisualStyleBackColor = false;
            // 
            // btnExport
            // 
            btnExport.BackColor = Color.FromArgb(80, 80, 85);
            btnExport.FlatAppearance.BorderSize = 0;
            btnExport.FlatStyle = FlatStyle.Flat;
            btnExport.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            btnExport.ForeColor = Color.White;
            btnExport.Location = new Point(478, 17);
            btnExport.Margin = new Padding(4);
            btnExport.Name = "btnExport";
            btnExport.Size = new Size(105, 40);
            btnExport.TabIndex = 4;
            btnExport.Text = "导出Excel";
            btnExport.UseVisualStyleBackColor = false;
            // 
            // btnImport
            // 
            btnImport.BackColor = Color.FromArgb(80, 80, 85);
            btnImport.FlatAppearance.BorderSize = 0;
            btnImport.FlatStyle = FlatStyle.Flat;
            btnImport.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            btnImport.ForeColor = Color.White;
            btnImport.Location = new Point(362, 17);
            btnImport.Margin = new Padding(4);
            btnImport.Name = "btnImport";
            btnImport.Size = new Size(105, 40);
            btnImport.TabIndex = 3;
            btnImport.Text = "导入Excel";
            btnImport.UseVisualStyleBackColor = false;
            // 
            // btnDelete
            // 
            btnDelete.BackColor = Color.FromArgb(159, 68, 74);
            btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.FlatStyle = FlatStyle.Flat;
            btnDelete.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            btnDelete.ForeColor = Color.White;
            btnDelete.Location = new Point(245, 17);
            btnDelete.Margin = new Padding(4);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(105, 40);
            btnDelete.TabIndex = 2;
            btnDelete.Text = "删除";
            btnDelete.UseVisualStyleBackColor = false;
            // 
            // btnEdit
            // 
            btnEdit.BackColor = Color.FromArgb(67, 67, 70);
            btnEdit.FlatAppearance.BorderSize = 0;
            btnEdit.FlatStyle = FlatStyle.Flat;
            btnEdit.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            btnEdit.ForeColor = Color.White;
            btnEdit.Location = new Point(128, 17);
            btnEdit.Margin = new Padding(4);
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new Size(105, 40);
            btnEdit.TabIndex = 1;
            btnEdit.Text = "编辑";
            btnEdit.UseVisualStyleBackColor = false;
            // 
            // btnAdd
            // 
            btnAdd.BackColor = Color.FromArgb(45, 45, 48);
            btnAdd.FlatAppearance.BorderSize = 0;
            btnAdd.FlatStyle = FlatStyle.Flat;
            btnAdd.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            btnAdd.ForeColor = Color.White;
            btnAdd.Location = new Point(12, 17);
            btnAdd.Margin = new Padding(4);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(105, 40);
            btnAdd.TabIndex = 0;
            btnAdd.Text = "新增";
            btnAdd.UseVisualStyleBackColor = false;
            // 
            // dgvLoginInfos
            // 
            dgvLoginInfos.AllowUserToAddRows = false;
            dgvLoginInfos.AllowUserToDeleteRows = false;
            dataGridViewCellStyle5.BackColor = Color.FromArgb(250, 250, 250);
            dgvLoginInfos.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle5;
            dgvLoginInfos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvLoginInfos.BackgroundColor = Color.White;
            dgvLoginInfos.BorderStyle = BorderStyle.None;
            dgvLoginInfos.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = Color.FromArgb(45, 45, 48);
            dataGridViewCellStyle6.Font = new Font("Microsoft YaHei UI", 9F);
            dataGridViewCellStyle6.ForeColor = Color.White;
            dataGridViewCellStyle6.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = DataGridViewTriState.True;
            dgvLoginInfos.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle6;
            dgvLoginInfos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvLoginInfos.Columns.AddRange(new DataGridViewColumn[] { colUserName, colPassword, colVerificationCode, colUUID });
            dataGridViewCellStyle7.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = SystemColors.Window;
            dataGridViewCellStyle7.Font = new Font("Microsoft YaHei UI", 9F);
            dataGridViewCellStyle7.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle7.SelectionBackColor = Color.FromArgb(67, 67, 70);
            dataGridViewCellStyle7.SelectionForeColor = Color.White;
            dataGridViewCellStyle7.WrapMode = DataGridViewTriState.False;
            dgvLoginInfos.DefaultCellStyle = dataGridViewCellStyle7;
            dgvLoginInfos.Dock = DockStyle.Fill;
            dgvLoginInfos.EnableHeadersVisualStyles = false;
            dgvLoginInfos.GridColor = Color.FromArgb(230, 230, 230);
            dgvLoginInfos.Location = new Point(10, 233);
            dgvLoginInfos.Margin = new Padding(4);
            dgvLoginInfos.MultiSelect = false;
            dgvLoginInfos.Name = "dgvLoginInfos";
            dgvLoginInfos.ReadOnly = true;
            dgvLoginInfos.RowHeadersVisible = false;
            dataGridViewCellStyle8.BackColor = Color.White;
            dgvLoginInfos.RowsDefaultCellStyle = dataGridViewCellStyle8;
            dgvLoginInfos.RowTemplate.Height = 36;
            dgvLoginInfos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvLoginInfos.Size = new Size(1473, 678);
            dgvLoginInfos.TabIndex = 3;
            // 
            // colUserName
            // 
            colUserName.DataPropertyName = "UserName";
            colUserName.HeaderText = "用户名";
            colUserName.Name = "colUserName";
            colUserName.ReadOnly = true;
            // 
            // colPassword
            // 
            colPassword.DataPropertyName = "Password";
            colPassword.HeaderText = "密码";
            colPassword.Name = "colPassword";
            colPassword.ReadOnly = true;
            // 
            // colVerificationCode
            // 
            colVerificationCode.DataPropertyName = "VerificationCode";
            colVerificationCode.HeaderText = "验证码";
            colVerificationCode.Name = "colVerificationCode";
            colVerificationCode.ReadOnly = true;
            // 
            // colUUID
            // 
            colUUID.DataPropertyName = "UUID";
            colUUID.HeaderText = "UUID";
            colUUID.Name = "colUUID";
            colUUID.ReadOnly = true;
            // 
            // pnlPager
            // 
            pnlPager.BackColor = Color.FromArgb(240, 240, 240);
            pnlPager.Controls.Add(lblPageInfo);
            pnlPager.Controls.Add(btnLastPage);
            pnlPager.Controls.Add(btnNextPage);
            pnlPager.Controls.Add(btnPrevPage);
            pnlPager.Controls.Add(btnFirstPage);
            pnlPager.Dock = DockStyle.Bottom;
            pnlPager.Location = new Point(10, 911);
            pnlPager.Margin = new Padding(4);
            pnlPager.Name = "pnlPager";
            pnlPager.Size = new Size(1473, 71);
            pnlPager.TabIndex = 4;
            // 
            // lblPageInfo
            // 
            lblPageInfo.AutoSize = true;
            lblPageInfo.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            lblPageInfo.Location = new Point(233, 24);
            lblPageInfo.Margin = new Padding(4, 0, 4, 0);
            lblPageInfo.Name = "lblPageInfo";
            lblPageInfo.Size = new Size(110, 17);
            lblPageInfo.TabIndex = 4;
            lblPageInfo.Text = "第 1/1 页，共 0 条";
            // 
            // btnLastPage
            // 
            btnLastPage.BackColor = Color.FromArgb(230, 230, 230);
            btnLastPage.FlatAppearance.BorderColor = Color.FromArgb(210, 210, 210);
            btnLastPage.FlatStyle = FlatStyle.Flat;
            btnLastPage.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            btnLastPage.Location = new Point(183, 17);
            btnLastPage.Margin = new Padding(4);
            btnLastPage.Name = "btnLastPage";
            btnLastPage.Size = new Size(41, 40);
            btnLastPage.TabIndex = 3;
            btnLastPage.Text = ">|";
            btnLastPage.UseVisualStyleBackColor = false;
            // 
            // btnNextPage
            // 
            btnNextPage.BackColor = Color.FromArgb(230, 230, 230);
            btnNextPage.FlatAppearance.BorderColor = Color.FromArgb(210, 210, 210);
            btnNextPage.FlatStyle = FlatStyle.Flat;
            btnNextPage.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            btnNextPage.Location = new Point(126, 17);
            btnNextPage.Margin = new Padding(4);
            btnNextPage.Name = "btnNextPage";
            btnNextPage.Size = new Size(41, 40);
            btnNextPage.TabIndex = 2;
            btnNextPage.Text = ">";
            btnNextPage.UseVisualStyleBackColor = false;
            // 
            // btnPrevPage
            // 
            btnPrevPage.BackColor = Color.FromArgb(230, 230, 230);
            btnPrevPage.FlatAppearance.BorderColor = Color.FromArgb(210, 210, 210);
            btnPrevPage.FlatStyle = FlatStyle.Flat;
            btnPrevPage.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            btnPrevPage.Location = new Point(69, 17);
            btnPrevPage.Margin = new Padding(4);
            btnPrevPage.Name = "btnPrevPage";
            btnPrevPage.Size = new Size(41, 40);
            btnPrevPage.TabIndex = 1;
            btnPrevPage.Text = "<";
            btnPrevPage.UseVisualStyleBackColor = false;
            // 
            // btnFirstPage
            // 
            btnFirstPage.BackColor = Color.FromArgb(230, 230, 230);
            btnFirstPage.FlatAppearance.BorderColor = Color.FromArgb(210, 210, 210);
            btnFirstPage.FlatStyle = FlatStyle.Flat;
            btnFirstPage.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            btnFirstPage.Location = new Point(12, 17);
            btnFirstPage.Margin = new Padding(4);
            btnFirstPage.Name = "btnFirstPage";
            btnFirstPage.Size = new Size(41, 40);
            btnFirstPage.TabIndex = 0;
            btnFirstPage.Text = "|<";
            btnFirstPage.UseVisualStyleBackColor = false;
            // 
            // LoginInfoManagementForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1493, 992);
            Controls.Add(dgvLoginInfos);
            Controls.Add(pnlPager);
            Controls.Add(pnlToolbar);
            Controls.Add(pnlSearch);
            Margin = new Padding(4);
            Name = "LoginInfoManagementForm";
            Padding = new Padding(10);
            StartPosition = FormStartPosition.CenterScreen;
            Text = "LoginInfo管理";
            pnlSearch.ResumeLayout(false);
            pnlSearch.PerformLayout();
            pnlToolbar.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvLoginInfos).EndInit();
            pnlPager.ResumeLayout(false);
            pnlPager.PerformLayout();
            ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel pnlSearch;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.Label lblUserName;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtVerificationCode;
        private System.Windows.Forms.Label lblVerificationCode;
        private System.Windows.Forms.Panel pnlToolbar;
        private System.Windows.Forms.Button btnExportPDF;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.DataGridView dgvLoginInfos;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUserName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPassword;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVerificationCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUUID;
        private System.Windows.Forms.Panel pnlPager;
        private System.Windows.Forms.Label lblPageInfo;
        private System.Windows.Forms.Button btnLastPage;
        private System.Windows.Forms.Button btnNextPage;
        private System.Windows.Forms.Button btnPrevPage;
        private System.Windows.Forms.Button btnFirstPage;
    }
}
