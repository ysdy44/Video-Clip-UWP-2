using System;
using System.Collections.Generic;
using Video_Clip2.Clips;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace Video_Clip2.Controls
{
    public class PositionCanvas : Canvas
    {

        int Power = 0;
        // Index & Point
        readonly IDictionary<int, Ellipse> Ellipses = new Dictionary<int, Ellipse>();

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
                control.UpdateItemsWidth(value, control.TrackScale);
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
                control.UpdateItemsWidth(control.Duration, value);
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
                this.UpdateItemsWidth(this.Duration, this.TrackScale);
            };
        }

        private void UpdateWidth(TimeSpan duration, double trackScale)
        {
            base.Width = duration.ToDouble(trackScale);
        }

        public void UpdateItemsWidth(TimeSpan duration, double trackScale)
        {
            double space = 20 * trackScale;
            int power = 0;

            while (space > 50)
            {
                space /= 5;
                power--;
            }
            while (space < 5)
            {
                space *= 5;
                power++;
            }


            if (this.Power == power)
            {
                foreach (var item in this.Ellipses)
                {
                    int i = item.Key;
                    Ellipse ellipse = item.Value;
                    Canvas.SetLeft(ellipse, i * space);
                }
            }
            else
            {
                this.Power = power;

                double width = duration.ToDouble(trackScale);
                int count = (int)(width / space);

                base.Children.Clear();
                this.Ellipses.Clear();

                for (int i = 0; i < count; i++)
                {
                    bool isFive = i % 5 == 0;
                    double spuare = isFive ? 6.0 : 3.0;
                    double top = isFive ? 12.0 : 13.5;

                    Ellipse ellipse = new Ellipse
                    {
                        Width = spuare,
                        Height = spuare,
                        Fill = new SolidColorBrush(Colors.Gray)
                    };

                    Canvas.SetLeft(ellipse, i * space);
                    Canvas.SetTop(ellipse, top);

                    this.Ellipses.Add(i, ellipse);
                    base.Children.Add(ellipse);
                }
            }

        }
    }
}