namespace GC_MES.WinForm.Forms
{
    partial class QualityInspectionItemEditForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkIsCritical = new System.Windows.Forms.CheckBox();
            this.label11 = new System.Windows.Forms.Label();
            this.numSequenceNo = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.txtRemark = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtDefectType = new System.Windows.Forms.TextBox();
            this.lblDefectType = new System.Windows.Forms.Label();
            this.txtDefectReason = new System.Windows.Forms.TextBox();
            this.lblDefectReason = new System.Windows.Forms.Label();
            this.cmbResult = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtActualValue = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtUnit = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.numLowerLimit = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.numUpperLimit = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.txtStandardValue = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtInspectionTool = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtInspectionMethod = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.txtItemName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtItemCode = new System.Windows.Forms.TextBox();
            this.lblItemCode = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSequenceNo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLowerLimit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpperLimit)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(584, 461);
            this.panel1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.chkIsCritical);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.numSequenceNo);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.txtRemark);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.txtDefectType);
            this.groupBox1.Controls.Add(this.lblDefectType);
            this.groupBox1.Controls.Add(this.txtDefectReason);
            this.groupBox1.Controls.Add(this.lblDefectReason);
            this.groupBox1.Controls.Add(this.cmbResult);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.txtActualValue);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txtUnit);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.numLowerLimit);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.numUpperLimit);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtStandardValue);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtInspectionTool);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtInspectionMethod);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.txtItemName);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtItemCode);
            this.groupBox1.Controls.Add(this.lblItemCode);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(560, 399);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "检验项目信息";
            // 
            // chkIsCritical
            // 
            this.chkIsCritical.AutoSize = true;
            this.chkIsCritical.Location = new System.Drawing.Point(310, 329);
            this.chkIsCritical.Name = "chkIsCritical";
            this.chkIsCritical.Size = new System.Drawing.Size(84, 16);
            this.chkIsCritical.TabIndex = 29;
            this.chkIsCritical.Text = "是否关键项";
            this.chkIsCritical.UseVisualStyleBackColor = true;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(263, 330);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(41, 12);
            this.label11.TabIndex = 28;
            this.label11.Text = "属性：";
            // 
            // numSequenceNo
            // 
            this.numSequenceNo.Location = new System.Drawing.Point(107, 327);
            this.numSequenceNo.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numSequenceNo.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numSequenceNo.Name = "numSequenceNo";
            this.numSequenceNo.Size = new System.Drawing.Size(120, 21);
            this.numSequenceNo.TabIndex = 27;
            this.numSequenceNo.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(36, 330);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(65, 12);
            this.label10.TabIndex = 26;
            this.label10.Text = "检验顺序：";
            // 
            // txtRemark
            // 
            this.txtRemark.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRemark.Location = new System.Drawing.Point(107, 358);
            this.txtRemark.Multiline = true;
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.Size = new System.Drawing.Size(433, 35);
            this.txtRemark.TabIndex = 25;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(36, 361);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(65, 12);
            this.label9.TabIndex = 24;
            this.label9.Text = "备    注：";
            // 
            // txtDefectType
            // 
            this.txtDefectType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDefectType.Enabled = false;
            this.txtDefectType.Location = new System.Drawing.Point(107, 297);
            this.txtDefectType.Name = "txtDefectType";
            this.txtDefectType.Size = new System.Drawing.Size(433, 21);
            this.txtDefectType.TabIndex = 23;
            // 
            // lblDefectType
            // 
            this.lblDefectType.AutoSize = true;
            this.lblDefectType.Enabled = false;
            this.lblDefectType.Location = new System.Drawing.Point(36, 300);
            this.lblDefectType.Name = "lblDefectType";
            this.lblDefectType.Size = new System.Drawing.Size(65, 12);
            this.lblDefectType.TabIndex = 22;
            this.lblDefectType.Text = "不合格类型：";
            // 
            // txtDefectReason
            // 
            this.txtDefectReason.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDefectReason.Enabled = false;
            this.txtDefectReason.Location = new System.Drawing.Point(107, 267);
            this.txtDefectReason.Name = "txtDefectReason";
            this.txtDefectReason.Size = new System.Drawing.Size(433, 21);
            this.txtDefectReason.TabIndex = 21;
            // 
            // lblDefectReason
            // 
            this.lblDefectReason.AutoSize = true;
            this.lblDefectReason.Enabled = false;
            this.lblDefectReason.Location = new System.Drawing.Point(36, 270);
            this.lblDefectReason.Name = "lblDefectReason";
            this.lblDefectReason.Size = new System.Drawing.Size(65, 12);
            this.lblDefectReason.TabIndex = 20;
            this.lblDefectReason.Text = "不合格原因：";
            // 
            // cmbResult
            // 
            this.cmbResult.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbResult.FormattingEnabled = true;
            this.cmbResult.Location = new System.Drawing.Point(107, 238);
            this.cmbResult.Name = "cmbResult";
            this.cmbResult.Size = new System.Drawing.Size(120, 20);
            this.cmbResult.TabIndex = 19;
            this.cmbResult.SelectedIndexChanged += new System.EventHandler(this.cmbResult_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(36, 241);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 18;
            this.label6.Text = "检验结果：";
            // 
            // txtActualValue
            // 
            this.txtActualValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtActualValue.Location = new System.Drawing.Point(107, 208);
            this.txtActualValue.Name = "txtActualValue";
            this.txtActualValue.Size = new System.Drawing.Size(433, 21);
            this.txtActualValue.TabIndex = 17;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(36, 211);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 16;
            this.label5.Text = "实 测 值：";
            // 
            // txtUnit
            // 
            this.txtUnit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtUnit.Location = new System.Drawing.Point(420, 178);
            this.txtUnit.Name = "txtUnit";
            this.txtUnit.Size = new System.Drawing.Size(120, 21);
            this.txtUnit.TabIndex = 15;
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(373, 181);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(41, 12);
            this.label8.TabIndex = 14;
            this.label8.Text = "单位：";
            // 
            // numLowerLimit
            // 
            this.numLowerLimit.DecimalPlaces = 3;
            this.numLowerLimit.Location = new System.Drawing.Point(249, 178);
            this.numLowerLimit.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numLowerLimit.Name = "numLowerLimit";
            this.numLowerLimit.Size = new System.Drawing.Size(100, 21);
            this.numLowerLimit.TabIndex = 13;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(225, 181);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(17, 12);
            this.label7.TabIndex = 12;
            this.label7.Text = "～";
            // 
            // numUpperLimit
            // 
            this.numUpperLimit.DecimalPlaces = 3;
            this.numUpperLimit.Location = new System.Drawing.Point(107, 178);
            this.numUpperLimit.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numUpperLimit.Name = "numUpperLimit";
            this.numUpperLimit.Size = new System.Drawing.Size(100, 21);
            this.numUpperLimit.TabIndex = 11;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(36, 181);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 10;
            this.label4.Text = "规格范围：";
            // 
            // txtStandardValue
            // 
            this.txtStandardValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtStandardValue.Location = new System.Drawing.Point(107, 148);
            this.txtStandardValue.Name = "txtStandardValue";
            this.txtStandardValue.Size = new System.Drawing.Size(433, 21);
            this.txtStandardValue.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(36, 151);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 8;
            this.label3.Text = "标 准 值：";
            // 
            // txtInspectionTool
            // 
            this.txtInspectionTool.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtInspectionTool.Location = new System.Drawing.Point(107, 118);
            this.txtInspectionTool.Name = "txtInspectionTool";
            this.txtInspectionTool.Size = new System.Drawing.Size(433, 21);
            this.txtInspectionTool.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(36, 121);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = "检验工具：";
            // 
            // txtInspectionMethod
            // 
            this.txtInspectionMethod.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtInspectionMethod.Location = new System.Drawing.Point(107, 88);
            this.txtInspectionMethod.Name = "txtInspectionMethod";
            this.txtInspectionMethod.Size = new System.Drawing.Size(433, 21);
            this.txtInspectionMethod.TabIndex = 5;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(36, 91);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(65, 12);
            this.label12.TabIndex = 4;
            this.label12.Text = "检验方法：";
            // 
            // txtItemName
            // 
            this.txtItemName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtItemName.Location = new System.Drawing.Point(107, 58);
            this.txtItemName.Name = "txtItemName";
            this.txtItemName.Size = new System.Drawing.Size(433, 21);
            this.txtItemName.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(36, 61);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "项目名称：";
            // 
            // txtItemCode
            // 
            this.txtItemCode.Location = new System.Drawing.Point(107, 28);
            this.txtItemCode.Name = "txtItemCode";
            this.txtItemCode.Size = new System.Drawing.Size(156, 21);
            this.txtItemCode.TabIndex = 1;
            // 
            // lblItemCode
            // 
            this.lblItemCode.AutoSize = true;
            this.lblItemCode.Location = new System.Drawing.Point(36, 31);
            this.lblItemCode.Name = "lblItemCode";
            this.lblItemCode.Size = new System.Drawing.Size(65, 12);
            this.lblItemCode.TabIndex = 0;
            this.lblItemCode.Text = "项目编号：";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Controls.Add(this.btnSave);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 411);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(584, 50);
            this.panel2.TabIndex = 1;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(497, 14);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(416, 14);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // QualityInspectionItemEditForm
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(584, 461);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "QualityInspectionItemEditForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "检验项目编辑";
            this.Load += new System.EventHandler(this.QualityInspectionItemEditForm_Load);
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSequenceNo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLowerLimit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpperLimit)).EndInit();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtItemCode;
        private System.Windows.Forms.Label lblItemCode;
        private System.Windows.Forms.TextBox txtItemName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtInspectionMethod;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtInspectionTool;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtStandardValue;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numUpperLimit;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numLowerLimit;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtUnit;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtActualValue;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmbResult;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtDefectReason;
        private System.Windows.Forms.Label lblDefectReason;
        private System.Windows.Forms.TextBox txtDefectType;
        private System.Windows.Forms.Label lblDefectType;
        private System.Windows.Forms.TextBox txtRemark;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown numSequenceNo;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox chkIsCritical;
        private System.Windows.Forms.Label label11;
    }
} 