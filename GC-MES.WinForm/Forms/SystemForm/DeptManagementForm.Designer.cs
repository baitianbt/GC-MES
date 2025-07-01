namespace GC_MES.WinForm.Forms.SystemForm
{
    partial class DeptManagementForm
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
            pnlSearch = new Panel();
            btnClear = new Button();
            btnSearch = new Button();
            txtSearchCode = new TextBox();
            label2 = new Label();
            txtSearchName = new TextBox();
            label1 = new Label();
            pnlToolbar = new Panel();
            btnExportPDF = new Button();
            btnExport = new Button();
            btnImport = new Button();
            btnDelete = new Button();
            btnEdit = new Button();
            btnAdd = new Button();
            dgvDepts = new DataGridView();
            colDeptId = new DataGridViewTextBoxColumn();
            colDeptName = new DataGridViewTextBoxColumn();
            colDeptCode = new DataGridViewTextBoxColumn();
            colCreator = new DataGridViewTextBoxColumn();
            colCreateDate = new DataGridViewTextBoxColumn();
            colModifier = new DataGridViewTextBoxColumn();
            colModifyDate = new DataGridViewTextBoxColumn();
            pnlPager = new Panel();
            lblPageInfo = new Label();
            btnLastPage = new Button();
            btnNextPage = new Button();
            btnPrevPage = new Button();
            btnFirstPage = new Button();
            pnlSearch.SuspendLayout();
            pnlToolbar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvDepts).BeginInit();
            pnlPager.SuspendLayout();
            SuspendLayout();
            // 
            // pnlSearch
            // 
            pnlSearch.BackColor = Color.FromArgb(250, 250, 250);
            pnlSearch.Controls.Add(btnClear);
            pnlSearch.Controls.Add(btnSearch);
            pnlSearch.Controls.Add(txtSearchCode);
            pnlSearch.Controls.Add(label2);
            pnlSearch.Controls.Add(txtSearchName);
            pnlSearch.Controls.Add(label1);
            pnlSearch.Dock = DockStyle.Top;
            pnlSearch.Location = new Point(10, 10);
            pnlSearch.Margin = new Padding(4);
            pnlSearch.Name = "pnlSearch";
            pnlSearch.Size = new Size(1473, 113);
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
            // txtSearchCode
            // 
            txtSearchCode.BorderStyle = BorderStyle.FixedSingle;
            txtSearchCode.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            txtSearchCode.Location = new Point(373, 42);
            txtSearchCode.Margin = new Padding(4);
            txtSearchCode.Name = "txtSearchCode";
            txtSearchCode.Size = new Size(174, 23);
            txtSearchCode.TabIndex = 3;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            label2.Location = new Point(327, 48);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(68, 17);
            label2.TabIndex = 2;
            label2.Text = "部门编码：";
            // 
            // txtSearchName
            // 
            txtSearchName.BorderStyle = BorderStyle.FixedSingle;
            txtSearchName.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            txtSearchName.Location = new Point(128, 42);
            txtSearchName.Margin = new Padding(4);
            txtSearchName.Name = "txtSearchName";
            txtSearchName.Size = new Size(175, 23);
            txtSearchName.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            label1.Location = new Point(23, 47);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(68, 17);
            label1.TabIndex = 0;
            label1.Text = "部门名称：";
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
            pnlToolbar.Location = new Point(10, 123);
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
            // dgvDepts
            // 
            dgvDepts.AllowUserToAddRows = false;
            dgvDepts.AllowUserToDeleteRows = false;
            dgvDepts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvDepts.BackgroundColor = Color.White;
            dgvDepts.BorderStyle = BorderStyle.None;
            dgvDepts.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvDepts.Columns.AddRange(new DataGridViewColumn[] { colDeptId, colDeptName, colDeptCode, colCreator, colCreateDate, colModifier, colModifyDate });
            dgvDepts.Dock = DockStyle.Fill;
            dgvDepts.Location = new Point(10, 194);
            dgvDepts.Margin = new Padding(4);
            dgvDepts.MultiSelect = false;
            dgvDepts.Name = "dgvDepts";
            dgvDepts.ReadOnly = true;
            dgvDepts.RowHeadersVisible = false;
            dgvDepts.RowTemplate.Height = 30;
            dgvDepts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDepts.Size = new Size(1473, 717);
            dgvDepts.TabIndex = 3;
            // 
            // colDeptId
            // 
            colDeptId.DataPropertyName = "Dept_Id";
            colDeptId.HeaderText = "ID";
            colDeptId.Name = "colDeptId";
            colDeptId.ReadOnly = true;
            colDeptId.Visible = false;
            // 
            // colDeptName
            // 
            colDeptName.DataPropertyName = "DeptName";
            colDeptName.HeaderText = "部门名称";
            colDeptName.Name = "colDeptName";
            colDeptName.ReadOnly = true;
            // 
            // colDeptCode
            // 
            colDeptCode.DataPropertyName = "DeptCode";
            colDeptCode.HeaderText = "部门编码";
            colDeptCode.Name = "colDeptCode";
            colDeptCode.ReadOnly = true;
            // 
            // colCreator
            // 
            colCreator.DataPropertyName = "Creator";
            colCreator.HeaderText = "创建人";
            colCreator.Name = "colCreator";
            colCreator.ReadOnly = true;
            // 
            // colCreateDate
            // 
            colCreateDate.DataPropertyName = "CreateDate";
            colCreateDate.HeaderText = "创建时间";
            colCreateDate.Name = "colCreateDate";
            colCreateDate.ReadOnly = true;
            // 
            // colModifier
            // 
            colModifier.DataPropertyName = "Modifier";
            colModifier.HeaderText = "修改人";
            colModifier.Name = "colModifier";
            colModifier.ReadOnly = true;
            // 
            // colModifyDate
            // 
            colModifyDate.DataPropertyName = "ModifyDate";
            colModifyDate.HeaderText = "修改时间";
            colModifyDate.Name = "colModifyDate";
            colModifyDate.ReadOnly = true;
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
            // DeptManagementForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1493, 992);
            Controls.Add(dgvDepts);
            Controls.Add(pnlPager);
            Controls.Add(pnlToolbar);
            Controls.Add(pnlSearch);
            Margin = new Padding(4);
            Name = "DeptManagementForm";
            Padding = new Padding(10);
            StartPosition = FormStartPosition.CenterScreen;
            Text = "部门管理";
            Load += DeptManagementForm_Load;
            pnlSearch.ResumeLayout(false);
            pnlSearch.PerformLayout();
            pnlToolbar.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvDepts).EndInit();
            pnlPager.ResumeLayout(false);
            pnlPager.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.Panel pnlSearch;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtSearchCode;
        private System.Windows.Forms.Label label2;

        private System.Windows.Forms.TextBox txtSearchName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pnlToolbar;
        private System.Windows.Forms.Button btnExportPDF;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.DataGridView dgvDepts;
        private System.Windows.Forms.Panel pnlPager;
        private System.Windows.Forms.Label lblPageInfo;
        private System.Windows.Forms.Button btnLastPage;
        private System.Windows.Forms.Button btnNextPage;
        private System.Windows.Forms.Button btnPrevPage;
        private System.Windows.Forms.Button btnFirstPage;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDeptId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDeptName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDeptCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCreator;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCreateDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colModifier;
        private System.Windows.Forms.DataGridViewTextBoxColumn colModifyDate;
    }
}