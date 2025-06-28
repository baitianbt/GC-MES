using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace GC_MES.WinForm.Controls
{
    /// <summary>
    /// 现代风格的分页控件
    /// </summary>
    public class DPagination : UserControl
    {
        #region 私有变量
        
        private int _currentPage = 1;             // 当前页码
        private int _totalPages = 1;              // 总页数
        private int _pageSize = 20;               // 每页显示数量
        private int _totalItems = 0;              // 总项目数
        private int _displayedPageCount = 5;      // 显示的页码按钮数量
        private bool _showPageSizeSelector = true; // 是否显示页大小选择器
        private bool _showJumpToPage = true;      // 是否显示跳转页面控件
        private int[] _pageSizeOptions = new int[] { 10, 20, 50, 100 }; // 页大小选项
        
        private Button btnFirst;                  // 首页按钮
        private Button btnPrevious;               // 上一页按钮
        private Button btnNext;                   // 下一页按钮
        private Button btnLast;                   // 末页按钮
        private Label lblPageInfo;                // 页码信息标签
        private NumericUpDown nudJumpTo;          // 跳转页面控件
        private Button btnJumpTo;                 // 跳转按钮
        private ComboBox cmbPageSize;             // 页大小选择器
        private Label lblPageSize;                // 页大小标签
        private FlowLayoutPanel panelPages;       // 页码面板
        
        #endregion

        #region 公共属性
        
        /// <summary>
        /// 获取或设置当前页码
        /// </summary>
        [Category("分页")]
        [Description("当前页码")]
        [DefaultValue(1)]
        public int CurrentPage
        {
            get => _currentPage;
            set
            {
                if (value < 1)
                    value = 1;
                else if (value > _totalPages)
                    value = _totalPages;
                
                if (_currentPage != value)
                {
                    _currentPage = value;
                    UpdateControls();
                    OnPageChanged();
                }
            }
        }
        
        /// <summary>
        /// 获取或设置总页数
        /// </summary>
        [Category("分页")]
        [Description("总页数")]
        [DefaultValue(1)]
        public int TotalPages
        {
            get => _totalPages;
            private set
            {
                if (value < 1)
                    value = 1;
                
                if (_totalPages != value)
                {
                    _totalPages = value;
                    
                    // 确保当前页在有效范围内
                    if (_currentPage > _totalPages)
                        _currentPage = _totalPages;
                    
                    UpdateControls();
                }
            }
        }
        
        /// <summary>
        /// 获取或设置每页显示的项数
        /// </summary>
        [Category("分页")]
        [Description("每页显示的项数")]
        [DefaultValue(20)]
        public int PageSize
        {
            get => _pageSize;
            set
            {
                if (value < 1)
                    value = 1;
                
                if (_pageSize != value)
                {
                    _pageSize = value;
                    
                    // 更新总页数
                    CalculateTotalPages();
                    
                    if (cmbPageSize != null)
                        cmbPageSize.Text = value.ToString();
                    
                    OnPageSizeChanged();
                }
            }
        }
        
        /// <summary>
        /// 获取或设置总项目数
        /// </summary>
        [Category("分页")]
        [Description("总项目数")]
        [DefaultValue(0)]
        public int TotalItems
        {
            get => _totalItems;
            set
            {
                if (value < 0)
                    value = 0;
                
                if (_totalItems != value)
                {
                    _totalItems = value;
                    CalculateTotalPages();
                    UpdateControls();
                }
            }
        }
        
        /// <summary>
        /// 获取或设置显示的页码按钮数量
        /// </summary>
        [Category("外观")]
        [Description("显示的页码按钮数量")]
        [DefaultValue(5)]
        public int DisplayedPageCount
        {
            get => _displayedPageCount;
            set
            {
                if (value < 1)
                    value = 1;
                
                if (_displayedPageCount != value)
                {
                    _displayedPageCount = value;
                    UpdateControls();
                }
            }
        }
        
        /// <summary>
        /// 获取或设置是否显示页大小选择器
        /// </summary>
        [Category("外观")]
        [Description("是否显示页大小选择器")]
        [DefaultValue(true)]
        public bool ShowPageSizeSelector
        {
            get => _showPageSizeSelector;
            set
            {
                if (_showPageSizeSelector != value)
                {
                    _showPageSizeSelector = value;
                    
                    if (cmbPageSize != null && lblPageSize != null)
                    {
                        cmbPageSize.Visible = value;
                        lblPageSize.Visible = value;
                    }
                }
            }
        }
        
        /// <summary>
        /// 获取或设置是否显示跳转页面控件
        /// </summary>
        [Category("外观")]
        [Description("是否显示跳转页面控件")]
        [DefaultValue(true)]
        public bool ShowJumpToPage
        {
            get => _showJumpToPage;
            set
            {
                if (_showJumpToPage != value)
                {
                    _showJumpToPage = value;
                    
                    if (nudJumpTo != null && btnJumpTo != null)
                    {
                        nudJumpTo.Visible = value;
                        btnJumpTo.Visible = value;
                    }
                }
            }
        }
        
        /// <summary>
        /// 获取或设置页大小选项
        /// </summary>
        [Category("分页")]
        [Description("页大小选项")]
        public int[] PageSizeOptions
        {
            get => _pageSizeOptions;
            set
            {
                if (value != null && value.Length > 0)
                {
                    _pageSizeOptions = value;
                    
                    if (cmbPageSize != null)
                    {
                        cmbPageSize.Items.Clear();
                        foreach (int size in value)
                            cmbPageSize.Items.Add(size.ToString());
                    }
                }
            }
        }
        
        /// <summary>
        /// 获取或设置按钮的前景色
        /// </summary>
        [Category("外观")]
        [Description("按钮的前景色")]
        public Color ButtonForeColor { get; set; } = Color.White;
        
        /// <summary>
        /// 获取或设置按钮的背景色
        /// </summary>
        [Category("外观")]
        [Description("按钮的背景色")]
        public Color ButtonBackColor { get; set; } = Color.FromArgb(0, 122, 204);
        
        /// <summary>
        /// 获取或设置当前页按钮的背景色
        /// </summary>
        [Category("外观")]
        [Description("当前页按钮的背景色")]
        public Color CurrentPageBackColor { get; set; } = Color.FromArgb(0, 99, 177);
        
        #endregion

        #region 事件

        /// <summary>
        /// 页面改变时触发的事件
        /// </summary>
        [Category("Action")]
        [Description("页面改变时触发的事件")]
        public event EventHandler PageChanged;
        
        /// <summary>
        /// 页大小改变时触发的事件
        /// </summary>
        [Category("Action")]
        [Description("页大小改变时触发的事件")]
        public event EventHandler PageSizeChanged;
        
        #endregion

        #region 构造函数
        
        /// <summary>
        /// 创建分页控件实例
        /// </summary>
        public DPagination()
        {
            InitializeComponent();
        }
        
        #endregion

        #region 初始化

        private void InitializeComponent()
        {
            // 设置控件属性
            Size = new Size(700, 40);
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            BackColor = Color.White;
            Font = new Font("Microsoft YaHei UI", 9F);
            Padding = new Padding(5);
            
            // 创建控件
            btnFirst = new Button();
            btnPrevious = new Button();
            btnNext = new Button();
            btnLast = new Button();
            lblPageInfo = new Label();
            nudJumpTo = new NumericUpDown();
            btnJumpTo = new Button();
            cmbPageSize = new ComboBox();
            lblPageSize = new Label();
            panelPages = new FlowLayoutPanel();
            
            // 控制控件
            ((ISupportInitialize)nudJumpTo).BeginInit();

            // 添加控件到容器
            Controls.Add(btnFirst);
            Controls.Add(btnPrevious);
            Controls.Add(panelPages);
            Controls.Add(btnNext);
            Controls.Add(btnLast);
            Controls.Add(lblPageInfo);
            Controls.Add(nudJumpTo);
            Controls.Add(btnJumpTo);
            Controls.Add(cmbPageSize);
            Controls.Add(lblPageSize);
            
            // 首页按钮
            btnFirst.Size = new Size(40, 30);
            btnFirst.FlatStyle = FlatStyle.Flat;
            btnFirst.FlatAppearance.BorderSize = 0;
            btnFirst.BackColor = ButtonBackColor;
            btnFirst.ForeColor = ButtonForeColor;
            btnFirst.Text = "<<";
            btnFirst.Location = new Point(5, 5);
            btnFirst.Cursor = Cursors.Hand;
            btnFirst.Click += BtnFirst_Click;
            
            // 上一页按钮
            btnPrevious.Size = new Size(40, 30);
            btnPrevious.FlatStyle = FlatStyle.Flat;
            btnPrevious.FlatAppearance.BorderSize = 0;
            btnPrevious.BackColor = ButtonBackColor;
            btnPrevious.ForeColor = ButtonForeColor;
            btnPrevious.Text = "<";
            btnPrevious.Location = new Point(50, 5);
            btnPrevious.Cursor = Cursors.Hand;
            btnPrevious.Click += BtnPrevious_Click;
            
            // 页码面板
            panelPages.Size = new Size(200, 30);
            panelPages.Location = new Point(95, 5);
            panelPages.AutoSize = true;
            panelPages.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            panelPages.WrapContents = false;
            
            // 下一页按钮
            btnNext.Size = new Size(40, 30);
            btnNext.FlatStyle = FlatStyle.Flat;
            btnNext.FlatAppearance.BorderSize = 0;
            btnNext.BackColor = ButtonBackColor;
            btnNext.ForeColor = ButtonForeColor;
            btnNext.Text = ">";
            btnNext.Location = new Point(300, 5);
            btnNext.Cursor = Cursors.Hand;
            btnNext.Click += BtnNext_Click;
            
            // 末页按钮
            btnLast.Size = new Size(40, 30);
            btnLast.FlatStyle = FlatStyle.Flat;
            btnLast.FlatAppearance.BorderSize = 0;
            btnLast.BackColor = ButtonBackColor;
            btnLast.ForeColor = ButtonForeColor;
            btnLast.Text = ">>";
            btnLast.Location = new Point(345, 5);
            btnLast.Cursor = Cursors.Hand;
            btnLast.Click += BtnLast_Click;
            
            // 页码信息标签
            lblPageInfo.Size = new Size(120, 30);
            lblPageInfo.Location = new Point(390, 5);
            lblPageInfo.TextAlign = ContentAlignment.MiddleLeft;
            lblPageInfo.Text = "1/1 共0条";
            
            // 跳转页面控件
            nudJumpTo.Size = new Size(60, 30);
            nudJumpTo.Location = new Point(515, 5);
            nudJumpTo.Minimum = 1;
            nudJumpTo.Maximum = 9999;
            nudJumpTo.Value = 1;
            nudJumpTo.TextAlign = HorizontalAlignment.Center;
            
            // 跳转按钮
            btnJumpTo.Size = new Size(40, 30);
            btnJumpTo.FlatStyle = FlatStyle.Flat;
            btnJumpTo.FlatAppearance.BorderSize = 0;
            btnJumpTo.BackColor = ButtonBackColor;
            btnJumpTo.ForeColor = ButtonForeColor;
            btnJumpTo.Text = "跳转";
            btnJumpTo.Location = new Point(580, 5);
            btnJumpTo.Cursor = Cursors.Hand;
            btnJumpTo.Click += BtnJumpTo_Click;
            
            // 页大小标签
            lblPageSize.Size = new Size(50, 30);
            lblPageSize.Location = new Point(625, 5);
            lblPageSize.TextAlign = ContentAlignment.MiddleRight;
            lblPageSize.Text = "每页:";
            
            // 页大小选择器
            cmbPageSize.Size = new Size(60, 30);
            cmbPageSize.Location = new Point(680, 5);
            cmbPageSize.DropDownStyle = ComboBoxStyle.DropDownList;
            foreach (int size in PageSizeOptions)
                cmbPageSize.Items.Add(size.ToString());
            cmbPageSize.SelectedIndex = Array.IndexOf(PageSizeOptions, PageSize);
            cmbPageSize.SelectedIndexChanged += CmbPageSize_SelectedIndexChanged;
            
            // 结束控件初始化
            ((ISupportInitialize)nudJumpTo).EndInit();
            
            // 调整控件位置
            LayoutControls();
            
            // 初始化页码按钮
            UpdateControls();
        }
        
        private void LayoutControls()
        {
            int x = Padding.Left;
            
            btnFirst.Location = new Point(x, Padding.Top);
            x += btnFirst.Width + 5;
            
            btnPrevious.Location = new Point(x, Padding.Top);
            x += btnPrevious.Width + 5;
            
            panelPages.Location = new Point(x, Padding.Top);
            x += panelPages.Width + 5;
            
            btnNext.Location = new Point(x, Padding.Top);
            x += btnNext.Width + 5;
            
            btnLast.Location = new Point(x, Padding.Top);
            x += btnLast.Width + 10;
            
            lblPageInfo.Location = new Point(x, Padding.Top);
            x += lblPageInfo.Width + 10;
            
            if (ShowJumpToPage)
            {
                nudJumpTo.Location = new Point(x, Padding.Top);
                x += nudJumpTo.Width + 5;
                
                btnJumpTo.Location = new Point(x, Padding.Top);
                x += btnJumpTo.Width + 10;
            }
            else
            {
                nudJumpTo.Visible = false;
                btnJumpTo.Visible = false;
            }
            
            if (ShowPageSizeSelector)
            {
                lblPageSize.Location = new Point(x, Padding.Top);
                x += lblPageSize.Width;
                
                cmbPageSize.Location = new Point(x, Padding.Top);
            }
            else
            {
                lblPageSize.Visible = false;
                cmbPageSize.Visible = false;
            }
        }
        
        #endregion

        #region 事件处理
        
        private void BtnFirst_Click(object sender, EventArgs e)
        {
            CurrentPage = 1;
        }
        
        private void BtnPrevious_Click(object sender, EventArgs e)
        {
            if (CurrentPage > 1)
                CurrentPage--;
        }
        
        private void BtnNext_Click(object sender, EventArgs e)
        {
            if (CurrentPage < TotalPages)
                CurrentPage++;
        }
        
        private void BtnLast_Click(object sender, EventArgs e)
        {
            CurrentPage = TotalPages;
        }
        
        private void PageButton_Click(object sender, EventArgs e)
        {
            if (sender is Button button && int.TryParse(button.Tag.ToString(), out int page))
                CurrentPage = page;
        }
        
        private void BtnJumpTo_Click(object sender, EventArgs e)
        {
            int page = (int)nudJumpTo.Value;
            CurrentPage = page;
        }
        
        private void CmbPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbPageSize.SelectedItem != null && int.TryParse(cmbPageSize.SelectedItem.ToString(), out int pageSize))
                PageSize = pageSize;
        }
        
        private void OnPageChanged()
        {
            PageChanged?.Invoke(this, EventArgs.Empty);
        }
        
        private void OnPageSizeChanged()
        {
            PageSizeChanged?.Invoke(this, EventArgs.Empty);
        }
        
        #endregion

        #region 更新控件
        
        /// <summary>
        /// 计算总页数
        /// </summary>
        private void CalculateTotalPages()
        {
            if (_totalItems <= 0 || _pageSize <= 0)
                TotalPages = 1;
            else
                TotalPages = (_totalItems + _pageSize - 1) / _pageSize;
        }
        
        /// <summary>
        /// 更新所有控件状态
        /// </summary>
        private void UpdateControls()
        {
            // 更新页码信息
            lblPageInfo.Text = $"{CurrentPage}/{TotalPages} 共{TotalItems}条";
            
            // 更新按钮状态
            btnFirst.Enabled = btnPrevious.Enabled = CurrentPage > 1;
            btnNext.Enabled = btnLast.Enabled = CurrentPage < TotalPages;
            
            // 更新跳转页面控件
            nudJumpTo.Maximum = TotalPages;
            nudJumpTo.Value = CurrentPage;
            
            // 更新页码按钮
            UpdatePageButtons();
        }
        
        /// <summary>
        /// 更新页码按钮
        /// </summary>
        private void UpdatePageButtons()
        {
            panelPages.Controls.Clear();
            
            if (TotalPages <= 1)
                return;
            
            int halfCount = _displayedPageCount / 2;
            int startPage = Math.Max(1, CurrentPage - halfCount);
            int endPage = Math.Min(TotalPages, startPage + _displayedPageCount - 1);
            
            if (endPage - startPage + 1 < _displayedPageCount)
                startPage = Math.Max(1, endPage - _displayedPageCount + 1);
            
            // 添加"1"和省略号
            if (startPage > 1)
            {
                AddPageButton(1);
                
                if (startPage > 2)
                    AddEllipsisButton();
            }
            
            // 添加中间页码
            for (int i = startPage; i <= endPage; i++)
                AddPageButton(i);
            
            // 添加省略号和最后一页
            if (endPage < TotalPages)
            {
                if (endPage < TotalPages - 1)
                    AddEllipsisButton();
                
                AddPageButton(TotalPages);
            }
            
            // 调整控件位置
            LayoutControls();
        }
        
        /// <summary>
        /// 添加页码按钮
        /// </summary>
        private void AddPageButton(int page)
        {
            Button btn = new Button();
            btn.Size = new Size(30, 30);
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Text = page.ToString();
            btn.Tag = page;
            btn.Cursor = Cursors.Hand;
            btn.Margin = new Padding(0, 0, 5, 0);
            
            if (page == CurrentPage)
            {
                btn.BackColor = CurrentPageBackColor;
                btn.Font = new Font(btn.Font, FontStyle.Bold);
            }
            else
            {
                btn.BackColor = ButtonBackColor;
            }
            btn.ForeColor = ButtonForeColor;
            
            btn.Click += PageButton_Click;
            
            panelPages.Controls.Add(btn);
        }
        
        /// <summary>
        /// 添加省略号按钮
        /// </summary>
        private void AddEllipsisButton()
        {
            Label lbl = new Label();
            lbl.Size = new Size(20, 30);
            lbl.TextAlign = ContentAlignment.MiddleCenter;
            lbl.Text = "...";
            lbl.Margin = new Padding(0, 0, 5, 0);
            
            panelPages.Controls.Add(lbl);
        }
        
        #endregion
        
        #region 外部方法
        
        /// <summary>
        /// 重置分页控件
        /// </summary>
        public void Reset(int totalItems, int pageSize = 0)
        {
            if (pageSize > 0)
                PageSize = pageSize;
            
            TotalItems = totalItems;
            CurrentPage = 1;
        }
        
        #endregion
    }
} 