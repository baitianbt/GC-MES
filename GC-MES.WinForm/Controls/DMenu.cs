using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace GC_MES.WinForm.Controls
{
    /// <summary>
    /// 现代化风格的菜单控件
    /// </summary>
    public class DMenu : Control
    {
        #region 私有变量

        // 菜单项列表
        private readonly List<DMenuItem> _items = new List<DMenuItem>();
        
        // 当前活动的菜单项索引
        private int _activeIndex = -1;
        
        // 当前打开的子菜单
        private DSubMenu? _activeSubMenu = null;
        
        // 菜单项高度
        private int _itemHeight = 40;
        
        // 子菜单是否打开
        private bool _isSubMenuOpen = false;
        
        // 是否只在点击时显示子菜单
        private bool _showSubMenuOnClick = true;

        #endregion

        #region 公共属性

        /// <summary>
        /// 菜单项集合
        /// </summary>
        [Browsable(false)]
        public IReadOnlyList<DMenuItem> Items => _items;

        /// <summary>
        /// 菜单项高度
        /// </summary>
        [Category("外观")]
        [Description("菜单项的高度")]
        [DefaultValue(40)]
        public int ItemHeight
        {
            get => _itemHeight;
            set
            {
                if (_itemHeight != value && value > 0)
                {
                    _itemHeight = value;
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// 菜单背景色
        /// </summary>
        [Category("外观")]
        [Description("菜单背景色")]
        public Color MenuBackColor { get; set; } = Color.FromArgb(45, 45, 48);

        /// <summary>
        /// 菜单项选中色
        /// </summary>
        [Category("外观")]
        [Description("菜单项选中色")]
        public Color MenuItemSelectedColor { get; set; } = Color.FromArgb(62, 62, 64);

        /// <summary>
        /// 菜单项悬停色
        /// </summary>
        [Category("外观")]
        [Description("菜单项悬停色")]
        public Color MenuItemHoverColor { get; set; } = Color.FromArgb(55, 55, 58);

        /// <summary>
        /// 菜单文本色
        /// </summary>
        [Category("外观")]
        [Description("菜单文本色")]
        public Color MenuTextColor { get; set; } = Color.White;

        /// <summary>
        /// 图标和文本之间的间距
        /// </summary>
        [Category("外观")]
        [Description("图标和文本之间的间距")]
        [DefaultValue(10)]
        public int IconTextSpacing { get; set; } = 10;

        /// <summary>
        /// 图标大小
        /// </summary>
        [Category("外观")]
        [Description("图标大小")]
        public Size IconSize { get; set; } = new Size(24, 24);

        /// <summary>
        /// 图标左边距
        /// </summary>
        [Category("外观")]
        [Description("图标左边距")]
        [DefaultValue(10)]
        public int IconLeftMargin { get; set; } = 10;

        /// <summary>
        /// 是否显示子菜单指示器
        /// </summary>
        [Category("外观")]
        [Description("是否显示子菜单指示器")]
        [DefaultValue(true)]
        public bool ShowSubMenuIndicator { get; set; } = true;

        /// <summary>
        /// 子菜单指示器颜色
        /// </summary>
        [Category("外观")]
        [Description("子菜单指示器颜色")]
        public Color SubMenuIndicatorColor { get; set; } = Color.LightGray;
        
        /// <summary>
        /// 是否只在点击时显示子菜单（否则在鼠标悬停时显示）
        /// </summary>
        [Category("行为")]
        [Description("是否只在点击时显示子菜单")]
        [DefaultValue(true)]
        public bool ShowSubMenuOnClick 
        { 
            get => _showSubMenuOnClick;
            set => _showSubMenuOnClick = value;
        }

        #endregion

        #region 构造函数

        public DMenu()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw, true);
            
            BackColor = MenuBackColor;
            ForeColor = MenuTextColor;
            Font = new Font("Microsoft YaHei UI", 9F);
        }

        #endregion

        #region 菜单项管理方法

        /// <summary>
        /// 添加菜单项
        /// </summary>
        public DMenuItem AddItem(DMenuItem item)
        {
            _items.Add(item);
            Invalidate();
            return item;
        }

        /// <summary>
        /// 添加菜单项
        /// </summary>
        public DMenuItem AddItem(string text)
        {
            var item = new DMenuItem(text);
            _items.Add(item);
            Invalidate();
            return item;
        }

        /// <summary>
        /// 添加菜单项
        /// </summary>
        public DMenuItem AddItem(string text, Image icon)
        {
            var item = new DMenuItem(text, icon);
            _items.Add(item);
            Invalidate();
            return item;
        }

        /// <summary>
        /// 添加菜单项
        /// </summary>
        public DMenuItem AddItem(string text, Image icon, EventHandler clickHandler)
        {
            var item = new DMenuItem(text, icon, clickHandler);
            _items.Add(item);
            Invalidate();
            return item;
        }

        /// <summary>
        /// 清空菜单项
        /// </summary>
        public void ClearItems()
        {
            _items.Clear();
            Invalidate();
        }

        #endregion

        #region 绘制方法

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // 绘制菜单背景
            using (var brush = new SolidBrush(MenuBackColor))
            {
                g.FillRectangle(brush, ClientRectangle);
            }

            // 绘制菜单项
            int y = 0;
            for (int i = 0; i < _items.Count; i++)
            {
                if (_items[i].Visible)
                {
                    DrawMenuItem(g, _items[i], i, new Rectangle(0, y, Width, ItemHeight));
                    y += ItemHeight;
                }
            }
        }

        private void DrawMenuItem(Graphics g, DMenuItem item, int index, Rectangle rect)
        {
            // 绘制菜单项背景
            using (var brush = new SolidBrush(index == _activeIndex ? MenuItemSelectedColor : MenuBackColor))
            {
                g.FillRectangle(brush, rect);
            }

            // 绘制图标
            if (item.Icon != null)
            {
                var iconRect = new Rectangle(
                    rect.Left + IconLeftMargin,
                    rect.Top + (rect.Height - IconSize.Height) / 2,
                    IconSize.Width,
                    IconSize.Height);
                g.DrawImage(item.Icon, iconRect);
            }

            // 绘制文本
            using (var brush = new SolidBrush(MenuTextColor))
            {
                var iconOffset = item.Icon != null ? IconLeftMargin + IconSize.Width + IconTextSpacing : IconLeftMargin;
                var textRect = new Rectangle(
                    rect.Left + iconOffset,
                    rect.Top,
                    rect.Width - iconOffset - 20,
                    rect.Height);
                
                var format = new StringFormat
                {
                    LineAlignment = StringAlignment.Center,
                    Alignment = StringAlignment.Near,
                    Trimming = StringTrimming.EllipsisCharacter
                };
                
                g.DrawString(item.Text, Font, brush, textRect, format);
            }

            // 绘制子菜单指示器
            if (ShowSubMenuIndicator && item.HasSubItems)
            {
                using (var pen = new Pen(SubMenuIndicatorColor, 1.5f))
                {
                    int arrowSize = 8;
                    int arrowX = rect.Right - arrowSize - 10;
                    int arrowY = rect.Top + (rect.Height - arrowSize) / 2;
                    
                    Point[] points = new Point[]
                    {
                        new Point(arrowX, arrowY),
                        new Point(arrowX + arrowSize, arrowY + arrowSize / 2),
                        new Point(arrowX, arrowY + arrowSize)
                    };
                    
                    g.DrawLines(pen, points);
                }
            }
        }

        #endregion

        #region 鼠标事件处理

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            int index = GetItemIndexAtPoint(e.Location);
            
            if (index != _activeIndex)
            {
                _activeIndex = index;
                
                // 如果不是只在点击时显示子菜单，则在鼠标悬停时显示
                if (!_showSubMenuOnClick)
                {
                    // 关闭之前的子菜单
                    CloseSubMenu();
                    
                    // 如果鼠标在菜单项上并且该菜单项有子菜单，则显示子菜单
                    if (_activeIndex >= 0 && _items[_activeIndex].HasSubItems)
                    {
                        ShowSubMenu(_activeIndex);
                    }
                }
                
                Invalidate();
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            int index = GetItemIndexAtPoint(e.Location);
            
            if (index >= 0)
            {
                var item = _items[index];
                
                // 如果菜单项没有子菜单，则触发点击事件
                if (!item.HasSubItems)
                {
                    item.OnClick(this, EventArgs.Empty);
                }
                // 如果菜单项有子菜单，则显示或隐藏子菜单
                else
                {
                    if (_isSubMenuOpen && _activeIndex == index)
                    {
                        CloseSubMenu();
                    }
                    else
                    {
                        // 如果已有其他菜单打开，先关闭它
                        if (_isSubMenuOpen)
                        {
                            CloseSubMenu();
                            // 短暂延迟以确保旧菜单关闭后再打开新菜单
                            Application.DoEvents();
                        }
                        ShowSubMenu(index);
                    }
                }
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            
            // 如果鼠标离开菜单区域但子菜单打开，则不重置活动索引
            if (!_isSubMenuOpen)
            {
                _activeIndex = -1;
                Invalidate();
            }
        }

        #endregion

        #region 子菜单处理

        private void ShowSubMenu(int index)
        {
            if (index < 0 || index >= _items.Count || !_items[index].HasSubItems)
                return;
                
            // 计算子菜单位置
            var item = _items[index];
            var itemRect = GetItemRectangle(index);
            
            // 获取窗体和屏幕信息
            Form? parentForm = FindForm();
            if (parentForm == null) return;
            
            // 创建子菜单
            _activeSubMenu = new DSubMenu(item.SubItems, this)
            {
                Font = Font,
                MenuBackColor = MenuBackColor,
                MenuItemSelectedColor = MenuItemSelectedColor,
                MenuItemHoverColor = MenuItemHoverColor,
                MenuTextColor = MenuTextColor,
                IconTextSpacing = IconTextSpacing,
                IconSize = IconSize,
                IconLeftMargin = IconLeftMargin,
                ShowSubMenuIndicator = ShowSubMenuIndicator,
                SubMenuIndicatorColor = SubMenuIndicatorColor,
                ItemHeight = ItemHeight,
                ShowSubMenuOnClick = ShowSubMenuOnClick
            };
            
            // 确保子菜单被创建后立即显示
            _isSubMenuOpen = true;
            _activeIndex = index;
            Invalidate();

            // 将菜单控件坐标转换为屏幕坐标
            Point menuPoint = PointToScreen(new Point(Width, itemRect.Top));
            
            // 设置子菜单位置
            _activeSubMenu.Owner = parentForm;
            _activeSubMenu.StartPosition = FormStartPosition.Manual;
            _activeSubMenu.Show(parentForm);
            _activeSubMenu.Location = menuPoint;
            _activeSubMenu.Focus();
            
            // 确保子菜单在屏幕范围内
            var screen = Screen.FromControl(this);
            
            // 检查右侧空间
            if (_activeSubMenu.Right > screen.WorkingArea.Right)
            {
                // 如果右侧空间不足，则显示在左侧
                Point leftPoint = PointToScreen(new Point(0, itemRect.Top));
                leftPoint.X -= _activeSubMenu.Width;
                _activeSubMenu.Location = leftPoint;
            }
            
            // 检查底部空间
            if (_activeSubMenu.Bottom > screen.WorkingArea.Bottom)
            {
                // 如果底部空间不足，则向上调整
                int newY = _activeSubMenu.Top - (_activeSubMenu.Height - itemRect.Height);
                if (newY < screen.WorkingArea.Top) newY = screen.WorkingArea.Top;
                _activeSubMenu.Location = new Point(_activeSubMenu.Left, newY);
            }
            
            _activeSubMenu.SubMenuClosed += (s, e) => 
            {
                _isSubMenuOpen = false;
                _activeSubMenu = null;
                
                // 确保主菜单获得焦点
                Focus();
            };
        }

        private void CloseSubMenu()
        {
            if (_activeSubMenu != null)
            {
                _activeSubMenu.Close();
                _activeSubMenu = null;
                _isSubMenuOpen = false;
            }
        }

        #endregion

        #region 辅助方法

        private Rectangle GetItemRectangle(int index)
        {
            if (index < 0 || index >= _items.Count)
                return Rectangle.Empty;
            
            int y = 0;
            for (int i = 0; i < index; i++)
            {
                if (_items[i].Visible)
                {
                    y += ItemHeight;
                }
            }
            
            return new Rectangle(0, y, Width, ItemHeight);
        }

        private int GetItemIndexAtPoint(Point point)
        {
            if (_items.Count == 0)
                return -1;
                
            int y = 0;
            for (int i = 0; i < _items.Count; i++)
            {
                if (_items[i].Visible)
                {
                    Rectangle rect = new Rectangle(0, y, Width, ItemHeight);
                    if (rect.Contains(point) && _items[i].Enabled)
                    {
                        return i;
                    }
                    y += ItemHeight;
                }
            }
            
            return -1;
        }

        #endregion
    }

    /// <summary>
    /// 子菜单窗体
    /// </summary>
    public class DSubMenu : Form
    {
        #region 私有变量

        private readonly List<DMenuItem> _items;
        private int _activeIndex = -1;
        private DSubMenu? _activeSubMenu = null;
        private bool _isSubMenuOpen = false;
        private readonly DMenu _parentMenu;
        private bool _showSubMenuOnClick = true;

        #endregion

        #region 公共属性

        public Color MenuBackColor { get; set; } = Color.FromArgb(45, 45, 48);
        public Color MenuItemSelectedColor { get; set; } = Color.FromArgb(62, 62, 64);
        public Color MenuItemHoverColor { get; set; } = Color.FromArgb(55, 55, 58);
        public Color MenuTextColor { get; set; } = Color.White;
        public int IconTextSpacing { get; set; } = 10;
        public Size IconSize { get; set; } = new Size(24, 24);
        public int IconLeftMargin { get; set; } = 10;
        public bool ShowSubMenuIndicator { get; set; } = true;
        public Color SubMenuIndicatorColor { get; set; } = Color.LightGray;
        public int ItemHeight { get; set; } = 40;
        public bool ShowSubMenuOnClick { get; set; } = true;

        #endregion

        #region 事件

        public event EventHandler? SubMenuClosed;

        #endregion

        #region 构造函数

        public DSubMenu(IReadOnlyList<DMenuItem> items, DMenu parentMenu)
        {
            _items = new List<DMenuItem>(items);
            _parentMenu = parentMenu;
            
            FormBorderStyle = FormBorderStyle.None;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.Manual;
            AutoSize = false;
            ControlBox = false;
            MaximizeBox = false;
            MinimizeBox = false;
            TopMost = true;
            
            // 修复高度计算，确保所有项都完全显示，添加额外的底部边距
            int visibleItems = _items.Count(i => i.Visible);
            int height = visibleItems * ItemHeight + 4; // 添加4个像素的额外边距
            Size = new Size(Math.Max(200, 150), height > 0 ? height : ItemHeight);
            
            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw, true);
        }

        #endregion

        #region 绘制方法

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // 绘制菜单背景
            using (var brush = new SolidBrush(MenuBackColor))
            {
                g.FillRectangle(brush, ClientRectangle);
            }

            // 绘制边框
            using (var pen = new Pen(Color.FromArgb(28, 28, 28), 1))
            {
                g.DrawRectangle(pen, 0, 0, Width - 1, Height - 1);
            }

            // 绘制菜单项
            int y = 0;
            for (int i = 0; i < _items.Count; i++)
            {
                if (_items[i].Visible)
                {
                    DrawMenuItem(g, _items[i], i, new Rectangle(0, y, Width, ItemHeight));
                    y += ItemHeight;
                }
            }
        }

        private void DrawMenuItem(Graphics g, DMenuItem item, int index, Rectangle rect)
        {
            // 绘制菜单项背景
            using (var brush = new SolidBrush(index == _activeIndex ? MenuItemSelectedColor : MenuBackColor))
            {
                g.FillRectangle(brush, rect);
            }

            // 绘制图标
            if (item.Icon != null)
            {
                var iconRect = new Rectangle(
                    rect.Left + IconLeftMargin,
                    rect.Top + (rect.Height - IconSize.Height) / 2,
                    IconSize.Width,
                    IconSize.Height);
                g.DrawImage(item.Icon, iconRect);
            }

            // 绘制文本
            using (var brush = new SolidBrush(MenuTextColor))
            {
                var iconOffset = item.Icon != null ? IconLeftMargin + IconSize.Width + IconTextSpacing : IconLeftMargin;
                var textRect = new Rectangle(
                    rect.Left + iconOffset,
                    rect.Top,
                    rect.Width - iconOffset - 20,
                    rect.Height);
                
                var format = new StringFormat
                {
                    LineAlignment = StringAlignment.Center,
                    Alignment = StringAlignment.Near,
                    Trimming = StringTrimming.EllipsisCharacter
                };
                
                g.DrawString(item.Text, Font, brush, textRect, format);
            }

            // 绘制子菜单指示器
            if (ShowSubMenuIndicator && item.HasSubItems)
            {
                using (var pen = new Pen(SubMenuIndicatorColor, 1.5f))
                {
                    int arrowSize = 8;
                    int arrowX = rect.Right - arrowSize - 10;
                    int arrowY = rect.Top + (rect.Height - arrowSize) / 2;
                    
                    Point[] points = new Point[]
                    {
                        new Point(arrowX, arrowY),
                        new Point(arrowX + arrowSize, arrowY + arrowSize / 2),
                        new Point(arrowX, arrowY + arrowSize)
                    };
                    
                    g.DrawLines(pen, points);
                }
            }
        }

        #endregion

        #region 鼠标事件处理

        // 修复WndProc不要将客户区域设置为标题栏
        protected override void WndProc(ref Message m)
        {
            const int WM_NCHITTEST = 0x0084;
            const int HTTRANSPARENT = -1;
            
            // 处理点击事件，确保点击不会穿透
            if (m.Msg == WM_NCHITTEST)
            {
                base.WndProc(ref m);
                
                // 如果点击在子菜单上，确保它不会穿透
                Point pos = PointToClient(new Point(m.LParam.ToInt32() & 0xffff, m.LParam.ToInt32() >> 16));
                
                if (!ClientRectangle.Contains(pos))
                {
                    // 如果点击在菜单窗体外部，则直接关闭菜单
                    CloseAll();
                    return;
                }
                
                return;
            }
            
            base.WndProc(ref m);
        }
        
        // 点击外部时关闭所有菜单
        private void CloseAll()
        {
            // 关闭所有子菜单
            CloseSubMenu();
            
            // 触发关闭事件
            SubMenuClosed?.Invoke(this, EventArgs.Empty);
            
            // 关闭当前菜单
            Close();
            
            // 递归关闭所有父级菜单
            Form? parentForm = Owner;
            while (parentForm != null && parentForm is DSubMenu parentSubMenu)
            {
                Form? nextParent = parentSubMenu.Owner;
                parentSubMenu.Close();
                parentForm = nextParent;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            int index = GetItemIndexAtPoint(e.Location);
            
            if (index != _activeIndex)
            {
                _activeIndex = index;
                
                // 如果不是只在点击时显示子菜单，则在鼠标悬停时显示
                if (!_showSubMenuOnClick)
                {
                    // 关闭之前的子菜单
                    CloseSubMenu();
                    
                    // 如果鼠标在菜单项上并且该菜单项有子菜单，则显示子菜单
                    if (_activeIndex >= 0 && _items[_activeIndex].HasSubItems)
                    {
                        ShowSubMenu(_activeIndex);
                    }
                }
                
                Invalidate();
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            int index = GetItemIndexAtPoint(e.Location);
            
            if (index >= 0)
            {
                var item = _items[index];
                
                // 如果菜单项没有子菜单，则触发点击事件并关闭所有菜单
                if (!item.HasSubItems)
                {
                    // 先关闭子菜单
                    CloseSubMenu();
                    
                    // 再触发点击事件
                    item.OnClick(this, EventArgs.Empty);
                    
                    // 关闭菜单层次结构
                    CloseAll();
                }
                else // 如果菜单项有子菜单，则显示或隐藏子菜单
                {
                    if (_isSubMenuOpen && _activeIndex == index)
                    {
                        CloseSubMenu();
                    }
                    else
                    {
                        // 如果已有其他菜单打开，先关闭它
                        if (_isSubMenuOpen)
                        {
                            CloseSubMenu();
                            // 短暂延迟以确保旧菜单关闭后再打开新菜单
                            Application.DoEvents();
                        }
                        ShowSubMenu(index);
                    }
                }
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            
            // 如果没有子菜单打开，则重置活动索引
            if (!_isSubMenuOpen)
            {
                _activeIndex = -1;
                Invalidate();
            }
        }

        #endregion

        #region 子菜单处理

        private void ShowSubMenu(int index)
        {
            if (index < 0 || index >= _items.Count || !_items[index].HasSubItems)
                return;
                
            // 计算子菜单位置
            var item = _items[index];
            var itemRect = GetItemRectangle(index);
            
            // 获取父窗体
            Form? parentForm = Owner;
            if (parentForm == null) return;
            
            // 创建子菜单
            _activeSubMenu = new DSubMenu(item.SubItems, _parentMenu)
            {
                Font = Font,
                MenuBackColor = MenuBackColor,
                MenuItemSelectedColor = MenuItemSelectedColor,
                MenuItemHoverColor = MenuItemHoverColor,
                MenuTextColor = MenuTextColor,
                IconTextSpacing = IconTextSpacing,
                IconSize = IconSize,
                IconLeftMargin = IconLeftMargin,
                ShowSubMenuIndicator = ShowSubMenuIndicator,
                SubMenuIndicatorColor = SubMenuIndicatorColor,
                ItemHeight = ItemHeight,
                ShowSubMenuOnClick = ShowSubMenuOnClick
            };
            
            // 确保子菜单被创建后立即显示
            _isSubMenuOpen = true;
            _activeIndex = index;
            Invalidate();
            
            // 将菜单控件坐标转换为屏幕坐标
            Point menuPoint = PointToScreen(new Point(Width, itemRect.Top));
            
            // 设置子菜单位置
            _activeSubMenu.Owner = parentForm;
            _activeSubMenu.StartPosition = FormStartPosition.Manual;
            _activeSubMenu.Show(parentForm);
            _activeSubMenu.Location = menuPoint;
            _activeSubMenu.Focus();
            
            // 确保子菜单在屏幕范围内
            var screen = Screen.FromControl(this);
            
            // 检查右侧空间
            if (_activeSubMenu.Right > screen.WorkingArea.Right)
            {
                // 如果右侧空间不足，则显示在左侧
                Point leftPoint = PointToScreen(new Point(0, itemRect.Top));
                leftPoint.X -= _activeSubMenu.Width;
                _activeSubMenu.Location = leftPoint;
            }
            
            // 检查底部空间
            if (_activeSubMenu.Bottom > screen.WorkingArea.Bottom)
            {
                // 如果底部空间不足，则向上调整
                int newY = _activeSubMenu.Top - (_activeSubMenu.Height - itemRect.Height);
                if (newY < screen.WorkingArea.Top) newY = screen.WorkingArea.Top;
                _activeSubMenu.Location = new Point(_activeSubMenu.Left, newY);
            }
            
            _activeSubMenu.SubMenuClosed += (s, e) => 
            {
                _isSubMenuOpen = false;
                _activeSubMenu = null;
                
                // 确保主菜单获得焦点
                Focus();
            };
        }

        private void CloseSubMenu()
        {
            if (_activeSubMenu != null)
            {
                _activeSubMenu.Close();
                _activeSubMenu = null;
                _isSubMenuOpen = false;
            }
        }

        #endregion

        #region 辅助方法

        private Rectangle GetItemRectangle(int index)
        {
            if (index < 0 || index >= _items.Count)
                return Rectangle.Empty;
            
            int y = 0;
            for (int i = 0; i < index; i++)
            {
                if (_items[i].Visible)
                {
                    y += ItemHeight;
                }
            }
            
            return new Rectangle(0, y, Width, ItemHeight);
        }

        private int GetItemIndexAtPoint(Point point)
        {
            if (_items.Count == 0)
                return -1;
                
            int y = 0;
            for (int i = 0; i < _items.Count; i++)
            {
                if (_items[i].Visible)
                {
                    Rectangle rect = new Rectangle(0, y, Width, ItemHeight);
                    if (rect.Contains(point) && _items[i].Enabled)
                    {
                        return i;
                    }
                    y += ItemHeight;
                }
            }
            
            return -1;
        }
        
        private void CreateDropShadow()
        {
            // 为窗体添加阴影效果
            const int CS_DROPSHADOW = 0x00020000;
            CreateParams cp = CreateParams;
            cp.ClassStyle |= CS_DROPSHADOW;
        }

        #endregion

        #region 窗体事件处理

        protected override void OnDeactivate(EventArgs e)
        {
            base.OnDeactivate(e);
            
            // 确保窗体不会因为失去焦点而关闭
            if (!IsDisposed && !_isSubMenuOpen)
            {
                Close();
                SubMenuClosed?.Invoke(this, EventArgs.Empty);
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            CloseSubMenu();
            SubMenuClosed?.Invoke(this, EventArgs.Empty);
        }

        // 激活子菜单时，确保所有子菜单保持显示
        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            BringToFront();
            
            // 确保子菜单也置于前台
            if (_activeSubMenu != null && !_activeSubMenu.IsDisposed)
            {
                _activeSubMenu.BringToFront();
            }
        }

        #endregion

        #region 覆盖ShowWithoutActivation属性，使子菜单可以显示在父窗体之上而不激活
        protected override bool ShowWithoutActivation => true;
        
        protected override CreateParams CreateParams
        {
            get
            {
                const int CS_DROPSHADOW = 0x00020000;
                const int WS_EX_TOOLWINDOW = 0x00000080; // 使用工具窗口样式，不会出现在任务栏
                const int WS_EX_TOPMOST = 0x00000008;    // 窗口总是置顶
                
                CreateParams cp = base.CreateParams;
                cp.ClassStyle |= CS_DROPSHADOW;
                cp.ExStyle |= WS_EX_TOOLWINDOW | WS_EX_TOPMOST;
                return cp;
            }
        }
        #endregion
    }
} 