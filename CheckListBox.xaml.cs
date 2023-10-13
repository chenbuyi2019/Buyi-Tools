using MahApps.Metro.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BuyiTools
{
    /// <summary>
    /// Interaction logic for CheckListBox.xaml
    /// </summary>
    public partial class CheckListBox : UserControl, INotifyPropertyChanged
    {
        public CheckListBox()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register(
                nameof(Items), typeof(IEnumerable), typeof(CheckListBox));

        private IEnumerable items = Array.Empty<object>();

        [Browsable(false)]
        public ObservableCollection<CheckedListItem> UIItems { get; set; } = new();

        [Browsable(false)]
        public IEnumerable Items
        {
            get
            {
                return items;
            }
            set
            {

                if (value == null)
                {
                    items = Array.Empty<object>();
                    UIItems.Clear();
                }
                else
                {
                    items = value;
                    foreach (var item in items)
                    {
                        if (item == null) { throw new Exception("不能将null加入 CheckListBox"); }
                        if (UIItems.Contains(item)) { throw new Exception($"不能将重复值加入 CheckListBox  {item}"); }
                        var uiItem = new CheckedListItem(item, this);
                        UIItems.Add(uiItem);
                    }
                }
                CallPropertyChanged(nameof(Items));
            }
        }

        #region 函数
        /// <summary>
        /// 获取全部已经打勾的项目
        /// </summary>
        public IList GetCheckedItems()
        {
            var ls = new List<object>();
            foreach (var item in UIItems)
            {
                if (item.IsChecked)
                {
                    ls.Add(item.Item);
                }
            }
            return ls;
        }

        /// <summary>
        /// 获取全部已经打勾的项目
        /// </summary>
        public IList<T> GetCheckedItems<T>()
        {
            var ls = new List<T>();
            foreach (var item in UIItems)
            {
                if (item.IsChecked && item.Item is T value)
                {
                    ls.Add(value);
                }
            }
            return ls;
        }

        private CheckedListItem GetUIItemByValue(object value)
        {
            var v = UIItems.FirstOrDefault((x) => { return x.Item.Equals(value); });
            if (v == null) { throw new Exception($"值不在 CheckListBox 里 {value}"); }
            return v;
        }

        /// <summary>
        /// 判断某个项目是否已经打勾
        /// </summary>
        public bool IsChecked(object value)
        {
            var u = GetUIItemByValue(value);
            return u.IsChecked;
        }

        /// <summary>
        /// 设置某个项目是否打勾
        /// </summary>
        public void SetChecked(object value, bool ischecked)
        {
            var u = GetUIItemByValue(value);
            u.IsChecked = ischecked;
        }
        #endregion

        #region 事件

        public static readonly RoutedEvent CheckedChangedEvent = EventManager.RegisterRoutedEvent(
           nameof(CheckedChanged), RoutingStrategy.Bubble,
            typeof(EventHandler), typeof(CheckListBox));

        public event EventHandler CheckedChanged
        {
            add { AddHandler(CheckedChangedEvent, value); }
            remove { RemoveHandler(CheckedChangedEvent, value); }
        }

        public void CallCheckedChanged()
        {
            var a = new RoutedEventArgs(CheckedChangedEvent, this);
            RaiseEvent(a);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void CallPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion
    }

    public class CheckedListItem : INotifyPropertyChanged
    {
        public CheckedListItem(object obj, CheckListBox parent)
        {
            this.item = obj;
            this.parent = parent;
        }

        private readonly CheckListBox parent;
        private object item;
        private bool isChecked = false;

        public bool IsChecked
        {
            get { return isChecked; }
            set
            {
                isChecked = value;
                parent.CallCheckedChanged();
                CallPropertyChanged(nameof(IsChecked));
            }
        }

        public object Item
        {
            get { return item; }
            set
            {
                item = value;
                CallPropertyChanged(nameof(Item));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void CallPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

    }
}
