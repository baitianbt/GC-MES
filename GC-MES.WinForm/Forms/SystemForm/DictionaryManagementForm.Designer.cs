namespace GC_MES.WinForm.Forms.SystemForm
{
    partial class DictionaryManagementForm
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
            btnDelete = new Button();
            btnEdit = new Button();
            btnAdd = new Button();
            splitContainer1 = new SplitContainer();
            dgvDictionaries = new DataGridView();
            colDicId = new DataGridViewTextBoxColumn();
            colDicNo = new DataGridViewTextBoxColumn();
            colDicName = new DataGridViewTextBoxColumn();
            colParentId = new DataGridViewTextBoxColumn();
            colOrderNo = new DataGridViewTextBoxColumn();
            colEnable = new DataGridViewCheckBoxColumn();
            colRemark = new DataGridViewTextBoxColumn();
            pnlPager = new Panel();
            lblPageInfo = new Label();
            btnLastPage = new Button();
            btnNextPage = new Button();
            btnPrevPage = new Button();
            btnFirstPage = new Button();
            pnlItemToolbar = new Panel();
            btnDeleteItem = new Button();
            btnEditItem = new Button();
            btnAddItem = new Button();
            label3 = new Label();
            dgvDictionaryItems = new DataGridView();
            colItemId = new DataGridViewTextBoxColumn();
            colDicId2 = new DataGridViewTextBoxColumn();
            colDicValue = new DataGridViewTextBoxColumn();
            colDicName2 = new DataGridViewTextBoxColumn();
            colOrderNo2 = new DataGridViewTextBoxColumn();
            colEnable2 = new DataGridViewCheckBoxColumn();
            colRemark2 = new DataGridViewTextBoxColumn();
            pnlSearch.SuspendLayout();
            pnlToolbar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvDictionaries).BeginInit();
            pnlPager.SuspendLayout();
            pnlItemToolbar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvDictionaryItems).BeginInit();
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
            pnlSearch.Size = new Size(1473, 80);
            pnlSearch.TabIndex = 1;
            // 
            // btnClear
            // 
            btnClear.BackColor = Color.FromArgb(180, 180, 180);
            btnClear.FlatAppearance.BorderColor = Color.FromArgb(160, 160, 160);
            btnClear.FlatStyle = FlatStyle.Flat;
            btnClear.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            btnClear.Location = new Point(688, 20);
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
            btnSearch.Location = new Point(583, 20);
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
            txtSearchCode.Location = new Point(373, 30);
            txtSearchCode.Margin = new Padding(4);
            txtSearchCode.Name = "txtSearchCode";
            txtSearchCode.Size = new Size(174, 23);
            txtSearchCode.TabIndex = 3;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            label2.Location = new Point(327, 32);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(44, 17);
            label2.TabIndex = 2;
            label2.Text = "编号：";
            // 
            // txtSearchName
            // 
            txtSearchName.BorderStyle = BorderStyle.FixedSingle;
            txtSearchName.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            txtSearchName.Location = new Point(128, 30);
            txtSearchName.Margin = new Padding(4);
            txtSearchName.Name = "txtSearchName";
            txtSearchName.Size = new Size(175, 23);
            txtSearchName.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            label1.Location = new Point(23, 32);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(68, 17);
            label1.TabIndex = 0;
            label1.Text = "字典名称：";
            // 
            // pnlToolbar
            // 
            pnlToolbar.BackColor = Color.FromArgb(240, 240, 240);
            pnlToolbar.Controls.Add(btnDelete);
            pnlToolbar.Controls.Add(btnEdit);
            pnlToolbar.Controls.Add(btnAdd);
            pnlToolbar.Dock = DockStyle.Top;
            pnlToolbar.Location = new Point(10, 90);
            pnlToolbar.Margin = new Padding(4);
            pnlToolbar.Name = "pnlToolbar";
            pnlToolbar.Size = new Size(1473, 60);
            pnlToolbar.TabIndex = 2;
            // 
            // btnDelete
            // 
            btnDelete.BackColor = Color.FromArgb(159, 68, 74);
            btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.FlatStyle = FlatStyle.Flat;
            btnDelete.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            btnDelete.ForeColor = Color.White;
            btnDelete.Location = new Point(245, 10);
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
            btnEdit.Location = new Point(128, 10);
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
            btnAdd.Location = new Point(12, 10);
            btnAdd.Margin = new Padding(4);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(105, 40);
            btnAdd.TabIndex = 0;
            btnAdd.Text = "新增";
            btnAdd.UseVisualStyleBackColor = false;
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(10, 150);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Orientation = Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(dgvDictionaries);
            splitContainer1.Panel1.Controls.Add(pnlPager);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(dgvDictionaryItems);
            splitContainer1.Panel2.Controls.Add(pnlItemToolbar);
            splitContainer1.Size = new Size(1473, 832);
            splitContainer1.SplitterDistance = 400;
            splitContainer1.TabIndex = 3;
            // 
            // dgvDictionaries
            // 
            dgvDictionaries.AllowUserToAddRows = false;
            dgvDictionaries.AllowUserToDeleteRows = false;
            dgvDictionaries.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvDictionaries.BackgroundColor = Color.White;
            dgvDictionaries.BorderStyle = BorderStyle.None;
            dgvDictionaries.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvDictionaries.Columns.AddRange(new DataGridViewColumn[] { colDicId, colDicNo, colDicName, colParentId, colOrderNo, colEnable, colRemark });
            dgvDictionaries.Dock = DockStyle.Fill;
            dgvDictionaries.Location = new Point(0, 0);
            dgvDictionaries.Margin = new Padding(4);
            dgvDictionaries.MultiSelect = false;
            dgvDictionaries.Name = "dgvDictionaries";
            dgvDictionaries.ReadOnly = true;
            dgvDictionaries.RowHeadersVisible = false;
            dgvDictionaries.RowTemplate.Height = 30;
            dgvDictionaries.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDictionaries.Size = new Size(1473, 330);
            dgvDictionaries.TabIndex = 3;
            // 
            // colDicId
            // 
            colDicId.DataPropertyName = "Dic_ID";
            colDicId.HeaderText = "ID";
            colDicId.Name = "colDicId";
            colDicId.ReadOnly = true;
            colDicId.Visible = false;
            // 
            // colDicNo
            // 
            colDicNo.DataPropertyName = "DicNo";
            colDicNo.HeaderText = "字典编号";
            colDicNo.Name = "colDicNo";
            colDicNo.ReadOnly = true;
            // 
            // colDicName
            // 
            colDicName.DataPropertyName = "DicName";
            colDicName.HeaderText = "字典名称";
            colDicName.Name = "colDicName";
            colDicName.ReadOnly = true;
            // 
            // colParentId
            // 
            colParentId.DataPropertyName = "ParentId";
            colParentId.HeaderText = "父级ID";
            colParentId.Name = "colParentId";
            colParentId.ReadOnly = true;
            // 
            // colOrderNo
            // 
            colOrderNo.DataPropertyName = "OrderNo";
            colOrderNo.HeaderText = "排序";
            colOrderNo.Name = "colOrderNo";
            colOrderNo.ReadOnly = true;
            // 
            // colEnable
            // 
            colEnable.DataPropertyName = "Enable";
            colEnable.HeaderText = "启用";
            colEnable.Name = "colEnable";
            colEnable.ReadOnly = true;
            // 
            // colRemark
            // 
            colRemark.DataPropertyName = "Remark";
            colRemark.HeaderText = "备注";
            colRemark.Name = "colRemark";
            colRemark.ReadOnly = true;
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
            pnlPager.Location = new Point(0, 330);
            pnlPager.Margin = new Padding(4);
            pnlPager.Name = "pnlPager";
            pnlPager.Size = new Size(1473, 70);
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
            // pnlItemToolbar
            // 
            pnlItemToolbar.BackColor = Color.FromArgb(240, 240, 240);
            pnlItemToolbar.Controls.Add(btnDeleteItem);
            pnlItemToolbar.Controls.Add(btnEditItem);
            pnlItemToolbar.Controls.Add(btnAddItem);
            pnlItemToolbar.Controls.Add(label3);
            pnlItemToolbar.Dock = DockStyle.Top;
            pnlItemToolbar.Location = new Point(0, 0);
            pnlItemToolbar.Margin = new Padding(4);
            pnlItemToolbar.Name = "pnlItemToolbar";
            pnlItemToolbar.Size = new Size(1473, 60);
            pnlItemToolbar.TabIndex = 3;
            // 
            // btnDeleteItem
            // 
            btnDeleteItem.BackColor = Color.FromArgb(159, 68, 74);
            btnDeleteItem.Enabled = false;
            btnDeleteItem.FlatAppearance.BorderSize = 0;
            btnDeleteItem.FlatStyle = FlatStyle.Flat;
            btnDeleteItem.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            btnDeleteItem.ForeColor = Color.White;
            btnDeleteItem.Location = new Point(245, 10);
            btnDeleteItem.Margin = new Padding(4);
            btnDeleteItem.Name = "btnDeleteItem";
            btnDeleteItem.Size = new Size(105, 40);
            btnDeleteItem.TabIndex = 5;
            btnDeleteItem.Text = "删除";
            btnDeleteItem.UseVisualStyleBackColor = false;
            // 
            // btnEditItem
            // 
            btnEditItem.BackColor = Color.FromArgb(67, 67, 70);
            btnEditItem.Enabled = false;
            btnEditItem.FlatAppearance.BorderSize = 0;
            btnEditItem.FlatStyle = FlatStyle.Flat;
            btnEditItem.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            btnEditItem.ForeColor = Color.White;
            btnEditItem.Location = new Point(128, 10);
            btnEditItem.Margin = new Padding(4);
            btnEditItem.Name = "btnEditItem";
            btnEditItem.Size = new Size(105, 40);
            btnEditItem.TabIndex = 4;
            btnEditItem.Text = "编辑";
            btnEditItem.UseVisualStyleBackColor = false;
            // 
            // btnAddItem
            // 
            btnAddItem.BackColor = Color.FromArgb(45, 45, 48);
            btnAddItem.Enabled = false;
            btnAddItem.FlatAppearance.BorderSize = 0;
            btnAddItem.FlatStyle = FlatStyle.Flat;
            btnAddItem.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            btnAddItem.ForeColor = Color.White;
            btnAddItem.Location = new Point(12, 10);
            btnAddItem.Margin = new Padding(4);
            btnAddItem.Name = "btnAddItem";
            btnAddItem.Size = new Size(105, 40);
            btnAddItem.TabIndex = 3;
            btnAddItem.Text = "新增";
            btnAddItem.UseVisualStyleBackColor = false;
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label3.AutoSize = true;
            label3.Font = new Font("Microsoft YaHei UI", 10.5F, FontStyle.Bold, GraphicsUnit.Point, 134);
            label3.Location = new Point(1350, 20);
            label3.Name = "label3";
            label3.Size = new Size(107, 19);
            label3.TabIndex = 0;
            label3.Text = "字典明细数据：";
            // 
            // dgvDictionaryItems
            // 
            dgvDictionaryItems.AllowUserToAddRows = false;
            dgvDictionaryItems.AllowUserToDeleteRows = false;
            dgvDictionaryItems.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvDictionaryItems.BackgroundColor = Color.White;
            dgvDictionaryItems.BorderStyle = BorderStyle.None;
            dgvDictionaryItems.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvDictionaryItems.Columns.AddRange(new DataGridViewColumn[] { colItemId, colDicId2, colDicValue, colDicName2, colOrderNo2, colEnable2, colRemark2 });
            dgvDictionaryItems.Dock = DockStyle.Fill;
            dgvDictionaryItems.Location = new Point(0, 60);
            dgvDictionaryItems.Margin = new Padding(4);
            dgvDictionaryItems.MultiSelect = false;
            dgvDictionaryItems.Name = "dgvDictionaryItems";
            dgvDictionaryItems.ReadOnly = true;
            dgvDictionaryItems.RowHeadersVisible = false;
            dgvDictionaryItems.RowTemplate.Height = 30;
            dgvDictionaryItems.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDictionaryItems.Size = new Size(1473, 368);
            dgvDictionaryItems.TabIndex = 4;
            // 
            // colItemId
            // 
            colItemId.DataPropertyName = "DicList_ID";
            colItemId.HeaderText = "ID";
            colItemId.Name = "colItemId";
            colItemId.ReadOnly = true;
            colItemId.Visible = false;
            // 
            // colDicId2
            // 
            colDicId2.DataPropertyName = "Dic_ID";
            colDicId2.HeaderText = "字典ID";
            colDicId2.Name = "colDicId2";
            colDicId2.ReadOnly = true;
            colDicId2.Visible = false;
            // 
            // colDicValue
            // 
            colDicValue.DataPropertyName = "DicValue";
            colDicValue.HeaderText = "字典值";
            colDicValue.Name = "colDicValue";
            colDicValue.ReadOnly = true;
            // 
            // colDicName2
            // 
            colDicName2.DataPropertyName = "DicName";
            colDicName2.HeaderText = "字典文本";
            colDicName2.Name = "colDicName2";
            colDicName2.ReadOnly = true;
            // 
            // colOrderNo2
            // 
            colOrderNo2.DataPropertyName = "OrderNo";
            colOrderNo2.HeaderText = "排序";
            colOrderNo2.Name = "colOrderNo2";
            colOrderNo2.ReadOnly = true;
            // 
            // colEnable2
            // 
            colEnable2.DataPropertyName = "Enable";
            colEnable2.HeaderText = "启用";
            colEnable2.Name = "colEnable2";
            colEnable2.ReadOnly = true;
            // 
            // colRemark2
            // 
            colRemark2.DataPropertyName = "Remark";
            colRemark2.HeaderText = "备注";
            colRemark2.Name = "colRemark2";
            colRemark2.ReadOnly = true;
            // 
            // DictionaryManagementForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1493, 992);
            Controls.Add(splitContainer1);
            Controls.Add(pnlToolbar);
            Controls.Add(pnlSearch);
            Margin = new Padding(4);
            Name = "DictionaryManagementForm";
            Padding = new Padding(10);
            StartPosition = FormStartPosition.CenterScreen;
            Text = "数据字典管理";
            Load += DictionaryManagementForm_Load;
            pnlSearch.ResumeLayout(false);
            pnlSearch.PerformLayout();
            pnlToolbar.ResumeLayout(false);
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvDictionaries).EndInit();
            pnlPager.ResumeLayout(false);
            pnlPager.PerformLayout();
            pnlItemToolbar.ResumeLayout(false);
            pnlItemToolbar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvDictionaryItems).EndInit();
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
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView dgvDictionaries;
        private System.Windows.Forms.Panel pnlPager;
        private System.Windows.Forms.Label lblPageInfo;
        private System.Windows.Forms.Button btnLastPage;
        private System.Windows.Forms.Button btnNextPage;
        private System.Windows.Forms.Button btnPrevPage;
        private System.Windows.Forms.Button btnFirstPage;
        private System.Windows.Forms.Panel pnlItemToolbar;
        private System.Windows.Forms.Button btnDeleteItem;
        private System.Windows.Forms.Button btnEditItem;
        private System.Windows.Forms.Button btnAddItem;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridView dgvDictionaryItems;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDicId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDicNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDicName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colParentId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOrderNo;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colEnable;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRemark;
        private System.Windows.Forms.DataGridViewTextBoxColumn colItemId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDicId2;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDicValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDicName2;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOrderNo2;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colEnable2;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRemark2;
    }




}