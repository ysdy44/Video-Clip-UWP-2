using System.Collections.Generic;
using System.Collections.Specialized;
using Video_Clip2.Clips;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Video_Clip2.Controls
{
    /// <summary>
    /// Canvas for <see cref="ItemsControl.ItemSource"/>s.
    /// </summary>
    public sealed class ItemCanvas : Canvas
    {

        //@Delegate
        public event TappedEventHandler ItemTapped;
        public event RoutedEventHandler ItemClick;

        public event RightTappedEventHandler ItemRightTapped;
        public event HoldingEventHandler ItemHolding;

        public event ManipulationStartedEventHandler ItemManipulationStarted;
        public event ManipulationDeltaEventHandler ItemManipulationDelta;
        public event ManipulationCompletedEventHandler ItemManipulationCompleted;


        #region DependencyProperty


        /// <summary> Gets or sets the source of<see cref = "ItemCanvas" />'s items. </summary>
        public object ItemSource
        {
            get => (object)base.GetValue(ItemSourceProperty);
            set => base.SetValue(ItemSourceProperty, value);
        }
        /// <summary> Identifies the <see cref = "ItemSource" /> dependency property. </summary>
        public static readonly DependencyProperty ItemSourceProperty = DependencyProperty.Register(nameof(ItemSource), typeof(object), typeof(ItemCanvas), new PropertyMetadata(false, (sender, e) =>
        {
            ItemCanvas control = (ItemCanvas)sender;

            if (e.NewValue is object value)
            {
                if (control.ItemSourceNotify != null)
                {
                    control.ItemSourceNotify.CollectionChanged -= control.ItemSourceNotify_CollectionChanged;
                    if (control.ItemSourceNotify is IEnumerable<Clipping> items)
                    {
                        foreach (Clipping item in items)
                        {
                            control.Children.Remove(item.Self.Track.Self);
                            control.RemoveHandler2(item);
                        }
                    }
                }
                control.ItemSourceNotify = value as INotifyCollectionChanged;
                if (control.ItemSourceNotify != null)
                {
                    control.ItemSourceNotify.CollectionChanged += control.ItemSourceNotify_CollectionChanged;
                    if (control.ItemSourceNotify is IEnumerable<Clipping> items)
                    {
                        foreach (Clipping item in items)
                        {
                            control.Children.Add(item.Self.Track.Self);
                            control.AddHandler2(item);
                        }
                    }
                }
            }
        }));


        #endregion


        private INotifyCollectionChanged ItemSourceNotify;


        private void ItemSourceNotify_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (e.NewItems[0] is Clipping itemAdd)
                    {
                        int index = e.NewStartingIndex;
                        this.Children.Insert(index, itemAdd.Self.Track.Self);
                        this.AddHandler2(itemAdd);
                    }
                    break;

                case NotifyCollectionChangedAction.Move:
                    {
                        int index = e.OldStartingIndex;
                        this.Children.RemoveAt(index);
                        this.RemoveHandler2(e.OldItems[index] as Clipping);
                    }
                    if (e.NewItems[0] is Clipping itemMove)
                    {
                        int index = e.NewStartingIndex;
                        base.Children.Insert(index, itemMove.Self.Track.Self);
                        this.AddHandler2(e.NewItems[index] as Clipping);
                    }
                    break;

                case NotifyCollectionChangedAction.Remove:
                    if (e.OldItems[0] is Clipping itemRemove)
                    {
                        int index = e.OldStartingIndex;
                        this.Children.RemoveAt(index);
                        this.RemoveHandler2(itemRemove);
                    }
                    break;

                case NotifyCollectionChangedAction.Replace:
                    {
                        int index = e.OldStartingIndex;
                        this.Children.RemoveAt(index);
                        this.RemoveHandler2(e.OldItems[index] as Clipping);
                    }
                    if (e.NewItems[0] is Clipping itemReplace)
                    {
                        int index = e.NewStartingIndex;
                        this.Children.Insert(index, itemReplace.Self.Track.Self);
                        this.AddHandler2(e.NewItems[index] as Clipping);
                    }
                    break;

                case NotifyCollectionChangedAction.Reset:
                    foreach (FrameworkElement item in base.Children)
                    {
                        this.RemoveHandler2(item.DataContext as Clipping);
                    }
                    base.Children.Clear();
                    break;

                default:
                    break;
            };
        }


        private void AddHandler2(Clipping clipping)
        {
            Button element = clipping.Self.Track.Self;
            if (element is null) return;
            element.DataContext = clipping;

            element.Tapped += this.ItemTapped;
            element.Click += this.ItemClick;
            element.RightTapped += this.ItemRightTapped;
            element.Holding += this.ItemHolding;
            element.ManipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateY;
            element.ManipulationStarted += this.ItemManipulationStarted;
            element.ManipulationDelta += this.ItemManipulationDelta;
            element.ManipulationCompleted += this.ItemManipulationCompleted;

            element.PointerPressed += this.Item_PointerPressed;
            element.PointerMoved += this.Item_PointerMoved;
            element.PointerReleased += this.Item_PointerReleased;
        }

        private void RemoveHandler2(Clipping clipping)
        {
            Button element = clipping.Self.Track.Self;
            if (element is null) return;
            element.DataContext = null;

            element.Tapped -= this.ItemTapped;
            element.Click -= this.ItemClick;
            element.RightTapped -= this.ItemRightTapped;
            element.Holding -= this.ItemHolding;
            element.ManipulationMode = ManipulationModes.None;
            element.ManipulationStarted -= this.ItemManipulationStarted;
            element.ManipulationDelta -= this.ItemManipulationDelta;
            element.ManipulationCompleted -= this.ItemManipulationCompleted;

            element.PointerPressed -= this.Item_PointerPressed;
            element.PointerMoved -= this.Item_PointerMoved;
            element.PointerReleased -= this.Item_PointerReleased;
        }

        private void Item_PointerPressed(object sender, PointerRoutedEventArgs e) => e.Handled = true;
        private void Item_PointerMoved(object sender, PointerRoutedEventArgs e) => e.Handled = true;
        private void Item_PointerReleased(object sender, PointerRoutedEventArgs e) => e.Handled = true;

    }
}