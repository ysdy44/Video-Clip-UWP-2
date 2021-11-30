using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace Video_Clip2.Controls
{
    /// <summary>
    /// Canvas for <see cref="Rectangle"/>s.
    /// </summary>
    public sealed class RectangleCanvas : Canvas
    {

        #region DependencyProperty


        public Style DarkStyle
        {
            get => (Style)base.GetValue(DarkStyleProperty);
            set => base.SetValue(DarkStyleProperty, value);
        }
        /// <summary> Identifies the <see cref = "RectangleCanvas.DarkStyle" /> dependency property. </summary>
        public static readonly DependencyProperty DarkStyleProperty = DependencyProperty.Register(nameof(DarkStyle), typeof(Style), typeof(RectangleCanvas), new PropertyMetadata(null));


        public Style LightStyle
        {
            get => (Style)base.GetValue(LightStyleProperty);
            set => base.SetValue(LightStyleProperty, value);
        }
        /// <summary> Identifies the <see cref = "RectangleCanvas.LightStyle" /> dependency property. </summary>
        public static readonly DependencyProperty LightStyleProperty = DependencyProperty.Register(nameof(LightStyle), typeof(Style), typeof(RectangleCanvas), new PropertyMetadata(null));


        public double ItemWidth
        {
            get => (double)base.GetValue(ItemWidthProperty);
            set => base.SetValue(ItemWidthProperty, value);
        }
        /// <summary> Identifies the <see cref = "RectangleCanvas.ItemWidth" /> dependency property. </summary>
        public static readonly DependencyProperty ItemWidthProperty = DependencyProperty.Register(nameof(ItemWidth), typeof(double), typeof(RectangleCanvas), new PropertyMetadata(50d, (sender, e) =>
        {
            RectangleCanvas control = (RectangleCanvas)sender;

            if (e.NewValue is double value)
            {
                foreach (FrameworkElement item in control.Children)
                {
                    item.Width = value;
                }
            }
        }));


        public double ItemHeight
        {
            get => (double)base.GetValue(ItemHeightProperty);
            set => base.SetValue(ItemHeightProperty, value);
        }
        /// <summary> Identifies the <see cref = "RectangleCanvas.ItemHeight" /> dependency property. </summary>
        public static readonly DependencyProperty ItemHeightProperty = DependencyProperty.Register(nameof(ItemHeight), typeof(double), typeof(RectangleCanvas), new PropertyMetadata(50d, (sender, e) =>
        {
            RectangleCanvas control = (RectangleCanvas)sender;

            if (e.NewValue is double value)
            {
                foreach (FrameworkElement item in control.Children)
                {
                    int index = (int)(Canvas.GetTop(item) / item.Height);
                    item.Height = value;
                    Canvas.SetTop(item, index * value);
                }
            }
        }));


        public int MaximumRows
        {
            get => (int)base.GetValue(MaximumRowsProperty);
            set => base.SetValue(MaximumRowsProperty, value);
        }
        /// <summary> Identifies the <see cref = "RectangleCanvas.MaximumRows" /> dependency property. </summary>
        public static readonly DependencyProperty MaximumRowsProperty = DependencyProperty.Register(nameof(MaximumRows), typeof(int), typeof(RectangleCanvas), new PropertyMetadata(5, (sender, e) =>
        {
            RectangleCanvas control = (RectangleCanvas)sender;

            if (e.NewValue is int value)
            {
                control.Children.Clear();
                control.UpdateRows(value);
            }
        }));


        #endregion

        //@Construct
        /// <summary>
        /// Initializes a RectangleCanvas. 
        /// </summary>
        public RectangleCanvas()
        {
            base.Loaded += (s, e) =>
            {
                this.UpdateRows(this.MaximumRows);
            };
        }

        private void UpdateRows(int count)
        {
            for (int i = 0; i < count; i++)
            {
                this.Children.Add(this.CreateRectangle(i));
            }
            base.Height = count * this.ItemHeight;
        }

        private Rectangle CreateRectangle(int index)
        {
            Rectangle rectangle = new Rectangle
            {
                Width = this.ItemWidth,
                Height = this.ItemHeight,
                Style = index % 2 == 0 ? this.DarkStyle : this.LightStyle
            };
            Canvas.SetTop(rectangle, index * this.ItemHeight);

            return rectangle;
        }

    }
}