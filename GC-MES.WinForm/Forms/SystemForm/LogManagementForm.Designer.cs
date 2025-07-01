namespace GC_MES.WinForm.Forms.SystemForm
{
  
        partial class LogManagementForm
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
                this.pnlSearch = new System.Windows.Forms.Panel();
                this.btnSearch = new System.Windows.Forms.Button();
                this.dtpEndDate = new System.Windows.Forms.DateTimePicker();
                this.label3 = new System.Windows.Forms.Label();
                this.dtpStartDate = new System.Windows.Forms.DateTimePicker();
                this.label2 = new System.Windows.Forms.Label();
                this.txtSearchContent = new System.Windows.Forms.TextBox();
                this.label1 = new System.Windows.Forms.Label();
                this.pnlToolbar = new System.Windows.Forms.Panel();
                this.btnExportPDF = new System.Windows.Forms.Button();
                this.btnExport = new System.Windows.Forms.Button();
                this.btnDelete = new System.Windows.Forms.Button();
                this.btnViewDetail = new System.Windows.Forms.Button();
                this.dgvLogs = new System.Windows.Forms.DataGridView();
                this.pnlPager = new System.Windows.Forms.Panel();
                this.lblPageInfo = new System.Windows.Forms.Label();
                this.btnLastPage = new System.Windows.Forms.Button();
                this.btnNextPage = new System.Windows.Forms.Button();
                this.btnPrevPage = new System.Windows.Forms.Button();
                this.btnFirstPage = new System.Windows.Forms.Button();
                this.cmbLogType = new System.Windows.Forms.ComboBox();
                this.label4 = new System.Windows.Forms.Label();

                // 
                // pnlSearch
                // 
                this.pnlSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(53)))));
                this.pnlSearch.Controls.Add(this.label4);
                this.pnlSearch.Controls.Add(this.cmbLogType);
                this.pnlSearch.Controls.Add(this.btnSearch);
                this.pnlSearch.Controls.Add(this.dtpEndDate);
                this.pnlSearch.Controls.Add(this.label3);
                this.pnlSearch.Controls.Add(this.dtpStartDate);
                this.pnlSearch.Controls.Add(this.label2);
                this.pnlSearch.Controls.Add(this.txtSearchContent);
                this.pnlSearch.Controls.Add(this.label1);
                this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Top;
                this.pnlSearch.Location = new System.Drawing.Point(0, 0);
                this.pnlSearch.Name = "pnlSearch";
                this.pnlSearch.Size = new System.Drawing.Size(1493, 80);
                this.pnlSearch.TabIndex = 0;

                // 
                // btnSearch
                // 
                this.btnSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
                this.btnSearch.FlatAppearance.BorderSize = 0;
                this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                this.btnSearch.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                this.btnSearch.ForeColor = System.Drawing.Color.White;
                this.btnSearch.Location = new System.Drawing.Point(1100, 20);
                this.btnSearch.Name = "btnSearch";
                this.btnSearch.Size = new System.Drawing.Size(90, 40);
                this.btnSearch.TabIndex = 6;
                this.btnSearch.Text = "查询";
                this.btnSearch.UseVisualStyleBackColor = false;

                // 
                // dtpEndDate
                // 
                this.dtpEndDate.CalendarForeColor = System.Drawing.Color.White;
                this.dtpEndDate.CalendarMonthBackground = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
                this.dtpEndDate.CalendarTitleBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
                this.dtpEndDate.CalendarTitleForeColor = System.Drawing.Color.White;
                this.dtpEndDate.CustomFormat = "yyyy-MM-dd HH:mm:ss";
                this.dtpEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
                this.dtpEndDate.Location = new System.Drawing.Point(840, 28);
                this.dtpEndDate.Name = "dtpEndDate";
                this.dtpEndDate.Size = new System.Drawing.Size(200, 23);
                this.dtpEndDate.TabIndex = 5;

                // 
                // label3
                // 
                this.label3.AutoSize = true;
                this.label3.ForeColor = System.Drawing.Color.White;
                this.label3.Location = new System.Drawing.Point(770, 32);
                this.label3.Name = "label3";
                this.label3.Size = new System.Drawing.Size(64, 17);
                this.label3.TabIndex = 4;
                this.label3.Text = "结束时间:";

                // 
                // dtpStartDate
                // 
                this.dtpStartDate.CalendarForeColor = System.Drawing.Color.White;
                this.dtpStartDate.CalendarMonthBackground = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
                this.dtpStartDate.CalendarTitleBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
                this.dtpStartDate.CalendarTitleForeColor = System.Drawing.Color.White;
                this.dtpStartDate.CustomFormat = "yyyy-MM-dd HH:mm:ss";
                this.dtpStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
                this.dtpStartDate.Location = new System.Drawing.Point(550, 28);
                this.dtpStartDate.Name = "dtpStartDate";
                this.dtpStartDate.Size = new System.Drawing.Size(200, 23);
                this.dtpStartDate.TabIndex = 3;

                // 
                // label2
                // 
                this.label2.AutoSize = true;
                this.label2.ForeColor = System.Drawing.Color.White;
                this.label2.Location = new System.Drawing.Point(480, 32);
                this.label2.Name = "label2";
                this.label2.Size = new System.Drawing.Size(64, 17);
                this.label2.TabIndex = 2;
                this.label2.Text = "开始时间:";

                // 
                // txtSearchContent
                // 
                this.txtSearchContent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
                this.txtSearchContent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                this.txtSearchContent.ForeColor = System.Drawing.Color.White;
                this.txtSearchContent.Location = new System.Drawing.Point(100, 28);
                this.txtSearchContent.Name = "txtSearchContent";
                this.txtSearchContent.Size = new System.Drawing.Size(150, 23);
                this.txtSearchContent.TabIndex = 1;

                // 
                // label1
                // 
                this.label1.AutoSize = true;
                this.label1.ForeColor = System.Drawing.Color.White;
                this.label1.Location = new System.Drawing.Point(30, 32);
                this.label1.Name = "label1";
                this.label1.Size = new System.Drawing.Size(64, 17);
                this.label1.TabIndex = 0;
                this.label1.Text = "日志内容:";

                // 
                // pnlToolbar
                // 
                this.pnlToolbar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(53)))));
                this.pnlToolbar.Controls.Add(this.btnExportPDF);
                this.pnlToolbar.Controls.Add(this.btnExport);
                this.pnlToolbar.Controls.Add(this.btnDelete);
                this.pnlToolbar.Controls.Add(this.btnViewDetail);
                this.pnlToolbar.Dock = System.Windows.Forms.DockStyle.Top;
                this.pnlToolbar.Location = new System.Drawing.Point(0, 80);
                this.pnlToolbar.Name = "pnlToolbar";
                this.pnlToolbar.Size = new System.Drawing.Size(1493, 60);
                this.pnlToolbar.TabIndex = 1;

                // 
                // btnExportPDF
                // 
                this.btnExportPDF.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(65)))));
                this.btnExportPDF.FlatAppearance.BorderSize = 0;
                this.btnExportPDF.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                this.btnExportPDF.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                this.btnExportPDF.ForeColor = System.Drawing.Color.White;
                this.btnExportPDF.Location = new System.Drawing.Point(310, 10);
                this.btnExportPDF.Name = "btnExportPDF";
                this.btnExportPDF.Size = new System.Drawing.Size(90, 40);
                this.btnExportPDF.TabIndex = 3;
                this.btnExportPDF.Text = "导出PDF";
                this.btnExportPDF.UseVisualStyleBackColor = false;

                // 
                // btnExport
                // 
                this.btnExport.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(65)))));
                this.btnExport.FlatAppearance.BorderSize = 0;
                this.btnExport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                this.btnExport.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                this.btnExport.ForeColor = System.Drawing.Color.White;
                this.btnExport.Location = new System.Drawing.Point(210, 10);
                this.btnExport.Name = "btnExport";
                this.btnExport.Size = new System.Drawing.Size(90, 40);
                this.btnExport.TabIndex = 2;
                this.btnExport.Text = "导出Excel";
                this.btnExport.UseVisualStyleBackColor = false;

                // 
                // btnDelete
                // 
                this.btnDelete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(65)))));
                this.btnDelete.FlatAppearance.BorderSize = 0;
                this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                this.btnDelete.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                this.btnDelete.ForeColor = System.Drawing.Color.White;
                this.btnDelete.Location = new System.Drawing.Point(110, 10);
                this.btnDelete.Name = "btnDelete";
                this.btnDelete.Size = new System.Drawing.Size(90, 40);
                this.btnDelete.TabIndex = 1;
                this.btnDelete.Text = "删除";
                this.btnDelete.UseVisualStyleBackColor = false;

                // 
                // btnViewDetail
                // 
                this.btnViewDetail.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
                this.btnViewDetail.FlatAppearance.BorderSize = 0;
                this.btnViewDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                this.btnViewDetail.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                this.btnViewDetail.ForeColor = System.Drawing.Color.White;
                this.btnViewDetail.Location = new System.Drawing.Point(10, 10);
                this.btnViewDetail.Name = "btnViewDetail";
                this.btnViewDetail.Size = new System.Drawing.Size(90, 40);
                this.btnViewDetail.TabIndex = 0;
                this.btnViewDetail.Text = "查看详情";
                this.btnViewDetail.UseVisualStyleBackColor = false;

                // 
                // dgvLogs
                // 
                this.dgvLogs.AllowUserToAddRows = false;
                this.dgvLogs.AllowUserToDeleteRows = false;
                this.dgvLogs.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
                this.dgvLogs.BorderStyle = System.Windows.Forms.BorderStyle.None;
                this.dgvLogs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
                this.dgvLogs.Dock = System.Windows.Forms.DockStyle.Fill;
                this.dgvLogs.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(65)))));
                this.dgvLogs.Location = new System.Drawing.Point(0, 140);
                this.dgvLogs.Name = "dgvLogs";
                this.dgvLogs.ReadOnly = true;
                this.dgvLogs.RowTemplate.Height = 25;
                this.dgvLogs.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
                this.dgvLogs.Size = new System.Drawing.Size(1493, 778);
                this.dgvLogs.TabIndex = 2;

                // 
                // pnlPager
                // 
                this.pnlPager.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(53)))));
                this.pnlPager.Controls.Add(this.lblPageInfo);
                this.pnlPager.Controls.Add(this.btnLastPage);
                this.pnlPager.Controls.Add(this.btnNextPage);
                this.pnlPager.Controls.Add(this.btnPrevPage);
                this.pnlPager.Controls.Add(this.btnFirstPage);
                this.pnlPager.Dock = System.Windows.Forms.DockStyle.Bottom;
                this.pnlPager.Location = new System.Drawing.Point(0, 918);
                this.pnlPager.Name = "pnlPager";
                this.pnlPager.Size = new System.Drawing.Size(1493, 74);
                this.pnlPager.TabIndex = 3;

                // 
                // lblPageInfo
                // 
                this.lblPageInfo.AutoSize = true;
                this.lblPageInfo.ForeColor = System.Drawing.Color.White;
                this.lblPageInfo.Location = new System.Drawing.Point(240, 29);
                this.lblPageInfo.Name = "lblPageInfo";
                this.lblPageInfo.Size = new System.Drawing.Size(127, 17);
                this.lblPageInfo.TabIndex = 4;
                this.lblPageInfo.Text = "第1页/共1页 共0条记录";

                // 
                // btnLastPage
                // 
                this.btnLastPage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(65)))));
                this.btnLastPage.FlatAppearance.BorderSize = 0;
                this.btnLastPage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                this.btnLastPage.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                this.btnLastPage.ForeColor = System.Drawing.Color.White;
                this.btnLastPage.Location = new System.Drawing.Point(183, 17);
                this.btnLastPage.Name = "btnLastPage";
                this.btnLastPage.Size = new System.Drawing.Size(41, 40);
                this.btnLastPage.TabIndex = 3;
                this.btnLastPage.Text = ">|";
                this.btnLastPage.UseVisualStyleBackColor = false;

                // 
                // btnNextPage
                // 
                this.btnNextPage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(65)))));
                this.btnNextPage.FlatAppearance.BorderSize = 0;
                this.btnNextPage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                this.btnNextPage.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                this.btnNextPage.ForeColor = System.Drawing.Color.White;
                this.btnNextPage.Location = new System.Drawing.Point(126, 17);
                this.btnNextPage.Name = "btnNextPage";
                this.btnNextPage.Size = new System.Drawing.Size(41, 40);
                this.btnNextPage.TabIndex = 2;
                this.btnNextPage.Text = ">";
                this.btnNextPage.UseVisualStyleBackColor = false;

                // 
                // btnPrevPage
                // 
                this.btnPrevPage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(65)))));
                this.btnPrevPage.FlatAppearance.BorderSize = 0;
                this.btnPrevPage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                this.btnPrevPage.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                this.btnPrevPage.ForeColor = System.Drawing.Color.White;
                this.btnPrevPage.Location = new System.Drawing.Point(69, 17);
                this.btnPrevPage.Name = "btnPrevPage";
                this.btnPrevPage.Size = new System.Drawing.Size(41, 40);
                this.btnPrevPage.TabIndex = 1;
                this.btnPrevPage.Text = "<";
                this.btnPrevPage.UseVisualStyleBackColor = false;

                // 
                // btnFirstPage
                // 
                this.btnFirstPage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(65)))));
                this.btnFirstPage.FlatAppearance.BorderSize = 0;
                this.btnFirstPage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                this.btnFirstPage.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                this.btnFirstPage.ForeColor = System.Drawing.Color.White;
                this.btnFirstPage.Location = new System.Drawing.Point(12, 17);
                this.btnFirstPage.Name = "btnFirstPage";
                this.btnFirstPage.Size = new System.Drawing.Size(41, 40);
                this.btnFirstPage.TabIndex = 0;
                this.btnFirstPage.Text = "|<";
                this.btnFirstPage.UseVisualStyleBackColor = false;

                // 
                // cmbLogType
                // 
                this.cmbLogType.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
                this.cmbLogType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
                this.cmbLogType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                this.cmbLogType.ForeColor = System.Drawing.Color.White;
                this.cmbLogType.FormattingEnabled = true;
                this.cmbLogType.Location = new System.Drawing.Point(330, 28);
                this.cmbLogType.Name = "cmbLogType";
                this.cmbLogType.Size = new System.Drawing.Size(130, 25);
                this.cmbLogType.TabIndex = 7;

                // 
                // label4
                // 
                this.label4.AutoSize = true;
                this.label4.ForeColor = System.Drawing.Color.White;
                this.label4.Location = new System.Drawing.Point(260, 32);
                this.label4.Name = "label4";
                this.label4.Size = new System.Drawing.Size(64, 17);
                this.label4.TabIndex = 8;
                this.label4.Text = "日志类型:";

                // 
                // LogManagementForm
                // 
                this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
                this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
                this.ClientSize = new System.Drawing.Size(1493, 992);
                this.Controls.Add(this.dgvLogs);
                this.Controls.Add(this.pnlPager);
                this.Controls.Add(this.pnlToolbar);
                this.Controls.Add(this.pnlSearch);
                this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                this.MinimumSize = new System.Drawing.Size(1200, 800);
                this.Name = "LogManagementForm";
                this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
                this.Text = "日志管理";
                this.Load += new System.EventHandler(this.LogManagementForm_Load);
                this.pnlSearch.ResumeLayout(false);
                this.pnlSearch.PerformLayout();
                this.pnlToolbar.ResumeLayout(false);
                ((System.ComponentModel.ISupportInitialize)(this.dgvLogs)).EndInit();
                this.pnlPager.ResumeLayout(false);
                this.pnlPager.PerformLayout();
                this.ResumeLayout(false);
            }

            #endregion

            private System.Windows.Forms.Panel pnlSearch;
            private System.Windows.Forms.TextBox txtSearchContent;
            private System.Windows.Forms.Label label1;
            private System.Windows.Forms.Panel pnlToolbar;
            private System.Windows.Forms.Button btnExportPDF;
            private System.Windows.Forms.Button btnExport;
            private System.Windows.Forms.Button btnDelete;
            private System.Windows.Forms.Button btnViewDetail;
            private System.Windows.Forms.DataGridView dgvLogs;
            private System.Windows.Forms.Panel pnlPager;
            private System.Windows.Forms.Label lblPageInfo;
            private System.Windows.Forms.Button btnLastPage;
            private System.Windows.Forms.Button btnNextPage;
            private System.Windows.Forms.Button btnPrevPage;
            private System.Windows.Forms.Button btnFirstPage;
            private System.Windows.Forms.DateTimePicker dtpEndDate;
            private System.Windows.Forms.Label label3;
            private System.Windows.Forms.DateTimePicker dtpStartDate;
            private System.Windows.Forms.Label label2;
            private System.Windows.Forms.Button btnSearch;
            private System.Windows.Forms.Label label4;
            private System.Windows.Forms.ComboBox cmbLogType;
        }
    }
