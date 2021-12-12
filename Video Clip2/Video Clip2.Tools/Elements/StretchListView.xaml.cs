using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Video_Clip2.Tools.Elements
{
    internal class StretchListViewItem : ListViewItem
    {
        //public string Name { get; set; }
        public Stretch Type { get; set; }
        public int Index { get; set; } = -1;
    }

    public sealed partial class StretchListView : UserControl
    {

        //@Delegate
        /// <summary> Occurs when stretch change. </summary>
        public event EventHandler<Stretch> StretchChanged;

        //@Group
        private readonly IDictionary<Stretch, StretchListViewItem> ItemDictionary = new Dictionary<Stretch, StretchListViewItem>();

        #region DependencyProperty


        /// <summary> Gets or sets the tool-type. </summary>
        public Stretch Stretch
        {
            get => (Stretch)base.GetValue(StretchProperty);
            set => base.SetValue(StretchProperty, value);
        }
        /// <summary> Identifies the <see cref = "StretchListView.Stretch" /> dependency property. </summary>
        public static readonly DependencyProperty StretchProperty = DependencyProperty.Register(nameof(Stretch), typeof(Stretch), typeof(StretchListView), new PropertyMetadata(Stretch.None, (sender, e) =>
        {
            StretchListView control = (StretchListView)sender;

            if (e.NewValue is Stretch value)
            {
                if (control.ItemDictionary.ContainsKey(value))
                {
                    StretchListViewItem item = control.ItemDictionary[value];
                    control.ListView.SelectedIndex = item.Index;
                }
                else
                {
                    control.ListView.SelectedIndex = -1;
                }
            }
        }
    ));


        #endregion

        public StretchListView()
        {
            this.InitializeComponent();
            this.InitializeDictionary();
            this.ListView.ItemClick += (s, e) =>
            {
                if (e.ClickedItem is ContentControl control)
                {
                    if (control.Parent is StretchListViewItem item)
                    {
                        Stretch type = item.Type;
                        this.StretchChanged?.Invoke(this, type); // Delegate
                    }
                }
            };
        }
    }

    public sealed partial class StretchListView : UserControl
    {


        //@Group
        private void InitializeDictionary()
        {
            foreach (object child in this.ListView.Items)
            {
                if (child is StretchListViewItem item)
                {
                    Stretch type = item.Type;

                    this.ItemDictionary.Add(type, item);
                }
            }
        }

    }
}