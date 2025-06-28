using System;
using System.Drawing;
using System.Windows.Forms;

namespace GC_MES.WinForm.Forms.SystemForm;

partial class UserManagementForm
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
        this.pnlTop = new System.Windows.Forms.Panel();
        this.lblTitle = new System.Windows.Forms.Label();
        this.pnlSearch = new System.Windows.Forms.Panel();
        this.btnClear = new System.Windows.Forms.Button();
        this.btnSearch = new System.Windows.Forms.Button();
        this.txtSearchEmail = new System.Windows.Forms.TextBox();
        this.label3 = new System.Windows.Forms.Label();
        this.txtSearchMobile = new System.Windows.Forms.TextBox();
        this.label2 = new System.Windows.Forms.Label();
        this.txtSearchName = new System.Windows.Forms.TextBox();
        this.label1 = new System.Windows.Forms.Label();
        this.pnlToolbar = new System.Windows.Forms.Panel();
        this.btnExportPDF = new System.Windows.Forms.Button();
        this.btnExport = new System.Windows.Forms.Button();
        this.btnImport = new System.Windows.Forms.Button();
        this.btnDelete = new System.Windows.Forms.Button();
        this.btnEdit = new System.Windows.Forms.Button();
        this.btnAdd = new System.Windows.Forms.Button();
        this.dgvUsers = new System.Windows.Forms.DataGridView();
        this.colUserId = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.colUserName = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.colTrueName = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.colRole = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.colDept = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.colMobile = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.colEmail = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.colEnable = new System.Windows.Forms.DataGridViewCheckBoxColumn();
        this.colCreateDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.pnlPager = new System.Windows.Forms.Panel();
        this.lblPageInfo = new System.Windows.Forms.Label();
        this.btnLastPage = new System.Windows.Forms.Button();
        this.btnNextPage = new System.Windows.Forms.Button();
        this.btnPrevPage = new System.Windows.Forms.Button();
        this.btnFirstPage = new System.Windows.Forms.Button();
        this.pnlTop.SuspendLayout();
        this.pnlSearch.SuspendLayout();
        this.pnlToolbar.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).BeginInit();
        this.pnlPager.SuspendLayout();
        this.SuspendLayout();
        // 
        // pnlTop
        // 
        this.pnlTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(63)))), ((int)(((byte)(88)))));
        this.pnlTop.Controls.Add(this.lblTitle);
        this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
        this.pnlTop.Location = new System.Drawing.Point(0, 0);
        this.pnlTop.Name = "pnlTop";
        this.pnlTop.Size = new System.Drawing.Size(1280, 60);
        this.pnlTop.TabIndex = 0;
        // 
        // lblTitle
        // 
        this.lblTitle.AutoSize = true;
        this.lblTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
        this.lblTitle.ForeColor = System.Drawing.Color.White;
        this.lblTitle.Location = new System.Drawing.Point(20, 18);
        this.lblTitle.Name = "lblTitle";
        this.lblTitle.Size = new System.Drawing.Size(88, 26);
        this.lblTitle.TabIndex = 0;
        this.lblTitle.Text = "用户管理";
        // 
        // pnlSearch
        // 
        this.pnlSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
        this.pnlSearch.Controls.Add(this.btnClear);
        this.pnlSearch.Controls.Add(this.btnSearch);
        this.pnlSearch.Controls.Add(this.txtSearchEmail);
        this.pnlSearch.Controls.Add(this.label3);
        this.pnlSearch.Controls.Add(this.txtSearchMobile);
        this.pnlSearch.Controls.Add(this.label2);
        this.pnlSearch.Controls.Add(this.txtSearchName);
        this.pnlSearch.Controls.Add(this.label1);
        this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Top;
        this.pnlSearch.Location = new System.Drawing.Point(0, 60);
        this.pnlSearch.Name = "pnlSearch";
        this.pnlSearch.Size = new System.Drawing.Size(1280, 80);
        this.pnlSearch.TabIndex = 1;
        // 
        // btnClear
        // 
        this.btnClear.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
        this.btnClear.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
        this.btnClear.ForeColor = System.Drawing.Color.Black;
        this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        this.btnClear.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
        this.btnClear.Location = new System.Drawing.Point(800, 30);
        this.btnClear.Name = "btnClear";
        this.btnClear.Size = new System.Drawing.Size(75, 28);
        this.btnClear.TabIndex = 7;
        this.btnClear.Text = "清空";
        this.btnClear.UseVisualStyleBackColor = false;
        // 
        // btnSearch
        // 
        this.btnSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(63)))), ((int)(((byte)(88)))));
        this.btnSearch.FlatAppearance.BorderSize = 0;
        this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        this.btnSearch.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
        this.btnSearch.ForeColor = System.Drawing.Color.White;
        this.btnSearch.Location = new System.Drawing.Point(710, 30);
        this.btnSearch.Name = "btnSearch";
        this.btnSearch.Size = new System.Drawing.Size(75, 28);
        this.btnSearch.TabIndex = 6;
        this.btnSearch.Text = "搜索";
        this.btnSearch.UseVisualStyleBackColor = false;
        // 
        // txtSearchEmail
        // 
        this.txtSearchEmail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.txtSearchEmail.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
        this.txtSearchEmail.Location = new System.Drawing.Point(530, 30);
        this.txtSearchEmail.Name = "txtSearchEmail";
        this.txtSearchEmail.Size = new System.Drawing.Size(150, 27);
        this.txtSearchEmail.TabIndex = 5;
        // 
        // label3
        // 
        this.label3.AutoSize = true;
        this.label3.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
        this.label3.Location = new System.Drawing.Point(490, 33);
        this.label3.Name = "label3";
        this.label3.Size = new System.Drawing.Size(43, 17);
        this.label3.TabIndex = 4;
        this.label3.Text = "邮箱：";
        // 
        // txtSearchMobile
        // 
        this.txtSearchMobile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.txtSearchMobile.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
        this.txtSearchMobile.Location = new System.Drawing.Point(320, 30);
        this.txtSearchMobile.Name = "txtSearchMobile";
        this.txtSearchMobile.Size = new System.Drawing.Size(150, 27);
        this.txtSearchMobile.TabIndex = 3;
        // 
        // label2
        // 
        this.label2.AutoSize = true;
        this.label2.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
        this.label2.Location = new System.Drawing.Point(280, 33);
        this.label2.Name = "label2";
        this.label2.Size = new System.Drawing.Size(43, 17);
        this.label2.TabIndex = 2;
        this.label2.Text = "电话：";
        // 
        // txtSearchName
        // 
        this.txtSearchName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.txtSearchName.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
        this.txtSearchName.Location = new System.Drawing.Point(110, 30);
        this.txtSearchName.Name = "txtSearchName";
        this.txtSearchName.Size = new System.Drawing.Size(150, 27);
        this.txtSearchName.TabIndex = 1;
        // 
        // label1
        // 
        this.label1.AutoSize = true;
        this.label1.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
        this.label1.Location = new System.Drawing.Point(20, 33);
        this.label1.Name = "label1";
        this.label1.Size = new System.Drawing.Size(91, 17);
        this.label1.TabIndex = 0;
        this.label1.Text = "用户名/姓名：";
        // 
        // pnlToolbar
        // 
        this.pnlToolbar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
        this.pnlToolbar.Controls.Add(this.btnExportPDF);
        this.pnlToolbar.Controls.Add(this.btnExport);
        this.pnlToolbar.Controls.Add(this.btnImport);
        this.pnlToolbar.Controls.Add(this.btnDelete);
        this.pnlToolbar.Controls.Add(this.btnEdit);
        this.pnlToolbar.Controls.Add(this.btnAdd);
        this.pnlToolbar.Dock = System.Windows.Forms.DockStyle.Top;
        this.pnlToolbar.Location = new System.Drawing.Point(0, 140);
        this.pnlToolbar.Name = "pnlToolbar";
        this.pnlToolbar.Size = new System.Drawing.Size(1280, 50);
        this.pnlToolbar.TabIndex = 2;
        // 
        // btnExportPDF
        // 
        this.btnExportPDF.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(105)))), ((int)(((byte)(137)))));
        this.btnExportPDF.FlatAppearance.BorderSize = 0;
        this.btnExportPDF.FlatAppearance.BorderSize = 0;
        this.btnExportPDF.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        this.btnExportPDF.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
        this.btnExportPDF.ForeColor = System.Drawing.Color.White;
        this.btnExportPDF.Location = new System.Drawing.Point(510, 12);
        this.btnExportPDF.Name = "btnExportPDF";
        this.btnExportPDF.Size = new System.Drawing.Size(90, 28);
        this.btnExportPDF.TabIndex = 5;
        this.btnExportPDF.Text = "导出PDF";
        this.btnExportPDF.UseVisualStyleBackColor = false;
        // 
        // btnExport
        // 
        this.btnExport.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(105)))), ((int)(((byte)(137)))));
        this.btnExport.FlatAppearance.BorderSize = 0;
        this.btnExport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        this.btnExport.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
        this.btnExport.ForeColor = System.Drawing.Color.White;
        this.btnExport.Location = new System.Drawing.Point(410, 12);
        this.btnExport.Name = "btnExport";
        this.btnExport.Size = new System.Drawing.Size(90, 28);
        this.btnExport.TabIndex = 4;
        this.btnExport.Text = "导出Excel";
        this.btnExport.UseVisualStyleBackColor = false;
        // 
        // btnImport
        // 
        this.btnImport.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(105)))), ((int)(((byte)(137)))));
        this.btnImport.FlatAppearance.BorderSize = 0;
        this.btnImport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        this.btnImport.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
        this.btnImport.ForeColor = System.Drawing.Color.White;
        this.btnImport.Location = new System.Drawing.Point(310, 12);
        this.btnImport.Name = "btnImport";
        this.btnImport.Size = new System.Drawing.Size(90, 28);
        this.btnImport.TabIndex = 3;
        this.btnImport.Text = "导入Excel";
        this.btnImport.UseVisualStyleBackColor = false;
        // 
        // btnDelete
        // 
        this.btnDelete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(159)))), ((int)(((byte)(68)))), ((int)(((byte)(74)))));
        this.btnDelete.FlatAppearance.BorderSize = 0;
        this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        this.btnDelete.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
        this.btnDelete.ForeColor = System.Drawing.Color.White;
        this.btnDelete.Location = new System.Drawing.Point(210, 12);
        this.btnDelete.Name = "btnDelete";
        this.btnDelete.Size = new System.Drawing.Size(90, 28);
        this.btnDelete.TabIndex = 2;
        this.btnDelete.Text = "删除";
        this.btnDelete.UseVisualStyleBackColor = false;
        // 
        // btnEdit
        // 
        this.btnEdit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(92)))), ((int)(((byte)(125))))); this.btnEdit.FlatAppearance.BorderSize = 0;
        this.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        this.btnEdit.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
        this.btnEdit.ForeColor = System.Drawing.Color.White;
        this.btnEdit.Location = new System.Drawing.Point(110, 12);
        this.btnEdit.Name = "btnEdit";
        this.btnEdit.Size = new System.Drawing.Size(90, 28);
        this.btnEdit.TabIndex = 1;
        this.btnEdit.Text = "编辑";
        this.btnEdit.UseVisualStyleBackColor = false;
        // 
        // btnAdd
        // 
        this.btnAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(63)))), ((int)(((byte)(88))))); this.btnAdd.FlatAppearance.BorderSize = 0;
        this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        this.btnAdd.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
        this.btnAdd.ForeColor = System.Drawing.Color.White;
        this.btnAdd.Location = new System.Drawing.Point(10, 12);
        this.btnAdd.Name = "btnAdd";
        this.btnAdd.Size = new System.Drawing.Size(90, 28);
        this.btnAdd.TabIndex = 0;
        this.btnAdd.Text = "新增";
        this.btnAdd.UseVisualStyleBackColor = false;
        // 
        // dgvUsers
        // 
        this.dgvUsers.AllowUserToAddRows = false;
        this.dgvUsers.AllowUserToDeleteRows = false;
        this.dgvUsers.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
        this.dgvUsers.BackgroundColor = System.Drawing.Color.White;
        this.dgvUsers.BorderStyle = System.Windows.Forms.BorderStyle.None;
        this.dgvUsers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        this.dgvUsers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
        this.colUserId,
        this.colUserName,
        this.colTrueName,
        this.colRole,
        this.colDept,
        this.colMobile,
        this.colEmail,
        this.colEnable,
        this.colCreateDate});
        this.dgvUsers.Dock = System.Windows.Forms.DockStyle.Fill;
        this.dgvUsers.Location = new System.Drawing.Point(0, 190);
        this.dgvUsers.MultiSelect = false;
        this.dgvUsers.Name = "dgvUsers";
        this.dgvUsers.ReadOnly = true;
        this.dgvUsers.RowHeadersVisible = false;
        this.dgvUsers.RowTemplate.Height = 30;
        this.dgvUsers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
        this.dgvUsers.Size = new System.Drawing.Size(1280, 460);
        this.dgvUsers.TabIndex = 3;
        // 
        // colUserId
        // 
        this.colUserId.DataPropertyName = "User_Id";
        this.colUserId.HeaderText = "ID";
        this.colUserId.Name = "colUserId";
        this.colUserId.ReadOnly = true;
        this.colUserId.Visible = false;
        // 
        // colUserName
        // 
        this.colUserName.DataPropertyName = "UserName";
        this.colUserName.HeaderText = "用户名";
        this.colUserName.Name = "colUserName";
        this.colUserName.ReadOnly = true;
        // 
        // colTrueName
        // 
        this.colTrueName.DataPropertyName = "UserTrueName";
        this.colTrueName.HeaderText = "姓名";
        this.colTrueName.Name = "colTrueName";
        this.colTrueName.ReadOnly = true;
        // 
        // colRole
        // 
        this.colRole.DataPropertyName = "RoleName";
        this.colRole.HeaderText = "角色";
        this.colRole.Name = "colRole";
        this.colRole.ReadOnly = true;
        // 
        // colDept
        // 
        this.colDept.DataPropertyName = "DeptName";
        this.colDept.HeaderText = "部门";
        this.colDept.Name = "colDept";
        this.colDept.ReadOnly = true;
        // 
        // colMobile
        // 
        this.colMobile.DataPropertyName = "Mobile";
        this.colMobile.HeaderText = "电话";
        this.colMobile.Name = "colMobile";
        this.colMobile.ReadOnly = true;
        // 
        // colEmail
        // 
        this.colEmail.DataPropertyName = "Email";
        this.colEmail.HeaderText = "邮箱";
        this.colEmail.Name = "colEmail";
        this.colEmail.ReadOnly = true;
        // 
        // colEnable
        // 
        this.colEnable.DataPropertyName = "Enable";
        this.colEnable.HeaderText = "启用";
        this.colEnable.Name = "colEnable";
        this.colEnable.ReadOnly = true;
        // 
        // colCreateDate
        // 
        this.colCreateDate.DataPropertyName = "CreateDate";
        this.colCreateDate.HeaderText = "创建时间";
        this.colCreateDate.Name = "colCreateDate";
        this.colCreateDate.ReadOnly = true;
        // 
        // pnlPager
        // 
        this.pnlPager.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
        this.pnlPager.Controls.Add(this.lblPageInfo);
        this.pnlPager.Controls.Add(this.btnLastPage);
        this.pnlPager.Controls.Add(this.btnNextPage);
        this.pnlPager.Controls.Add(this.btnPrevPage);
        this.pnlPager.Controls.Add(this.btnFirstPage);
        this.pnlPager.Dock = System.Windows.Forms.DockStyle.Bottom;
        this.pnlPager.Location = new System.Drawing.Point(0, 650);
        this.pnlPager.Name = "pnlPager";
        this.pnlPager.Size = new System.Drawing.Size(1280, 50);
        this.pnlPager.TabIndex = 4;
        // 
        // lblPageInfo
        // 
        this.lblPageInfo.AutoSize = true;
        this.lblPageInfo.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
        this.lblPageInfo.Location = new System.Drawing.Point(200, 17);
        this.lblPageInfo.Name = "lblPageInfo";
        this.lblPageInfo.Size = new System.Drawing.Size(118, 17);
        this.lblPageInfo.TabIndex = 4;
        this.lblPageInfo.Text = "第 1/1 页，共 0 条";
        // 
        // btnLastPage
        // 
        this.btnLastPage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
        this.btnLastPage.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(210)))), ((int)(((byte)(210)))));
        this.btnLastPage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        this.btnLastPage.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
        this.btnLastPage.Location = new System.Drawing.Point(157, 12);
        this.btnLastPage.Name = "btnLastPage";
        this.btnLastPage.Size = new System.Drawing.Size(35, 28);
        this.btnLastPage.TabIndex = 3;
        this.btnLastPage.Text = ">|";
        this.btnLastPage.UseVisualStyleBackColor = false;
        // 
        // btnNextPage
        // 
        this.btnNextPage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
        this.btnNextPage.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(210)))), ((int)(((byte)(210)))));
        this.btnNextPage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        this.btnNextPage.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
        this.btnNextPage.Location = new System.Drawing.Point(108, 12);
        this.btnNextPage.Name = "btnNextPage";
        this.btnNextPage.Size = new System.Drawing.Size(35, 28);
        this.btnNextPage.TabIndex = 2;
        this.btnNextPage.Text = ">";
        this.btnNextPage.UseVisualStyleBackColor = false;
        // 
        // btnPrevPage
        // 
        this.btnPrevPage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
        this.btnPrevPage.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(210)))), ((int)(((byte)(210)))));
        this.btnPrevPage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        this.btnPrevPage.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
        this.btnPrevPage.Location = new System.Drawing.Point(59, 12);
        this.btnPrevPage.Name = "btnPrevPage";
        this.btnPrevPage.Size = new System.Drawing.Size(35, 28);
        this.btnPrevPage.TabIndex = 1;
        this.btnPrevPage.Text = "<";
        this.btnPrevPage.UseVisualStyleBackColor = false;
        // 
        // btnFirstPage
        // 
        this.btnFirstPage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
        this.btnFirstPage.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(210)))), ((int)(((byte)(210)))));
        this.btnFirstPage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        this.btnFirstPage.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
        this.btnFirstPage.Location = new System.Drawing.Point(10, 12);
        this.btnFirstPage.Name = "btnFirstPage";
        this.btnFirstPage.Size = new System.Drawing.Size(35, 28);
        this.btnFirstPage.TabIndex = 0;
        this.btnFirstPage.Text = "|<";
        this.btnFirstPage.UseVisualStyleBackColor = false;
        // 
        // UserManagementForm
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(1280, 700);
        this.Controls.Add(this.dgvUsers);
        this.Controls.Add(this.pnlPager);
        this.Controls.Add(this.pnlToolbar);
        this.Controls.Add(this.pnlSearch);
        this.Controls.Add(this.pnlTop);
        this.Name = "UserManagementForm";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        this.Text = "用户管理";
        this.pnlTop.ResumeLayout(false);
        this.pnlTop.PerformLayout();
        this.pnlSearch.ResumeLayout(false);
        this.pnlSearch.PerformLayout();
        this.pnlToolbar.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).EndInit();
        this.pnlPager.ResumeLayout(false);
        this.pnlPager.PerformLayout();
        this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Panel pnlTop;
    private System.Windows.Forms.Label lblTitle;
    private System.Windows.Forms.Panel pnlSearch;
    private System.Windows.Forms.Button btnClear;
    private System.Windows.Forms.Button btnSearch;
    private System.Windows.Forms.TextBox txtSearchEmail;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.TextBox txtSearchMobile;
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
    private System.Windows.Forms.DataGridView dgvUsers;
    private System.Windows.Forms.Panel pnlPager;
    private System.Windows.Forms.Label lblPageInfo;
    private System.Windows.Forms.Button btnLastPage;
    private System.Windows.Forms.Button btnNextPage;
    private System.Windows.Forms.Button btnPrevPage;
    private System.Windows.Forms.Button btnFirstPage;
    private System.Windows.Forms.DataGridViewTextBoxColumn colUserId;
    private System.Windows.Forms.DataGridViewTextBoxColumn colUserName;
    private System.Windows.Forms.DataGridViewTextBoxColumn colTrueName;
    private System.Windows.Forms.DataGridViewTextBoxColumn colRole;
    private System.Windows.Forms.DataGridViewTextBoxColumn colDept;
    private System.Windows.Forms.DataGridViewTextBoxColumn colMobile;
    private System.Windows.Forms.DataGridViewTextBoxColumn colEmail;
    private System.Windows.Forms.DataGridViewCheckBoxColumn colEnable;
    private System.Windows.Forms.DataGridViewTextBoxColumn colCreateDate;
}