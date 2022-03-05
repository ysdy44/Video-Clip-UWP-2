using System;
using System.Linq;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Shapes;

namespace Video_Clip2.Elements
{
    /// <summary>
    /// Represents a control that indicates that an operation is ongoing. 
    /// The typical visual appearance is a ring-shaped "spinner"
    /// that cycles an animation as progress continues.
    /// </summary>
    public sealed class ProgressRing : Canvas
    {

        readonly Path[] Paths = new Path[4];
        readonly RotateTransform[] RotateTransforms = new RotateTransform[4];
        readonly PathFigure[] PathFigures = new PathFigure[4];
        readonly ArcSegment[] ArcSegments = new ArcSegment[4];
        readonly Color[] Strokes = new Color[4]
        {
            //Color.FromArgb(byte.MaxValue, 25, 168, 83),
            //Color.FromArgb(byte.MaxValue, 249, 187, 4),
            //Color.FromArgb(byte.MaxValue, 232, 67, 54),
            //Color.FromArgb(byte.MaxValue, 66, 133, 244)

            Color.FromArgb(byte.MaxValue, 242, 80, 34),
            Color.FromArgb(byte.MaxValue, 255, 185, 0),
            Color.FromArgb(byte.MaxValue, 127, 186, 0),
            Color.FromArgb(byte.MaxValue, 0, 164, 239)
        };

        #region DependencyProperty


        /// <summary> Gets or sets the Thickness of<see cref = "ProgressRing" />. </summary>
        public double Thickness
        {
            get => (double)base.GetValue(ThicknessProperty);
            set => base.SetValue(ThicknessProperty, value);
        }
        /// <summary> Identifies the <see cref = "ProgressRing.Thickness" /> dependency property. </summary>
        public static readonly DependencyProperty ThicknessProperty = DependencyProperty.Register(nameof(Thickness), typeof(double), typeof(ProgressRing), new PropertyMetadata(6d, (sender, e) =>
        {
            ProgressRing control = (ProgressRing)sender;
            if (control.IsLoadedCore == false) return;

            if (e.NewValue is double value)
            {
                control.Update(control.ActualWidth, control.ActualHeight, value);
            }
        }));


