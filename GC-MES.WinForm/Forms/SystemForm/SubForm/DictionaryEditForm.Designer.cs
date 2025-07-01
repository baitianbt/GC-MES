using System.Windows.Forms;

namespace GC_MES.WinForm.Forms.SystemForm.SubForm
{
    partial class DictionaryEditForm
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
            chkEnable = new CheckBox();
            nudOrderNo = new NumericUpDown();
            label8 = new Label();
            txtRemark = new TextBox();
            label7 = new Label();
            txtDbSql = new TextBox();
            label6 = new Label();
            txtConfig = new TextBox();
            label5 = new Label();
            cmbParent = new ComboBox();
            label3 = new Label();
            txtDicName = new TextBox();
            label2 = new Label();
            txtDicNo = new TextBox();
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
            lblTitle.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 134);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(0, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(684, 50);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "编辑数据字典";
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
            tabPageBasic.Controls.Add(chkEnable);
            tabPageBasic.Controls.Add(nudOrderNo);
            tabPageBasic.Controls.Add(label8);
            tabPageBasic.Controls.Add(txtRemark);
            tabPageBasic.Controls.Add(label7);
            tabPageBasic.Controls.Add(txtDbSql);
            tabPageBasic.Controls.Add(label6);
            tabPageBasic.Controls.Add(txtConfig);
            tabPageBasic.Controls.Add(label5);
            tabPageBasic.Controls.Add(cmbParent);
            tabPageBasic.Controls.Add(label3);
            tabPageBasic.Controls.Add(txtDicName);
            tabPageBasic.Controls.Add(label2);
            tabPageBasic.Controls.Add(txtDicNo);
            tabPageBasic.Controls.Add(label1);
            tabPageBasic.Location = new Point(4, 26);
            tabPageBasic.Name = "tabPageBasic";
            tabPageBasic.Padding = new Padding(3);
            tabPageBasic.Size = new Size(636, 381);
            tabPageBasic.TabIndex = 0;
            tabPageBasic.Text = "基本信息";
            // 
            // chkEnable
            // 
            chkEnable.AutoSize = true;
            chkEnable.Location = new Point(120, 320);
            chkEnable.Name = "chkEnable";
            chkEnable.Size = new Size(75, 21);
            chkEnable.TabIndex = 16;
            chkEnable.Text = "是否启用";
            chkEnable.UseVisualStyleBackColor = true;
            // 
            // nudOrderNo
            // 
            nudOrderNo.Location = new Point(120, 280);
            nudOrderNo.Maximum = new decimal(new int[] { 9999, 0, 0, 0 });
            nudOrderNo.Name = "nudOrderNo";
            nudOrderNo.Size = new Size(150, 23);
            nudOrderNo.TabIndex = 15;
            nudOrderNo.Value = new decimal(new int[] { 100, 0, 0, 0 });
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(20, 282);
            label8.Name = "label8";
            label8.Size = new Size(56, 17);
            label8.TabIndex = 14;
            label8.Text = "排序号：";
            // 
            // txtRemark
            // 
            txtRemark.Location = new Point(120, 240);
            txtRemark.Name = "txtRemark";
            txtRemark.Size = new Size(400, 23);
            txtRemark.TabIndex = 13;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(20, 243);
            label7.Name = "label7";
            label7.Size = new Size(44, 17);
            label7.TabIndex = 12;
            label7.Text = "备注：";
            // 
            // txtDbSql
            // 
            txtDbSql.Location = new Point(120, 160);
            txtDbSql.Multiline = true;
            txtDbSql.Name = "txtDbSql";
            txtDbSql.Size = new Size(400, 60);
            txtDbSql.TabIndex = 11;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(20, 163);
            label6.Name = "label6";
            label6.Size = new Size(68, 17);
            label6.TabIndex = 10;
            label6.Text = "SQL语句：";
            // 
            // txtConfig
            // 
            txtConfig.Location = new Point(120, 120);
            txtConfig.Name = "txtConfig";
            txtConfig.Size = new Size(400, 23);
            txtConfig.TabIndex = 9;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(20, 123);
            label5.Name = "label5";
            label5.Size = new Size(56, 17);
            label5.TabIndex = 8;
            label5.Text = "配置项：";
            // 
            // cmbParent
            // 
            cmbParent.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbParent.FormattingEnabled = true;
            cmbParent.Location = new Point(120, 80);
            cmbParent.Name = "cmbParent";
            cmbParent.Size = new Size(400, 25);
            cmbParent.TabIndex = 5;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(20, 83);
            label3.Name = "label3";
            label3.Size = new Size(68, 17);
            label3.TabIndex = 4;
            label3.Text = "父级字典：";
            // 
            // txtDicName
            // 
            txtDicName.Location = new Point(120, 50);
            txtDicName.Name = "txtDicName";
            txtDicName.Size = new Size(400, 23);
            txtDicName.TabIndex = 3;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(20, 53);
            label2.Name = "label2";
            label2.Size = new Size(68, 17);
            label2.TabIndex = 2;
            label2.Text = "字典名称：";
            // 
            // txtDicNo
            // 
            txtDicNo.Location = new Point(120, 20);
            txtDicNo.Name = "txtDicNo";
            txtDicNo.Size = new Size(400, 23);
            txtDicNo.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(20, 23);
            label1.Name = "label1";
            label1.Size = new Size(68, 17);
            label1.TabIndex = 0;
            label1.Text = "字典编号：";
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
            btnCancel.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
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
            btnSave.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            btnSave.ForeColor = Color.White;
            btnSave.Location = new Point(227, 15);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(90, 30);
            btnSave.TabIndex = 0;
            btnSave.Text = "保存";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;
            // 
            // DictionaryEditForm
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
            Name = "DictionaryEditForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "编辑数据字典";
            Load += DictionaryEditForm_Load;
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
        private System.Windows.Forms.TextBox txtDicNo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtDicName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbParent;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtConfig;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtDbSql;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtRemark;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown nudOrderNo;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox chkEnable;
    }

}