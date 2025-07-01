

namespace GC_MES.WinForm.Forms.SystemForm.SubForm
{

    partial class DictionaryItemEditForm
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
            chkEnable = new CheckBox();
            nudOrderNo = new NumericUpDown();
            label4 = new Label();
            txtRemark = new TextBox();
            label3 = new Label();
            txtDicName = new TextBox();
            label2 = new Label();
            txtDicValue = new TextBox();
            label1 = new Label();
            pnlButtons = new Panel();
            btnCancel = new Button();
            btnSave = new Button();
            pnlContent.SuspendLayout();
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
            lblTitle.Size = new Size(484, 50);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "编辑字典项";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pnlContent
            // 
            pnlContent.Controls.Add(chkEnable);
            pnlContent.Controls.Add(nudOrderNo);
            pnlContent.Controls.Add(label4);
            pnlContent.Controls.Add(txtRemark);
            pnlContent.Controls.Add(label3);
            pnlContent.Controls.Add(txtDicName);
            pnlContent.Controls.Add(label2);
            pnlContent.Controls.Add(txtDicValue);
            pnlContent.Controls.Add(label1);
            pnlContent.Dock = DockStyle.Fill;
            pnlContent.Location = new Point(0, 50);
            pnlContent.Name = "pnlContent";
            pnlContent.Padding = new Padding(20);
            pnlContent.Size = new Size(484, 250);
            pnlContent.TabIndex = 1;
            // 
            // chkEnable
            // 
            chkEnable.AutoSize = true;
            chkEnable.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 134);
            chkEnable.Location = new Point(120, 200);
            chkEnable.Name = "chkEnable";
            chkEnable.Size = new Size(84, 24);
            chkEnable.TabIndex = 8;
            chkEnable.Text = "是否启用";
            chkEnable.UseVisualStyleBackColor = true;
            // 
            // nudOrderNo
            // 
            nudOrderNo.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 134);
            nudOrderNo.Location = new Point(120, 160);
            nudOrderNo.Maximum = new decimal(new int[] { 9999, 0, 0, 0 });
            nudOrderNo.Name = "nudOrderNo";
            nudOrderNo.Size = new Size(150, 24);
            nudOrderNo.TabIndex = 7;
            nudOrderNo.Value = new decimal(new int[] { 100, 0, 0, 0 });
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 134);
            label4.Location = new Point(20, 162);
            label4.Name = "label4";
            label4.Size = new Size(65, 20);
            label4.TabIndex = 6;
            label4.Text = "排序号：";
            // 
            // txtRemark
            // 
            txtRemark.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 134);
            txtRemark.Location = new Point(120, 120);
            txtRemark.Name = "txtRemark";
            txtRemark.Size = new Size(300, 24);
            txtRemark.TabIndex = 5;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 134);
            label3.Location = new Point(20, 123);
            label3.Name = "label3";
            label3.Size = new Size(51, 20);
            label3.TabIndex = 4;
            label3.Text = "备注：";
            // 
            // txtDicName
            // 
            txtDicName.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 134);
            txtDicName.Location = new Point(120, 80);
            txtDicName.Name = "txtDicName";
            txtDicName.Size = new Size(300, 24);
            txtDicName.TabIndex = 3;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 134);
            label2.Location = new Point(20, 83);
            label2.Name = "label2";
            label2.Size = new Size(79, 20);
            label2.TabIndex = 2;
            label2.Text = "字典文本：";
            // 
            // txtDicValue
            // 
            txtDicValue.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 134);
            txtDicValue.Location = new Point(120, 40);
            txtDicValue.Name = "txtDicValue";
            txtDicValue.Size = new Size(300, 24);
            txtDicValue.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 134);
            label1.Location = new Point(20, 43);
            label1.Name = "label1";
            label1.Size = new Size(65, 20);
            label1.TabIndex = 0;
            label1.Text = "字典值：";
            // 
            // pnlButtons
            // 
            pnlButtons.BackColor = Color.FromArgb(240, 240, 240);
            pnlButtons.Controls.Add(btnCancel);
            pnlButtons.Controls.Add(btnSave);
            pnlButtons.Dock = DockStyle.Bottom;
            pnlButtons.Location = new Point(0, 300);
            pnlButtons.Name = "pnlButtons";
            pnlButtons.Size = new Size(484, 60);
            pnlButtons.TabIndex = 2;
            // 
            // btnCancel
            // 
            btnCancel.BackColor = Color.FromArgb(180, 180, 180);
            btnCancel.FlatAppearance.BorderColor = Color.FromArgb(160, 160, 160);
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            btnCancel.Location = new Point(267, 15);
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
            btnSave.Location = new Point(127, 15);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(90, 30);
            btnSave.TabIndex = 0;
            btnSave.Text = "保存";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;
            // 
            // DictionaryItemEditForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(484, 360);
            Controls.Add(pnlContent);
            Controls.Add(lblTitle);
            Controls.Add(pnlButtons);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "DictionaryItemEditForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "编辑字典项";
            Load += DictionaryItemEditForm_Load;
            pnlContent.ResumeLayout(false);
            pnlContent.PerformLayout();
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
        private System.Windows.Forms.TextBox txtDicValue;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtDicName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtRemark;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown nudOrderNo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox chkEnable;
    }

}