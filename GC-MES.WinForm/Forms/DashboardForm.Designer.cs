using System.Windows.Forms.DataVisualization.Charting;

namespace GC_MES.WinForm.Forms
{
    partial class DashboardForm
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea4 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend4 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblCurrentTime = new System.Windows.Forms.Label();
            this.lblCompanyName = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblPendingOrders = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.lblInventoryWarnings = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.lblDeviceFaults = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.lblQualityIssues = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.lblCompletionRate = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.panel7 = new System.Windows.Forms.Panel();
            this.lblUtilizationRate = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.panel8 = new System.Windows.Forms.Panel();
            this.chartPlanCompletion = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.panel9 = new System.Windows.Forms.Panel();
            this.chartDeviceStatus = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.panel10 = new System.Windows.Forms.Panel();
            this.chartProductionTrend = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.panel11 = new System.Windows.Forms.Panel();
            this.chartQualityAnalysis = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnProductionMonitor = new System.Windows.Forms.Button();
            this.btnWorkOrderManagement = new System.Windows.Forms.Button();
            this.btnInventoryManagement = new System.Windows.Forms.Button();
            this.btnDeviceManagement = new System.Windows.Forms.Button();
            this.btnQualityControl = new System.Windows.Forms.Button();
            this.btnReportAnalysis = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel7.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.panel8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartPlanCompletion)).BeginInit();
            this.panel9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartDeviceStatus)).BeginInit();
            this.panel10.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartProductionTrend)).BeginInit();
            this.panel11.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartQualityAnalysis)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1033, 684);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblCurrentTime);
            this.panel1.Controls.Add(this.lblCompanyName);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1027, 44);
            this.panel1.TabIndex = 0;
            // 
            // lblCurrentTime
            // 
            this.lblCurrentTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCurrentTime.AutoSize = true;
            this.lblCurrentTime.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblCurrentTime.Location = new System.Drawing.Point(853, 12);
            this.lblCurrentTime.Name = "lblCurrentTime";
            this.lblCurrentTime.Size = new System.Drawing.Size(143, 20);
            this.lblCurrentTime.TabIndex = 1;
            this.lblCurrentTime.Text = "2023-06-01 12:00:00";
            // 
            // lblCompanyName
            // 
            this.lblCompanyName.AutoSize = true;
            this.lblCompanyName.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblCompanyName.Location = new System.Drawing.Point(12, 9);
            this.lblCompanyName.Name = "lblCompanyName";
            this.lblCompanyName.Size = new System.Drawing.Size(164, 26);
            this.lblCompanyName.TabIndex = 0;
            this.lblCompanyName.Text = "广创智能制造系统";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 6;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel2.Controls.Add(this.panel2, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.panel3, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.panel4, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.panel5, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.panel6, 4, 0);
            this.tableLayoutPanel2.Controls.Add(this.panel7, 5, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 53);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1027, 94);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(140)))), ((int)(((byte)(240)))));
            this.panel2.Controls.Add(this.lblPendingOrders);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(165, 88);
            this.panel2.TabIndex = 0;
            // 
            // lblPendingOrders
            // 
            this.lblPendingOrders.AutoSize = true;
            this.lblPendingOrders.Font = new System.Drawing.Font("Microsoft YaHei UI", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblPendingOrders.ForeColor = System.Drawing.Color.White;
            this.lblPendingOrders.Location = new System.Drawing.Point(61, 37);
            this.lblPendingOrders.Name = "lblPendingOrders";
            this.lblPendingOrders.Size = new System.Drawing.Size(38, 42);
            this.lblPendingOrders.TabIndex = 1;
            this.lblPendingOrders.Text = "0";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(30, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "待处理工单数";
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(180)))), ((int)(((byte)(65)))));
            this.panel3.Controls.Add(this.lblInventoryWarnings);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(174, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(165, 88);
            this.panel3.TabIndex = 1;
            // 
            // lblInventoryWarnings
            // 
            this.lblInventoryWarnings.AutoSize = true;
            this.lblInventoryWarnings.Font = new System.Drawing.Font("Microsoft YaHei UI", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblInventoryWarnings.ForeColor = System.Drawing.Color.White;
            this.lblInventoryWarnings.Location = new System.Drawing.Point(61, 37);
            this.lblInventoryWarnings.Name = "lblInventoryWarnings";
            this.lblInventoryWarnings.Size = new System.Drawing.Size(38, 42);
            this.lblInventoryWarnings.TabIndex = 1;
            this.lblInventoryWarnings.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(30, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(93, 20);
            this.label3.TabIndex = 0;
            this.label3.Text = "库存预警数量";
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(131)))), ((int)(((byte)(10)))));
            this.panel4.Controls.Add(this.lblDeviceFaults);
            this.panel4.Controls.Add(this.label5);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(345, 3);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(165, 88);
            this.panel4.TabIndex = 2;
            // 
            // lblDeviceFaults
            // 
            this.lblDeviceFaults.AutoSize = true;
            this.lblDeviceFaults.Font = new System.Drawing.Font("Microsoft YaHei UI", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblDeviceFaults.ForeColor = System.Drawing.Color.White;
            this.lblDeviceFaults.Location = new System.Drawing.Point(61, 37);
            this.lblDeviceFaults.Name = "lblDeviceFaults";
            this.lblDeviceFaults.Size = new System.Drawing.Size(38, 42);
            this.lblDeviceFaults.TabIndex = 1;
            this.lblDeviceFaults.Text = "0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(30, 13);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(93, 20);
            this.label5.TabIndex = 0;
            this.label5.Text = "设备故障数量";
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.panel5.Controls.Add(this.lblQualityIssues);
            this.panel5.Controls.Add(this.label7);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(516, 3);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(165, 88);
            this.panel5.TabIndex = 3;
            // 
            // lblQualityIssues
            // 
            this.lblQualityIssues.AutoSize = true;
            this.lblQualityIssues.Font = new System.Drawing.Font("Microsoft YaHei UI", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblQualityIssues.ForeColor = System.Drawing.Color.White;
            this.lblQualityIssues.Location = new System.Drawing.Point(61, 37);
            this.lblQualityIssues.Name = "lblQualityIssues";
            this.lblQualityIssues.Size = new System.Drawing.Size(38, 42);
            this.lblQualityIssues.TabIndex = 1;
            this.lblQualityIssues.Text = "0";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(30, 13);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(93, 20);
            this.label7.TabIndex = 0;
            this.label7.Text = "质量异常数量";
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.panel6.Controls.Add(this.lblCompletionRate);
            this.panel6.Controls.Add(this.label9);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel6.Location = new System.Drawing.Point(687, 3);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(165, 88);
            this.panel6.TabIndex = 4;
            // 
            // lblCompletionRate
            // 
            this.lblCompletionRate.AutoSize = true;
            this.lblCompletionRate.Font = new System.Drawing.Font("Microsoft YaHei UI", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblCompletionRate.ForeColor = System.Drawing.Color.White;
            this.lblCompletionRate.Location = new System.Drawing.Point(34, 37);
            this.lblCompletionRate.Name = "lblCompletionRate";
            this.lblCompletionRate.Size = new System.Drawing.Size(97, 42);
            this.lblCompletionRate.TabIndex = 1;
            this.lblCompletionRate.Text = "0.0%";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label9.ForeColor = System.Drawing.Color.White;
            this.label9.Location = new System.Drawing.Point(31, 13);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(79, 20);
            this.label9.TabIndex = 0;
            this.label9.Text = "计划完成率";
            // 
            // panel7
            // 
            this.panel7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(162)))), ((int)(((byte)(184)))));
            this.panel7.Controls.Add(this.lblUtilizationRate);
            this.panel7.Controls.Add(this.label11);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel7.Location = new System.Drawing.Point(858, 3);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(166, 88);
            this.panel7.TabIndex = 5;
            // 
            // lblUtilizationRate
            // 
            this.lblUtilizationRate.AutoSize = true;
            this.lblUtilizationRate.Font = new System.Drawing.Font("Microsoft YaHei UI", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblUtilizationRate.ForeColor = System.Drawing.Color.White;
            this.lblUtilizationRate.Location = new System.Drawing.Point(36, 37);
            this.lblUtilizationRate.Name = "lblUtilizationRate";
            this.lblUtilizationRate.Size = new System.Drawing.Size(97, 42);
            this.lblUtilizationRate.TabIndex = 1;
            this.lblUtilizationRate.Text = "0.0%";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label11.ForeColor = System.Drawing.Color.White;
            this.label11.Location = new System.Drawing.Point(38, 13);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(79, 20);
            this.label11.TabIndex = 0;
            this.label11.Text = "设备利用率";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this.panel8, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.panel9, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.panel10, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.panel11, 1, 1);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 153);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(1027, 478);
            this.tableLayoutPanel3.TabIndex = 2;
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.chartPlanCompletion);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel8.Location = new System.Drawing.Point(3, 3);
            this.panel8.Name = "panel8";
            this.panel8.Padding = new System.Windows.Forms.Padding(10);
            this.panel8.Size = new System.Drawing.Size(507, 233);
            this.panel8.TabIndex = 0;
            // 
            // chartPlanCompletion
            // 
            chartArea1.Name = "ChartArea1";
            this.chartPlanCompletion.ChartAreas.Add(chartArea1);
            this.chartPlanCompletion.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Name = "Legend1";
            this.chartPlanCompletion.Legends.Add(legend1);
            this.chartPlanCompletion.Location = new System.Drawing.Point(10, 10);
            this.chartPlanCompletion.Name = "chartPlanCompletion";
            this.chartPlanCompletion.Size = new System.Drawing.Size(487, 213);
            this.chartPlanCompletion.TabIndex = 0;
            this.chartPlanCompletion.Text = "生产计划完成情况";
            // 
            // panel9
            // 
            this.panel9.Controls.Add(this.chartDeviceStatus);
            this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel9.Location = new System.Drawing.Point(516, 3);
            this.panel9.Name = "panel9";
            this.panel9.Padding = new System.Windows.Forms.Padding(10);
            this.panel9.Size = new System.Drawing.Size(508, 233);
            this.panel9.TabIndex = 1;
            // 
            // chartDeviceStatus
            // 
            chartArea2.Name = "ChartArea1";
            this.chartDeviceStatus.ChartAreas.Add(chartArea2);
            this.chartDeviceStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            legend2.Name = "Legend1";
            this.chartDeviceStatus.Legends.Add(legend2);
            this.chartDeviceStatus.Location = new System.Drawing.Point(10, 10);
            this.chartDeviceStatus.Name = "chartDeviceStatus";
            this.chartDeviceStatus.Size = new System.Drawing.Size(488, 213);
            this.chartDeviceStatus.TabIndex = 0;
            this.chartDeviceStatus.Text = "设备状态";
            // 
            // panel10
            // 
            this.panel10.Controls.Add(this.chartProductionTrend);
            this.panel10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel10.Location = new System.Drawing.Point(3, 242);
            this.panel10.Name = "panel10";
            this.panel10.Padding = new System.Windows.Forms.Padding(10);
            this.panel10.Size = new System.Drawing.Size(507, 233);
            this.panel10.TabIndex = 2;
            // 
            // chartProductionTrend
            // 
            chartArea3.Name = "ChartArea1";
            this.chartProductionTrend.ChartAreas.Add(chartArea3);
            this.chartProductionTrend.Dock = System.Windows.Forms.DockStyle.Fill;
            legend3.Name = "Legend1";
            this.chartProductionTrend.Legends.Add(legend3);
            this.chartProductionTrend.Location = new System.Drawing.Point(10, 10);
            this.chartProductionTrend.Name = "chartProductionTrend";
            this.chartProductionTrend.Size = new System.Drawing.Size(487, 213);
            this.chartProductionTrend.TabIndex = 0;
            this.chartProductionTrend.Text = "产量趋势";
            // 
            // panel11
            // 
            this.panel11.Controls.Add(this.chartQualityAnalysis);
            this.panel11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel11.Location = new System.Drawing.Point(516, 242);
            this.panel11.Name = "panel11";
            this.panel11.Padding = new System.Windows.Forms.Padding(10);
            this.panel11.Size = new System.Drawing.Size(508, 233);
            this.panel11.TabIndex = 3;
            // 
            // chartQualityAnalysis
            // 
            chartArea4.Name = "ChartArea1";
            this.chartQualityAnalysis.ChartAreas.Add(chartArea4);
            this.chartQualityAnalysis.Dock = System.Windows.Forms.DockStyle.Fill;
            legend4.Name = "Legend1";
            this.chartQualityAnalysis.Legends.Add(legend4);
            this.chartQualityAnalysis.Location = new System.Drawing.Point(10, 10);
            this.chartQualityAnalysis.Name = "chartQualityAnalysis";
            this.chartQualityAnalysis.Size = new System.Drawing.Size(488, 213);
            this.chartQualityAnalysis.TabIndex = 0;
            this.chartQualityAnalysis.Text = "质量分析";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.btnRefresh);
            this.flowLayoutPanel1.Controls.Add(this.btnProductionMonitor);
            this.flowLayoutPanel1.Controls.Add(this.btnWorkOrderManagement);
            this.flowLayoutPanel1.Controls.Add(this.btnInventoryManagement);
            this.flowLayoutPanel1.Controls.Add(this.btnDeviceManagement);
            this.flowLayoutPanel1.Controls.Add(this.btnQualityControl);
            this.flowLayoutPanel1.Controls.Add(this.btnReportAnalysis);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 637);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1027, 44);
            this.flowLayoutPanel1.TabIndex = 3;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(3, 3);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 35);
            this.btnRefresh.TabIndex = 0;
            this.btnRefresh.Text = "刷新数据";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnProductionMonitor
            // 
            this.btnProductionMonitor.Location = new System.Drawing.Point(84, 3);
            this.btnProductionMonitor.Name = "btnProductionMonitor";
            this.btnProductionMonitor.Size = new System.Drawing.Size(100, 35);
            this.btnProductionMonitor.TabIndex = 1;
            this.btnProductionMonitor.Text = "生产监控";
            this.btnProductionMonitor.UseVisualStyleBackColor = true;
            this.btnProductionMonitor.Click += new System.EventHandler(this.btnProductionMonitor_Click);
            // 
            // btnWorkOrderManagement
            // 
            this.btnWorkOrderManagement.Location = new System.Drawing.Point(190, 3);
            this.btnWorkOrderManagement.Name = "btnWorkOrderManagement";
            this.btnWorkOrderManagement.Size = new System.Drawing.Size(100, 35);
            this.btnWorkOrderManagement.TabIndex = 2;
            this.btnWorkOrderManagement.Text = "工单管理";
            this.btnWorkOrderManagement.UseVisualStyleBackColor = true;
            this.btnWorkOrderManagement.Click += new System.EventHandler(this.btnWorkOrderManagement_Click);
            // 
            // btnInventoryManagement
            // 
            this.btnInventoryManagement.Location = new System.Drawing.Point(296, 3);
            this.btnInventoryManagement.Name = "btnInventoryManagement";
            this.btnInventoryManagement.Size = new System.Drawing.Size(100, 35);
            this.btnInventoryManagement.TabIndex = 3;
            this.btnInventoryManagement.Text = "库存管理";
            this.btnInventoryManagement.UseVisualStyleBackColor = true;
            this.btnInventoryManagement.Click += new System.EventHandler(this.btnInventoryManagement_Click);
            // 
            // btnDeviceManagement
            // 
            this.btnDeviceManagement.Location = new System.Drawing.Point(402, 3);
            this.btnDeviceManagement.Name = "btnDeviceManagement";
            this.btnDeviceManagement.Size = new System.Drawing.Size(100, 35);
            this.btnDeviceManagement.TabIndex = 4;
            this.btnDeviceManagement.Text = "设备管理";
            this.btnDeviceManagement.UseVisualStyleBackColor = true;
            this.btnDeviceManagement.Click += new System.EventHandler(this.btnDeviceManagement_Click);
            // 
            // btnQualityControl
            // 
            this.btnQualityControl.Location = new System.Drawing.Point(508, 3);
            this.btnQualityControl.Name = "btnQualityControl";
            this.btnQualityControl.Size = new System.Drawing.Size(100, 35);
            this.btnQualityControl.TabIndex = 5;
            this.btnQualityControl.Text = "质量控制";
            this.btnQualityControl.UseVisualStyleBackColor = true;
            this.btnQualityControl.Click += new System.EventHandler(this.btnQualityControl_Click);
            // 
            // btnReportAnalysis
            // 
            this.btnReportAnalysis.Location = new System.Drawing.Point(614, 3);
            this.btnReportAnalysis.Name = "btnReportAnalysis";
            this.btnReportAnalysis.Size = new System.Drawing.Size(100, 35);
            this.btnReportAnalysis.TabIndex = 6;
            this.btnReportAnalysis.Text = "报表分析";
            this.btnReportAnalysis.UseVisualStyleBackColor = true;
            this.btnReportAnalysis.Click += new System.EventHandler(this.btnReportAnalysis_Click);
            // 
            // DashboardForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1033, 684);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "DashboardForm";
            this.Text = "仪表板";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DashboardForm_FormClosing);
            this.Load += new System.EventHandler(this.DashboardForm_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.panel8.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartPlanCompletion)).EndInit();
            this.panel9.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartDeviceStatus)).EndInit();
            this.panel10.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartProductionTrend)).EndInit();
            this.panel11.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartQualityAnalysis)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblCurrentTime;
        private System.Windows.Forms.Label lblCompanyName;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lblPendingOrders;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label lblInventoryWarnings;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label lblDeviceFaults;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label lblQualityIssues;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Label lblCompletionRate;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Label lblUtilizationRate;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartPlanCompletion;
        private System.Windows.Forms.Panel panel9;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartDeviceStatus;
        private System.Windows.Forms.Panel panel10;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartProductionTrend;
        private System.Windows.Forms.Panel panel11;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartQualityAnalysis;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnProductionMonitor;
        private System.Windows.Forms.Button btnWorkOrderManagement;
        private System.Windows.Forms.Button btnInventoryManagement;
        private System.Windows.Forms.Button btnDeviceManagement;
        private System.Windows.Forms.Button btnQualityControl;
        private System.Windows.Forms.Button btnReportAnalysis;
    }
}