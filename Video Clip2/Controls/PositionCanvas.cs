using System;
using System.Collections.Generic;
using Video_Clip2.Elements;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace Video_Clip2.Controls
{
    public class PositionCanvas : Canvas
    {

        #region DependencyProperty


        /// <summary> Gets or sets <see cref = "PositionCanvas" />'s duration. </summary>
        public TimeSpan Duration
        {
            get => (TimeSpan)base.GetValue(DurationProperty);
            set => base.SetValue(DurationProperty, value);
        }
        /// <summary> Identifies the <see cref = "PositionCanvas.Duration" /> dependency property. </summary>
        public static readonly DependencyProperty DurationProperty = DependencyProperty.Register(nameof(Duration), typeof(TimeSpan), typeof(PositionCanvas), new PropertyMetadata(TimeSpan.FromMinutes(20), (sender, e) =>
        {
            PositionCanvas control = (PositionCanvas)sender;

            if (e.NewValue is TimeSpan value)
            {
                control.UpdateWidth(value, control.TrackScale);
            }
        }));


        /// <summary> Gets or sets <see cref = "PositionCanvas" />'s scale. </summary>
        public double TrackScale
        {
            get => (double)base.GetValue(TrackScaleProperty);
            set => base.SetValue(TrackScaleProperty, value);
        }
        /// <summary> Identifies the <see cref = "PositionCanvas.TrackScale" /> dependency property. </summary>
        public static readonly DependencyProperty TrackScaleProperty = DependencyProperty.Register(nameof(TrackScale), typeof(double), typeof(PositionCanvas), new PropertyMetadata(16d, (sender, e) =>
        {
            PositionCanvas control = (PositionCanvas)sender;

            if (e.NewValue is double value)
            {
                control.UpdateWidth(control.Duration, value);
            }
        }));


        #endregion

        //@Construct
        /// <summary>
        /// Initializes a PositionCanvas. 
        /// </summary>
        public PositionCanvas()
        {
            base.Loaded += (s, e) =>
            {
                this.UpdateWidth(this.Duration, this.TrackScale);
            };
        }

        private void UpdateWidth(TimeSpan duration, double trackScale)
        {
            double space = 20 * trackScale;

            while (space > 50)
                space /= 5;
            while (space < 5)
                space *= 5;

            double width = duration.ToDouble(trackScale);
            int count = (int)(width / space);

            base.Width = width;
            if (base.Children.Count == count)
            {
                for (int i = 0; i < count; i++)
                {
                    bool isFive = i % 5 == 0;
                    double left = isFive ? 3.0 : 1.5;

                    UIElement ellipse = base.Children[i];

                    Canvas.SetLeft(ellipse, i * space - left);
                }
            }
            else
            {
                base.Children.Clear();

                for (int i = 0; i < count; i++)
                {
                    bool isFive = i % 5 == 0;
                    double spuare = isFive ? 6.0 : 3.0;
                    double left = isFive ? 3.0 : 1.5;
                    double top = isFive ? 12.0 : 13.5;

                    Ellipse ellipse = new Ellipse
                    {
                        Width = spuare,
                        Height = spuare,
                        Fill = new SolidColorBrush(Colors.Gray)
                    };

                    Canvas.SetLeft(ellipse, i * space - left);
                    Canvas.SetTop(ellipse, top);

                    base.Children.Add(ellipse);
                }
            }

        }
    }
}