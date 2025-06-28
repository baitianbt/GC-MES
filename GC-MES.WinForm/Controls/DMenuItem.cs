using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace GC_MES.WinForm.Controls
{
    /// <summary>
    /// 自定义菜单项类
    /// </summary>
    public class DMenuItem
    {
        /// <summary>
        /// 菜单项文本
        /// </summary>
        public string Text { get; set; } = string.Empty;
        
        /// <summary>
        /// 菜单项图标
        /// </summary>
        public Image? Icon { get; set; }
        
        /// <summary>
        /// 子菜单项列表
        /// </summary>
        public List<DMenuItem> SubItems { get; } = new List<DMenuItem>();
        
        /// <summary>
        /// 菜单项ID
        /// </summary>
        public string ID { get; set; } = string.Empty;
        
        /// <summary>
        /// 菜单项是否可用
        /// </summary>
        public bool Enabled { get; set; } = true;
        
        /// <summary>
        /// 菜单项是否可见
        /// </summary>
        public bool Visible { get; set; } = true;
        
        /// <summary>
        /// 点击事件处理器
        /// </summary>
        public event EventHandler? Click;
        
        /// <summary>
        /// 触发点击事件
        /// </summary>
        internal void OnClick(object sender, EventArgs e)
        {
            Click?.Invoke(sender, e);
        }
        
        public DMenuItem()
        {
        }
        
        public DMenuItem(string text)
        {
            Text = text;
        }
        
        public DMenuItem(string text, Image icon)
        {
            Text = text;
            Icon = icon;
        }
        
        public DMenuItem(string text, Image icon, EventHandler clickHandler)
        {
            Text = text;
            Icon = icon;
            Click += clickHandler;
        }
        
        /// <summary>
        /// 添加子菜单项
        /// </summary>
        public DMenuItem AddSubItem(DMenuItem item)
        {
            SubItems.Add(item);
            return this;
        }
        
        /// <summary>
        /// 添加子菜单项
        /// </summary>
        public DMenuItem AddSubItem(string text)
        {
            var item = new DMenuItem(text);
            SubItems.Add(item);
            return item;
        }
        
        /// <summary>
        /// 添加子菜单项
        /// </summary>
        public DMenuItem AddSubItem(string text, Image icon)
        {
            var item = new DMenuItem(text, icon);
            SubItems.Add(item);
            return item;
        }
        
        /// <summary>
        /// 添加子菜单项
        /// </summary>
        public DMenuItem AddSubItem(string text, Image icon, EventHandler clickHandler)
        {
            var item = new DMenuItem(text, icon, clickHandler);
            SubItems.Add(item);
            return item;
        }
        
        /// <summary>
        /// 获取是否有子菜单
        /// </summary>
        public bool HasSubItems => SubItems.Count > 0;
    }
} 