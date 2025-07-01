using System;
using System.Windows.Forms;

namespace GC_MES.WinForm.Forms.SystemForm
{
    partial class CodeGeneratorForm
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
            splitContainer1 = new SplitContainer();
            lstEntities = new ListBox();
            lblEntities = new Label();
            pnlRight = new Panel();
            dgvProperties = new DataGridView();
            lblProperties = new Label();
            pnlEntityInfo = new Panel();
            txtTableName = new TextBox();
            lblTableName = new Label();
            txtNamespace = new TextBox();
            lblNamespace = new Label();
            txtEntityName = new TextBox();
            lblEntityName = new Label();
            pnlTop = new Panel();
            txtOutputNamespace = new TextBox();
            lblOutputNamespace = new Label();
            btnGenerateAll = new Button();
            btnGenerateEditForm = new Button();
            btnGenerateForm = new Button();
            btnGenerateIService = new Button();
            btnGenerateService = new Button();
            btnGenerateIRepository = new Button();
            btnGenerateRepository = new Button();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            pnlRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvProperties).BeginInit();
            pnlEntityInfo.SuspendLayout();
            pnlTop.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 68);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(lstEntities);
            splitContainer1.Panel1.Controls.Add(lblEntities);
            splitContainer1.Panel1MinSize = 200;
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(pnlRight);
            splitContainer1.Panel2.Controls.Add(pnlEntityInfo);
            splitContainer1.Size = new Size(1109, 649);
            splitContainer1.SplitterDistance = 277;
            splitContainer1.TabIndex = 0;
            // 
            // lstEntities
            // 
            lstEntities.Dock = DockStyle.Fill;
            lstEntities.FormattingEnabled = true;
            lstEntities.ItemHeight = 17;
            lstEntities.Location = new Point(0, 28);
            lstEntities.Name = "lstEntities";
            lstEntities.Size = new Size(277, 621);
            lstEntities.TabIndex = 1;
            lstEntities.SelectedIndexChanged += lstEntities_SelectedIndexChanged;
            // 
            // lblEntities
            // 
            lblEntities.Dock = DockStyle.Top;
            lblEntities.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 134);
            lblEntities.Location = new Point(0, 0);
            lblEntities.Name = "lblEntities";
            lblEntities.Size = new Size(277, 28);
            lblEntities.TabIndex = 0;
            lblEntities.Text = "实体类列表";
            lblEntities.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // pnlRight
            // 
            pnlRight.Controls.Add(dgvProperties);
            pnlRight.Controls.Add(lblProperties);
            pnlRight.Dock = DockStyle.Fill;
            pnlRight.Location = new Point(0, 136);
            pnlRight.Name = "pnlRight";
            pnlRight.Size = new Size(828, 513);
            pnlRight.TabIndex = 1;
            // 
            // dgvProperties
            // 
            dgvProperties.AllowUserToAddRows = false;
            dgvProperties.AllowUserToDeleteRows = false;
            dgvProperties.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvProperties.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvProperties.Dock = DockStyle.Fill;
            dgvProperties.Location = new Point(0, 28);
            dgvProperties.Name = "dgvProperties";
            dgvProperties.RowHeadersWidth = 51;
            dgvProperties.RowTemplate.Height = 27;
            dgvProperties.Size = new Size(828, 485);
            dgvProperties.TabIndex = 1;
            // 
            // lblProperties
            // 
            lblProperties.Dock = DockStyle.Top;
            lblProperties.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 134);
            lblProperties.Location = new Point(0, 0);
            lblProperties.Name = "lblProperties";
            lblProperties.Size = new Size(828, 28);
            lblProperties.TabIndex = 0;
            lblProperties.Text = "属性列表";
            lblProperties.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // pnlEntityInfo
            // 
            pnlEntityInfo.Controls.Add(txtTableName);
            pnlEntityInfo.Controls.Add(lblTableName);
            pnlEntityInfo.Controls.Add(txtNamespace);
            pnlEntityInfo.Controls.Add(lblNamespace);
            pnlEntityInfo.Controls.Add(txtEntityName);
            pnlEntityInfo.Controls.Add(lblEntityName);
            pnlEntityInfo.Dock = DockStyle.Top;
            pnlEntityInfo.Location = new Point(0, 0);
            pnlEntityInfo.Name = "pnlEntityInfo";
            pnlEntityInfo.Size = new Size(828, 136);
            pnlEntityInfo.TabIndex = 0;
            // 
            // txtTableName
            // 
            txtTableName.Location = new Point(105, 91);
            txtTableName.Name = "txtTableName";
            txtTableName.Size = new Size(263, 23);
            txtTableName.TabIndex = 5;
            // 
            // lblTableName
            // 
            lblTableName.AutoSize = true;
            lblTableName.Location = new Point(18, 94);
            lblTableName.Name = "lblTableName";
            lblTableName.Size = new Size(44, 17);
            lblTableName.TabIndex = 4;
            lblTableName.Text = "表名：";
            // 
            // txtNamespace
            // 
            txtNamespace.Location = new Point(105, 51);
            txtNamespace.Name = "txtNamespace";
            txtNamespace.Size = new Size(263, 23);
            txtNamespace.TabIndex = 3;
            // 
            // lblNamespace
            // 
            lblNamespace.AutoSize = true;
            lblNamespace.Location = new Point(18, 54);
            lblNamespace.Name = "lblNamespace";
            lblNamespace.Size = new Size(68, 17);
            lblNamespace.TabIndex = 2;
            lblNamespace.Text = "命名空间：";
            // 
            // txtEntityName
            // 
            txtEntityName.Location = new Point(105, 11);
            txtEntityName.Name = "txtEntityName";
            txtEntityName.Size = new Size(263, 23);
            txtEntityName.TabIndex = 1;
            // 
            // lblEntityName
            // 
            lblEntityName.AutoSize = true;
            lblEntityName.Location = new Point(18, 15);
            lblEntityName.Name = "lblEntityName";
            lblEntityName.Size = new Size(68, 17);
            lblEntityName.TabIndex = 0;
            lblEntityName.Text = "实体名称：";
            // 
            // pnlTop
            // 
            pnlTop.Controls.Add(txtOutputNamespace);
            pnlTop.Controls.Add(lblOutputNamespace);
            pnlTop.Controls.Add(btnGenerateAll);
            pnlTop.Controls.Add(btnGenerateEditForm);
            pnlTop.Controls.Add(btnGenerateForm);
            pnlTop.Controls.Add(btnGenerateIService);
            pnlTop.Controls.Add(btnGenerateService);
            pnlTop.Controls.Add(btnGenerateIRepository);
            pnlTop.Controls.Add(btnGenerateRepository);
            pnlTop.Dock = DockStyle.Top;
            pnlTop.Location = new Point(0, 0);
            pnlTop.Name = "pnlTop";
            pnlTop.Size = new Size(1109, 68);
            pnlTop.TabIndex = 1;
            // 
            // txtOutputNamespace
            // 
            txtOutputNamespace.Location = new Point(88, 20);
            txtOutputNamespace.Name = "txtOutputNamespace";
            txtOutputNamespace.Size = new Size(158, 23);
            txtOutputNamespace.TabIndex = 8;
            txtOutputNamespace.Text = "GC_MES";
            // 
            // lblOutputNamespace
            // 
            lblOutputNamespace.AutoSize = true;
            lblOutputNamespace.Location = new Point(10, 24);
            lblOutputNamespace.Name = "lblOutputNamespace";
            lblOutputNamespace.Size = new Size(83, 17);
            lblOutputNamespace.TabIndex = 7;
            lblOutputNamespace.Text = "输出命名空间:";
            // 
            // btnGenerateAll
            // 
            btnGenerateAll.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnGenerateAll.BackColor = Color.FromArgb(80, 80, 85);
            btnGenerateAll.FlatAppearance.BorderSize = 0;
            btnGenerateAll.FlatStyle = FlatStyle.Flat;
            btnGenerateAll.Location = new Point(1021, 12);
            btnGenerateAll.Name = "btnGenerateAll";
            btnGenerateAll.Size = new Size(76, 39);
            btnGenerateAll.TabIndex = 6;
            btnGenerateAll.Text = "生成全部";
            btnGenerateAll.UseVisualStyleBackColor = false;
            btnGenerateAll.Click += btnGenerateAll_Click;
            // 
            // btnGenerateEditForm
            // 
            btnGenerateEditForm.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnGenerateEditForm.BackColor = Color.FromArgb(80, 80, 85);
            btnGenerateEditForm.FlatAppearance.BorderSize = 0;
            btnGenerateEditForm.FlatStyle = FlatStyle.Flat;
            btnGenerateEditForm.Location = new Point(933, 12);
            btnGenerateEditForm.Name = "btnGenerateEditForm";
            btnGenerateEditForm.Size = new Size(76, 39);
            btnGenerateEditForm.TabIndex = 5;
            btnGenerateEditForm.Text = "编辑窗体";
            btnGenerateEditForm.UseVisualStyleBackColor = false;
            btnGenerateEditForm.Click += btnGenerateEditForm_Click;
            // 
            // btnGenerateForm
            // 
            btnGenerateForm.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnGenerateForm.BackColor = Color.FromArgb(80, 80, 85);
            btnGenerateForm.FlatAppearance.BorderSize = 0;
            btnGenerateForm.FlatStyle = FlatStyle.Flat;
            btnGenerateForm.Location = new Point(845, 12);
            btnGenerateForm.Name = "btnGenerateForm";
            btnGenerateForm.Size = new Size(76, 39);
            btnGenerateForm.TabIndex = 4;
            btnGenerateForm.Text = "管理窗体";
            btnGenerateForm.UseVisualStyleBackColor = false;
            btnGenerateForm.Click += btnGenerateForm_Click;
            // 
            // btnGenerateIService
            // 
            btnGenerateIService.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnGenerateIService.BackColor = Color.FromArgb(80, 80, 85);
            btnGenerateIService.FlatAppearance.BorderSize = 0;
            btnGenerateIService.FlatStyle = FlatStyle.Flat;
            btnGenerateIService.Location = new Point(758, 12);
            btnGenerateIService.Name = "btnGenerateIService";
            btnGenerateIService.Size = new Size(76, 39);
            btnGenerateIService.TabIndex = 3;
            btnGenerateIService.Text = "IService";
            btnGenerateIService.UseVisualStyleBackColor = false;
            btnGenerateIService.Click += btnGenerateIService_Click;
            // 
            // btnGenerateService
            // 
            btnGenerateService.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnGenerateService.BackColor = Color.FromArgb(80, 80, 85);
            btnGenerateService.FlatAppearance.BorderSize = 0;
            btnGenerateService.FlatStyle = FlatStyle.Flat;
            btnGenerateService.Location = new Point(671, 12);
            btnGenerateService.Name = "btnGenerateService";
            btnGenerateService.Size = new Size(76, 39);
            btnGenerateService.TabIndex = 2;
            btnGenerateService.Text = "Service";
            btnGenerateService.UseVisualStyleBackColor = false;
            btnGenerateService.Click += btnGenerateService_Click;
            // 
            // btnGenerateIRepository
            // 
            btnGenerateIRepository.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnGenerateIRepository.BackColor = Color.FromArgb(80, 80, 85);
            btnGenerateIRepository.FlatAppearance.BorderSize = 0;
            btnGenerateIRepository.FlatStyle = FlatStyle.Flat;
            btnGenerateIRepository.Location = new Point(583, 12);
            btnGenerateIRepository.Name = "btnGenerateIRepository";
            btnGenerateIRepository.Size = new Size(76, 39);
            btnGenerateIRepository.TabIndex = 1;
            btnGenerateIRepository.Text = "IRepository";
            btnGenerateIRepository.UseVisualStyleBackColor = false;
            btnGenerateIRepository.Click += btnGenerateIRepository_Click;
            // 
            // btnGenerateRepository
            // 
            btnGenerateRepository.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnGenerateRepository.BackColor = Color.FromArgb(80, 80, 85);
            btnGenerateRepository.FlatAppearance.BorderSize = 0;
            btnGenerateRepository.FlatStyle = FlatStyle.Flat;
            btnGenerateRepository.Location = new Point(495, 12);
            btnGenerateRepository.Name = "btnGenerateRepository";
            btnGenerateRepository.Size = new Size(76, 39);
            btnGenerateRepository.TabIndex = 0;
            btnGenerateRepository.Text = "Repository";
            btnGenerateRepository.UseVisualStyleBackColor = false;
            btnGenerateRepository.Click += btnGenerateRepository_Click;
            // 
            // CodeGeneratorForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1109, 717);
            Controls.Add(splitContainer1);
            Controls.Add(pnlTop);
            Name = "CodeGeneratorForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "代码生成器";
            Load += CodeGeneratorForm_Load;
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            pnlRight.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvProperties).EndInit();
            pnlEntityInfo.ResumeLayout(false);
            pnlEntityInfo.PerformLayout();
            pnlTop.ResumeLayout(false);
            pnlTop.PerformLayout();
            ResumeLayout(false);

        }

      

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Button btnGenerateRepository;
        private System.Windows.Forms.Button btnGenerateIRepository;
        private System.Windows.Forms.Button btnGenerateService;
        private System.Windows.Forms.Button btnGenerateIService;
        private System.Windows.Forms.Button btnGenerateForm;
        private System.Windows.Forms.Button btnGenerateEditForm;
        private System.Windows.Forms.Button btnGenerateAll;
        private System.Windows.Forms.ListBox lstEntities;
        private System.Windows.Forms.Label lblEntities;
        private System.Windows.Forms.Panel pnlRight;
        private System.Windows.Forms.Panel pnlEntityInfo;
        private System.Windows.Forms.TextBox txtEntityName;
        private System.Windows.Forms.Label lblEntityName;
        private System.Windows.Forms.TextBox txtNamespace;
        private System.Windows.Forms.Label lblNamespace;
        private System.Windows.Forms.TextBox txtTableName;
        private System.Windows.Forms.Label lblTableName;
        private System.Windows.Forms.DataGridView dgvProperties;
        private System.Windows.Forms.Label lblProperties;
        private System.Windows.Forms.TextBox txtOutputNamespace;
        private System.Windows.Forms.Label lblOutputNamespace;
    }
}