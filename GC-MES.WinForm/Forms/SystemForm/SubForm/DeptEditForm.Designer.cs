

namespace GC_MES.WinForm.Forms.SystemForm.SubForm
{
    
        partial class DeptEditForm
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
                txtDeptCode = new TextBox();
                label2 = new Label();
                txtDeptName = new TextBox();
                label1 = new Label();
                pnlButtons = new Panel();
                btnCancel = new Button();
                btnSave = new Button();
                pnlContent.SuspendLayout();
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
                lblTitle.Text = "编辑部门";
                lblTitle.TextAlign = ContentAlignment.MiddleCenter;
                // 
                // pnlContent
                // 
                pnlContent.Controls.Add(txtDeptCode);
                pnlContent.Controls.Add(label2);
                pnlContent.Controls.Add(txtDeptName);
                pnlContent.Controls.Add(label1);
                pnlContent.Dock = DockStyle.Fill;
                pnlContent.Location = new Point(0, 50);
                pnlContent.Name = "pnlContent";
                pnlContent.Padding = new Padding(20);
                pnlContent.Size = new Size(484, 200);
                pnlContent.TabIndex = 1;
                // 
                // txtDeptCode
                // 
                txtDeptCode.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 134);
                txtDeptCode.Location = new Point(120, 90);
                txtDeptCode.Name = "txtDeptCode";
                txtDeptCode.Size = new Size(300, 24);
                txtDeptCode.TabIndex = 3;
                // 
                // label2
                // 
                label2.AutoSize = true;
                label2.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 134);
                label2.Location = new Point(20, 93);
                label2.Name = "label2";
                label2.Size = new Size(79, 20);
                label2.TabIndex = 2;
                label2.Text = "部门编码：";
                // 
                // txtDeptName
                // 
                txtDeptName.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 134);
                txtDeptName.Location = new Point(120, 40);
                txtDeptName.Name = "txtDeptName";
                txtDeptName.Size = new Size(300, 24);
                txtDeptName.TabIndex = 1;
                // 
                // label1
                // 
                label1.AutoSize = true;
                label1.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 134);
                label1.Location = new Point(20, 43);
                label1.Name = "label1";
                label1.Size = new Size(79, 20);
                label1.TabIndex = 0;
                label1.Text = "部门名称：";
                // 
                // pnlButtons
                // 
                pnlButtons.BackColor = Color.FromArgb(240, 240, 240);
                pnlButtons.Controls.Add(btnCancel);
                pnlButtons.Controls.Add(btnSave);
                pnlButtons.Dock = DockStyle.Bottom;
                pnlButtons.Location = new Point(0, 250);
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
                // DeptEditForm
                // 
                AutoScaleDimensions = new SizeF(7F, 17F);
                AutoScaleMode = AutoScaleMode.Font;
                ClientSize = new Size(484, 310);
                Controls.Add(pnlContent);
                Controls.Add(lblTitle);
                Controls.Add(pnlButtons);
                FormBorderStyle = FormBorderStyle.FixedDialog;
                MaximizeBox = false;
                MinimizeBox = false;
                Name = "DeptEditForm";
                StartPosition = FormStartPosition.CenterParent;
                Text = "编辑部门";
                Load += DeptEditForm_Load;
                pnlContent.ResumeLayout(false);
                pnlContent.PerformLayout();
                pnlButtons.ResumeLayout(false);
                ResumeLayout(false);
            }

            #endregion

            private System.Windows.Forms.Label lblTitle;
            private System.Windows.Forms.Panel pnlContent;
            private System.Windows.Forms.Panel pnlButtons;
            private System.Windows.Forms.Button btnCancel;
            private System.Windows.Forms.Button btnSave;
            private System.Windows.Forms.TextBox txtDeptName;
            private System.Windows.Forms.Label label1;
            private System.Windows.Forms.TextBox txtDeptCode;
            private System.Windows.Forms.Label label2;
        }
    
}