        /// <summary> Gets or sets the value of<see cref = "ProgressRing" />. </summary>
        public double Value
        {
            get => (double)base.GetValue(ValueProperty);
            set => base.SetValue(ValueProperty, value);
        }
        /// <summary> Identifies the <see cref = "ProgressRing.Value" /> dependency property. </summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof(Value), typeof(double), typeof(ProgressRing), new PropertyMetadata(0d, (sender, e) =>
        {
            ProgressRing control = (ProgressRing)sender;

            if (e.NewValue is double value)
            {
                control.OutputAngle(value, out double Value, out int quotient, out int remainder);
                for (int i = 0; i < 4; i++)
                {
                    control.RotateTransforms[i].Angle = control.AngleConverter(i, Value, quotient, remainder);
                }
            }
        }));


        /// <summary> Gets or sets the state of<see cref = "ProgressRing" />. </summary>
        public bool IsActive
        {
            get => (bool)base.GetValue(IsActiveProperty);
            set => base.SetValue(IsActiveProperty, value);
        }
        /// <summary> Identifies the <see cref = "ProgressRing.IsActive" /> dependency property. </summary>
        public static readonly DependencyProperty IsActiveProperty = DependencyProperty.Register(nameof(IsActive), typeof(bool), typeof(ProgressRing), new PropertyMetadata(false, (sender, e) =>
        {
            ProgressRing control = (ProgressRing)sender;

            if (e.NewValue is bool value)
            {
                control.IsActivePropertyChanged(value);
            }
        }));

        private void IsActivePropertyChanged(bool value)
        {
            if (this.IsLoadedCore == false) return;

            if (value)
                this.Storyboard.Begin(); // Storyboard
            else
                this.Storyboard.Pause(); // Storyboard
        }


        #endregion

        bool IsLoadedCore;
        readonly Storyboard Storyboard = new Storyboard
        {
            RepeatBehavior = RepeatBehavior.Forever,
            Children =
            {
                new DoubleAnimation
                {
                    From = 0,
                    To = 360d * (4 + 4),
                    Duration = TimeSpan.FromSeconds(4),
                    EnableDependentAnimation = true,
                }
            }
        };

        //@Construct
        /// <summary>
        /// Initializes a ProgressRing. 
        /// </summary> 
        public ProgressRing()
        {
            {
                Timeline animation = this.Storyboard.Children.First();
                Storyboard.SetTarget(animation, this);
                Storyboard.SetTargetProperty(animation, nameof(Value));
            }
            base.Loaded += (s, e) =>
            {
                this.Initialize(base.ActualWidth, base.ActualHeight, this.Thickness);
                this.IsLoadedCore = true;
                this.IsActivePropertyChanged(this.IsActive);
            };
            base.SizeChanged += (s, e) =>
            {
                if (this.IsLoadedCore == false) return;
                if (e.NewSize == Size.Empty) return;
                if (e.NewSize == e.PreviousSize) return;

                this.Update(e.NewSize.Width, e.NewSize.Height, this.Thickness);
            };
        }


        private void Update(double width, double height, double thickness)
        {
            // double diameter = radius + radius + thickness;
            double diameter = Math.Max(40, Math.Min(width, height));
            double radius = (diameter - thickness) / 2;

            bool isLargeArc = this.IsLargeArcConverter(0.25);
            Point point = this.PointConverter(0.25, radius, thickness);
            Point startPoint = new Point(radius + thickness / 2, thickness / 2);

            foreach (ArcSegment arcSegment in this.ArcSegments)
            {
                arcSegment.Size = new Size(radius, radius);
                arcSegment.IsLargeArc = isLargeArc;
                arcSegment.Point = point;
            }

            foreach (PathFigure pathFigure in this.PathFigures)
            {
                pathFigure.StartPoint = startPoint;
            }

            foreach (Path path in this.Paths)
            {
                path.Width = diameter;
                path.Height = diameter;
                path.StrokeThickness = thickness;
            }
        }

        private void Initialize(double width, double height, double thickness)
        {
            // double diameter = radius + radius + thickness;
            double diameter = Math.Max(40, Math.Min(width, height));
            double radius = (diameter - thickness) / 2;

            base.Width = diameter;
            base.Height = diameter;

            for (int i = 0; i < 4; i++)
            {
                ArcSegment arcSegment = new ArcSegment
                {
                    Size = new Size(radius, radius),
                    IsLargeArc = this.IsLargeArcConverter(0.25),
                    Point = this.PointConverter(0.25, radius, thickness),
                    SweepDirection = SweepDirection.Counterclockwise
                };
                PathFigure pathFigure = new PathFigure
                {
                    StartPoint = new Point(radius + thickness / 2, thickness / 2),
                    Segments = { arcSegment }
                };
                RotateTransform rotateTransform = new RotateTransform();
                Path path = new Path
                {
                    Width = diameter,
                    Height = diameter,
                    StrokeThickness = thickness,
                    Stroke = new SolidColorBrush(this.Strokes[i]),
                    StrokeStartLineCap = PenLineCap.Flat,
                    StrokeEndLineCap = PenLineCap.Flat,
                    RenderTransformOrigin = new Point(0.5, 0.5),
                    RenderTransform = rotateTransform,
                    Data = new PathGeometry
                    {
                        Figures = { pathFigure }
                    }
                };

                this.Paths[i] = path;
                this.RotateTransforms[i] = rotateTransform;
                this.PathFigures[i] = pathFigure;
                this.ArcSegments[i] = arcSegment;

                base.Children.Add(path);
            }
        }

        private double AngleConverter(int i, double value2, int quotient, int remainder)
        {
            // Don't change this Magic Code !
            return 360d / 4 * (4 * remainder - i * remainder + quotient) + value2 / 4d * (4 - 3 * remainder + i * (2 * remainder - 1));
        }
        private void OutputAngle(double value, out double value2, out int quotient, out int remainder)
        {
            value2 = value % 360d;
            int num = (int)Math.Floor(value / 360d) % (4 + 4);
            quotient = num / 2;
            remainder = num % 2;
        }

        private bool IsLargeArcConverter(double percentage) => percentage > 0.5;
        private Point PointConverter(double percentage, double radius, double thickness)
        {
            if (percentage == 0d) return new Point(radius, 0);

            double angle = percentage / 0.5 * Math.PI;
            double sin = radius + radius * -Math.Sin(angle);
            double cos = radius + radius * -Math.Cos(angle);

            return new Point(sin + thickness / 2, cos + thickness / 2);
        }

    }
}