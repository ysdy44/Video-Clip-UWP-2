using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Video_Clip2.Elements;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Video_Clip2.Controls
{
    public sealed class PinCanvas : Canvas
    {

        // Position & Pin
        readonly IDictionary<TimeSpan, Button> Buttons = new Dictionary<TimeSpan, Button>();
        Button CurrentButton;

        #region DependencyProperty


        /// <summary> Gets or sets the source of <see cref = "PinCanvas" /> 's items. </summary>
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
                        foreach (var item in control.Buttons)
                        {
                            item.Value.Click -= control.ItemClick;
                        }
                        control.Children.Clear();
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
                        }
                    }
                }
            }
        }));


        /// <summary> Gets or sets state of <see cref = "PinCanvas" />'s position. </summary>
        public bool IsPositionOnPin
        {
            get => (bool)base.GetValue(IsPositionOnPinProperty);
            set => base.SetValue(IsPositionOnPinProperty, value);
        }
        /// <summary> Identifies the<see cref = "PinCanvas.IsPositionOnPin" /> dependency property. </summary>
        public static readonly DependencyProperty IsPositionOnPinProperty = DependencyProperty.Register(nameof(IsPositionOnPin), typeof(bool), typeof(PinCanvas), new PropertyMetadata(false));


        /// <summary> Gets or sets <see cref = "PinCanvas" />'s position. </summary>
        public TimeSpan Position
        {
            get => (TimeSpan)base.GetValue(PositionProperty);
            set => base.SetValue(PositionProperty, value);
        }
        /// <summary> Identifies the<see cref = "PinCanvas.Position" /> dependency property. </summary>
        public static readonly DependencyProperty PositionProperty = DependencyProperty.Register(nameof(Position), typeof(TimeSpan), typeof(PinCanvas), new PropertyMetadata(TimeSpan.Zero, (sender, e) =>
        {
            PinCanvas control = (PinCanvas)sender;

            if (e.NewValue is TimeSpan value)
            {
                control.UpdatePosition(value);
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


        /// <summary> Gets or sets style of <see cref = "PinCanvas" />'s item. </summary>
        public Style ItemStyle
        {
            get => (Style)base.GetValue(ItemStyleProperty);
            set => base.SetValue(ItemStyleProperty, value);
        }
        /// <summary> Identifies the <see cref = "PinCanvas.ItemStyle" /> dependency property. </summary>
        public static readonly DependencyProperty ItemStyleProperty = DependencyProperty.Register(nameof(ItemStyle), typeof(Style), typeof(PinCanvas), new PropertyMetadata(null));


        #endregion

        private INotifyCollectionChanged ItemSourceNotify;

        private Button Create(TimeSpan itemAdd, double trackScale)
        {
            if (this.Buttons.ContainsKey(itemAdd))
            {
                return this.Buttons[itemAdd];
            }

            Button button = new Button
            {
                Tag = itemAdd,
                Style = this.ItemStyle,
            };
            button.Click += this.ItemClick;
            Canvas.SetLeft(button, itemAdd.ToDouble(trackScale) - 9);
            this.Buttons.Add(itemAdd, button);
            this.UpdatePosition(this.Position);
            return button;
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
                    }
                    break;

                case NotifyCollectionChangedAction.Move:
                    {
                        int index = e.OldStartingIndex;
                    }
                    if (e.NewItems[0] is TimeSpan itemMove)
                    {
                        int index = e.NewStartingIndex;
                        base.Children.Insert(index, this.Buttons[itemMove]);
                        this.UpdatePosition(this.Position);
                    }
                    break;

                case NotifyCollectionChangedAction.Remove:
                    if (e.OldItems[0] is TimeSpan itemRemove)
                    {
                        int index = e.OldStartingIndex;
                        Button item = base.Children[index] as Button;
                        item.Click -= this.ItemClick;
                        base.Children.RemoveAt(index);
                        this.Buttons.Remove(itemRemove);
                        this.UpdatePosition(this.Position);
                    }
                    break;

                case NotifyCollectionChangedAction.Replace:
                    {
                        int index = e.OldStartingIndex;
                        Button item = base.Children[index] as Button;
                        item.Click -= this.ItemClick;
                        TimeSpan time = (TimeSpan)item.Tag;
                        base.Children.RemoveAt(index);
                        this.Buttons.Remove(time);
                    }
                    if (e.NewItems[0] is TimeSpan itemReplace)
                    {
                        int index = e.NewStartingIndex;
                        this.Children.Insert(index, this.Create(itemReplace, this.TrackScale));
                    }
                    break;

                case NotifyCollectionChangedAction.Reset:
                    foreach (Button item in base.Children)
                    {
                        item.Click -= this.ItemClick;
                    }
                    base.Children.Clear();
                    this.UpdatePosition(this.Position);
                    break;

                default:
                    break;
            };
        }

        private void ItemClick(object sender, RoutedEventArgs e)
        {
            Button item = sender as Button;
            this.Position = (TimeSpan)item.Tag;
        }

        private void UpdateWidth(double trackScale)
        {
            foreach (var item in this.Buttons)
            {
                Canvas.SetLeft(item.Value, item.Key.ToDouble(trackScale) - 9);
            }
        }

        private void UpdatePosition(TimeSpan position)
        {
            if (position == TimeSpan.Zero)
            {
                this.IsPositionOnPin = true;
            }
            else
            {
                if (this.CurrentButton != null) this.CurrentButton.IsEnabled = true;
                this.IsPositionOnPin = this.Buttons.ContainsKey(position);
                this.CurrentButton = this.IsPositionOnPin ? this.Buttons[position] : null;
                if (this.CurrentButton != null) this.CurrentButton.IsEnabled = false;
            }
        }

        //@Static
        public static bool Pin(TimeSpan position, ObservableCollection<TimeSpan> collection)
        {
            if (position == TimeSpan.Zero)
            {
                return false;
            }
            else if (collection.Contains(position))
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