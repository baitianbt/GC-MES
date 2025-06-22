 using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace GC_MES.WinForm.Forms
{
    public partial class DashboardForm : Form
    {
        private readonly ILogger<DashboardForm> _logger;
        private readonly IConfiguration _configuration;
        
        // 模拟数据
        private readonly Random _random = new Random();
        private Timer _refreshTimer;
        
        // 生产计划完成情况
        private int _planTotal = 0;
        private int _planCompleted = 0;
        
        // 设备状态
        private int _deviceTotal = 0;
        private int _deviceRunning = 0;
        private int _deviceIdle = 0;
        private int _deviceMaintenance = 0;
        private int _deviceFault = 0;

        public DashboardForm(ILogger<DashboardForm> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            
            InitializeComponent();
            
            // 初始化图表和数据
            InitializeCharts();
            LoadMockData();
            
            // 设置自动刷新定时器
            _refreshTimer = new Timer();
            _refreshTimer.Interval = 5000; // 5秒刷新一次
            _refreshTimer.Tick += RefreshTimer_Tick;
            _refreshTimer.Start();
            
            // 应用主题
            ThemeManager.ApplyTheme(this);
        }

        private void DashboardForm_Load(object sender, EventArgs e)
        {
            // 更新时间标签
            lblCurrentTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            
            // 更新公司名称
            lblCompanyName.Text = _configuration["CompanyName"] ?? "广创智能制造系统";
        }

        private void InitializeCharts()
        {
            // 初始化生产计划完成情况图表
            chartPlanCompletion.Series.Clear();
            Series planSeries = new Series("计划完成情况");
            planSeries.ChartType = SeriesChartType.Doughnut;
            planSeries["PieLabelStyle"] = "Outside";
            planSeries["PieLineColor"] = "Black";
            planSeries.IsValueShownAsLabel = true;
            planSeries.Label = "#PERCENT{P0}";
            planSeries.LegendText = "#VALX: #VAL";
            chartPlanCompletion.Series.Add(planSeries);
            
            // 设置图表标题
            chartPlanCompletion.Titles.Add(new Title("生产计划完成情况", Docking.Top, new Font("Microsoft YaHei UI", 10, FontStyle.Bold), Color.Black));
            
            // 初始化设备状态图表
            chartDeviceStatus.Series.Clear();
            Series deviceSeries = new Series("设备状态");
            deviceSeries.ChartType = SeriesChartType.Pie;
            deviceSeries["PieLabelStyle"] = "Outside";
            deviceSeries["PieLineColor"] = "Black";
            deviceSeries.IsValueShownAsLabel = true;
            deviceSeries.Label = "#VALX: #VAL";
            deviceSeries.LegendText = "#VALX: #VAL";
            chartDeviceStatus.Series.Add(deviceSeries);
            
            // 设置图表标题
            chartDeviceStatus.Titles.Add(new Title("设备状态分布", Docking.Top, new Font("Microsoft YaHei UI", 10, FontStyle.Bold), Color.Black));
            
            // 初始化产量趋势图表
            chartProductionTrend.Series.Clear();
            Series productionSeries = new Series("日产量");
            productionSeries.ChartType = SeriesChartType.Column;
            productionSeries.IsValueShownAsLabel = true;
            chartProductionTrend.Series.Add(productionSeries);
            
            // 设置图表标题
            chartProductionTrend.Titles.Add(new Title("近7日产量趋势", Docking.Top, new Font("Microsoft YaHei UI", 10, FontStyle.Bold), Color.Black));
            
            // 设置X轴
            chartProductionTrend.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.LightGray;
            chartProductionTrend.ChartAreas[0].AxisX.LabelStyle.Font = new Font("Microsoft YaHei UI", 8);
            
            // 设置Y轴
            chartProductionTrend.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.LightGray;
            chartProductionTrend.ChartAreas[0].AxisY.LabelStyle.Font = new Font("Microsoft YaHei UI", 8);
            
            // 初始化质量分析图表
            chartQualityAnalysis.Series.Clear();
            Series qualitySeries = new Series("良品率");
            qualitySeries.ChartType = SeriesChartType.Line;
            qualitySeries.BorderWidth = 3;
            qualitySeries.MarkerStyle = MarkerStyle.Circle;
            qualitySeries.MarkerSize = 8;
            chartQualityAnalysis.Series.Add(qualitySeries);
            
            // 设置图表标题
            chartQualityAnalysis.Titles.Add(new Title("近7日良品率趋势", Docking.Top, new Font("Microsoft YaHei UI", 10, FontStyle.Bold), Color.Black));
            
            // 设置X轴
            chartQualityAnalysis.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.LightGray;
            chartQualityAnalysis.ChartAreas[0].AxisX.LabelStyle.Font = new Font("Microsoft YaHei UI", 8);
            
            // 设置Y轴
            chartQualityAnalysis.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.LightGray;
            chartQualityAnalysis.ChartAreas[0].AxisY.LabelStyle.Font = new Font("Microsoft YaHei UI", 8);
            chartQualityAnalysis.ChartAreas[0].AxisY.Minimum = 90;
            chartQualityAnalysis.ChartAreas[0].AxisY.Maximum = 100;
        }

        private void LoadMockData()
        {
            try
            {
                // 生成模拟数据
                GenerateMockData();
                
                // 更新生产计划完成情况图表
                chartPlanCompletion.Series[0].Points.Clear();
                DataPoint completedPoint = chartPlanCompletion.Series[0].Points.Add(_planCompleted);
                completedPoint.Color = Color.FromArgb(65, 140, 240);
                completedPoint.AxisLabel = "已完成";
                
                DataPoint remainingPoint = chartPlanCompletion.Series[0].Points.Add(_planTotal - _planCompleted);
                remainingPoint.Color = Color.FromArgb(224, 224, 224);
                remainingPoint.AxisLabel = "未完成";
                
                // 更新设备状态图表
                chartDeviceStatus.Series[0].Points.Clear();
                
                DataPoint runningPoint = chartDeviceStatus.Series[0].Points.Add(_deviceRunning);
                runningPoint.Color = Color.FromArgb(65, 140, 240);
                runningPoint.AxisLabel = "运行中";
                
                DataPoint idlePoint = chartDeviceStatus.Series[0].Points.Add(_deviceIdle);
                idlePoint.Color = Color.FromArgb(252, 180, 65);
                idlePoint.AxisLabel = "空闲";
                
                DataPoint maintenancePoint = chartDeviceStatus.Series[0].Points.Add(_deviceMaintenance);
                maintenancePoint.Color = Color.FromArgb(224, 131, 10);
                maintenancePoint.AxisLabel = "维护中";
                
                DataPoint faultPoint = chartDeviceStatus.Series[0].Points.Add(_deviceFault);
                faultPoint.Color = Color.FromArgb(191, 0, 64);
                faultPoint.AxisLabel = "故障";
                
                // 更新产量趋势图表
                chartProductionTrend.Series[0].Points.Clear();
                DateTime today = DateTime.Today;
                
                for (int i = 6; i >= 0; i--)
                {
                    DateTime date = today.AddDays(-i);
                    int production = _random.Next(800, 1200);
                    DataPoint point = chartProductionTrend.Series[0].Points.Add(production);
                    point.AxisLabel = date.ToString("MM-dd");
                    point.Color = Color.FromArgb(65, 140, 240);
                }
                
                // 更新质量分析图表
                chartQualityAnalysis.Series[0].Points.Clear();
                
                for (int i = 6; i >= 0; i--)
                {
                    DateTime date = today.AddDays(-i);
                    double qualityRate = 90 + _random.NextDouble() * 9.5;
                    DataPoint point = chartQualityAnalysis.Series[0].Points.Add(qualityRate);
                    point.AxisLabel = date.ToString("MM-dd");
                    point.Label = qualityRate.ToString("F1") + "%";
                }
                
                // 更新待处理工单数量
                int pendingOrders = _random.Next(5, 20);
                lblPendingOrders.Text = pendingOrders.ToString();
                
                // 更新库存预警数量
                int inventoryWarnings = _random.Next(0, 8);
                lblInventoryWarnings.Text = inventoryWarnings.ToString();
                
                // 更新设备故障数量
                lblDeviceFaults.Text = _deviceFault.ToString();
                
                // 更新质量异常数量
                int qualityIssues = _random.Next(0, 10);
                lblQualityIssues.Text = qualityIssues.ToString();
                
                // 更新计划完成率
                double completionRate = (double)_planCompleted / _planTotal * 100;
                lblCompletionRate.Text = completionRate.ToString("F1") + "%";
                
                // 更新设备利用率
                double utilizationRate = (double)_deviceRunning / _deviceTotal * 100;
                lblUtilizationRate.Text = utilizationRate.ToString("F1") + "%";
                
                // 更新当前时间
                lblCurrentTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "加载模拟数据时出错");
            }
        }

        private void GenerateMockData()
        {
            // 生产计划数据
            _planTotal = 1000;
            _planCompleted = _random.Next(600, 950);
            
            // 设备状态数据
            _deviceTotal = 50;
            _deviceRunning = _random.Next(30, 45);
            _deviceMaintenance = _random.Next(1, 5);
            _deviceFault = _random.Next(0, 3);
            _deviceIdle = _deviceTotal - _deviceRunning - _deviceMaintenance - _deviceFault;
        }

        private void RefreshTimer_Tick(object sender, EventArgs e)
        {
            // 刷新数据
            LoadMockData();
        }

        private void DashboardForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 停止定时器
            _refreshTimer.Stop();
            _refreshTimer.Dispose();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            // 手动刷新数据
            LoadMockData();
        }
        
        private void btnProductionMonitor_Click(object sender, EventArgs e)
        {
            MessageBox.Show("生产监控功能即将上线，敬请期待！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        
        private void btnWorkOrderManagement_Click(object sender, EventArgs e)
        {
            MessageBox.Show("工单管理功能即将上线，敬请期待！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        
        private void btnInventoryManagement_Click(object sender, EventArgs e)
        {
            MessageBox.Show("库存管理功能即将上线，敬请期待！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        
        private void btnDeviceManagement_Click(object sender, EventArgs e)
        {
            MessageBox.Show("设备管理功能即将上线，敬请期待！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        
        private void btnQualityControl_Click(object sender, EventArgs e)
        {
            MessageBox.Show("质量控制功能即将上线，敬请期待！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        
        private void btnReportAnalysis_Click(object sender, EventArgs e)
        {
            MessageBox.Show("报表分析功能即将上线，敬请期待！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}