using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Video_Clip2.Clips;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace Video_Clip2.Controls
{
    public sealed class PinCanvas : Canvas
    {

        // Position & Pin
        readonly IDictionary<TimeSpan, Rectangle> Rectangles = new Dictionary<TimeSpan, Rectangle>();

        #region DependencyProperty


        /// <summary> Gets or sets the source of<see cref = "PinCanvas" />'s items. </summary>
        public object ItemSource
        {
            get => (object)base.GetValue(ItemSourceProperty);
            set => base.SetValue(ItemSourceProperty, value);
        }
        /// <summary> Identifies the <see cref = "ItemSource" /> dependency property. </summary>
        public static readonly DependencyProperty ItemSourceProperty = DependencyProperty.Register(nameof(ItemSource), typeof(object), typeof(PinCanvas), new PropertyMetadata(false, (sender, e) =>
        {
            PinCanvas control = (PinCanvas)sender;

            if (e.NewValue is object value)
            {
                if (control.ItemSourceNotify != null)
                {
                    control.ItemSourceNotify.CollectionChanged -= control.ItemSourceNotify_CollectionChanged;
                    if (control.ItemSourceNotify is IEnumerable<TimeSpan> items)
                    {
                        foreach (TimeSpan item in items)
                        {
                            control.Children.Remove(control.Rectangles[item]);
                            control.RemoveHandler2(item);
                        }
                    }
                }
                control.ItemSourceNotify = value as INotifyCollectionChanged;
                if (control.ItemSourceNotify != null)
                {
                    control.ItemSourceNotify.CollectionChanged += control.ItemSourceNotify_CollectionChanged;
                    if (control.ItemSourceNotify is IEnumerable<TimeSpan> items)
                    {
                        foreach (TimeSpan item in items)
                        {
                            control.Children.Add(control.Create(item, control.TrackScale));
                            control.AddHandler2(item);
                        }
                    }
                }
            }
        }));


        /// <summary> Gets or sets <see cref = "PinCanvas" />'s scale. </summary>
        public double TrackScale
        {
            get => (double)base.GetValue(TrackScaleProperty);
            set => base.SetValue(TrackScaleProperty, value);
        }
        /// <summary> Identifies the <see cref = "PinCanvas.TrackScale" /> dependency property. </summary>
        public static readonly DependencyProperty TrackScaleProperty = DependencyProperty.Register(nameof(TrackScale), typeof(double), typeof(PinCanvas), new PropertyMetadata(16d, (sender, e) =>
        {
            PinCanvas control = (PinCanvas)sender;

            if (e.NewValue is double value)
            {
                control.UpdateWidth(value);
            }
        }));


        #endregion

        private INotifyCollectionChanged ItemSourceNotify;

        private Rectangle Create(TimeSpan itemAdd, double trackScale)
        {
            if (this.Rectangles.ContainsKey(itemAdd))
            {
                return this.Rectangles[itemAdd];
            }

            Rectangle rectangle = new Rectangle
            {
                Width = 2,
                Height = 32,
                Fill = new SolidColorBrush(Colors.Red)
            };
            Canvas.SetLeft(rectangle, itemAdd.ToDouble(trackScale) - 1);
            this.Rectangles.Add(itemAdd, rectangle);
            return rectangle;
        }

        private void ItemSourceNotify_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (e.NewItems[0] is TimeSpan itemAdd)
                    {
                        int index = e.NewStartingIndex;
                        base.Children.Insert(index, this.Create(itemAdd, this.TrackScale));
                        this.AddHandler2(itemAdd);
                    }
                    break;

                case NotifyCollectionChangedAction.Move:
                    {
                        int index = e.OldStartingIndex;
                        base.Children.RemoveAt(index);
                        this.RemoveHandler2((TimeSpan)e.OldItems[index]);
                    }
                    if (e.NewItems[0] is TimeSpan itemMove)
                    {
                        int index = e.NewStartingIndex;
                        base.Children.Insert(index, this.Rectangles[itemMove]);
                        this.AddHandler2((TimeSpan)e.NewItems[index]);
                    }
                    break;

                case NotifyCollectionChangedAction.Remove:
                    if (e.OldItems[0] is TimeSpan itemRemove)
                    {
                        int index = e.OldStartingIndex;
                        base.Children.RemoveAt(index);
                        this.RemoveHandler2(itemRemove);
                    }
                    break;

                case NotifyCollectionChangedAction.Replace:
                    {
                        int index = e.OldStartingIndex;
                        TimeSpan time = (TimeSpan)(base.Children[index] as FrameworkElement).Tag;
                        base.Children.RemoveAt(index);
                        this.Rectangles.Remove(time);
                        this.RemoveHandler2((TimeSpan)e.OldItems[index]);
                    }
                    if (e.NewItems[0] is TimeSpan itemReplace)
                    {
                        int index = e.NewStartingIndex;
                        this.Children.Insert(index, this.Create(itemReplace, this.TrackScale));
                        this.AddHandler2((TimeSpan)e.NewItems[index]);
                    }
                    break;

                case NotifyCollectionChangedAction.Reset:
                    foreach (FrameworkElement item in base.Children)
                    {
                        this.RemoveHandler2((TimeSpan)item.Tag);
                    }
                    base.Children.Clear();
                    break;

                default:
                    break;
            };
        }


        private void AddHandler2(TimeSpan time)
        {
        }

        private void RemoveHandler2(TimeSpan time)
        {
        }

        private void UpdateWidth(double trackScale)
        {
            foreach (var item in this.Rectangles)
            {
                TimeSpan time = item.Key;
                Rectangle rectangle = item.Value;
                Canvas.SetLeft(rectangle, time.ToDouble(trackScale) - 1);
            }
        }


        //@Static
        public static bool Pin(TimeSpan position, ObservableCollection<TimeSpan> collection)
        {
            if (collection.Contains(position))
            {
                collection.Remove(position);
                return false;
            }
            else
            {
                collection.Add(position);
                return true;
            }
        }

    }
}