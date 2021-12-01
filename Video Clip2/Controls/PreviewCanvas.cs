using System;
using Windows.Foundation;
using Windows.Graphics.Imaging;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace Video_Clip2.Controls
{
    public sealed partial class PreviewCanvas : Canvas
    {

        double StartingHeight;
        double StartingRectW;
        double StartingRectH;
        double StartingRectX;
        double StartingRectY;

        bool IsSpliterManipulationStarted;
        bool IsAnimationStarted;

        #region DependencyProperty


        public double Scale2
        {
            get => (double)base.GetValue(Scale2Property);
            set => SetValue(Scale2Property, value);
        }
        /// <summary> Identifies the <see cref = "PreviewCanvas.Scale2" /> dependency property. </summary>
        public static readonly DependencyProperty Scale2Property = DependencyProperty.Register(nameof(Scale2), typeof(double), typeof(PreviewCanvas), new PropertyMetadata(1d, (sender, e) =>
        {
            PreviewCanvas control = (PreviewCanvas)sender;

            if (e.NewValue is double value)
            {
                double width = control.ActualWidth;
                double height = control.ActualHeight;
                double scale = value;
                BitmapSize size = control.Size2;

                control.UpdateRect(width, height, scale, size);
            }
        }));


        public BitmapSize Size2
        {
            get => (BitmapSize)base.GetValue(Size2Property);
            set => SetValue(Size2Property, value);
        }
        /// <summary> Identifies the <see cref = "PreviewCanvas.Size2" /> dependency property. </summary>
        public static readonly DependencyProperty Size2Property = DependencyProperty.Register(nameof(Size2), typeof(BitmapSize), typeof(PreviewCanvas), new PropertyMetadata(new BitmapSize
        {
            Width = 1920,
            Height = 1080
        }, (sender, e) =>
        {
            PreviewCanvas control = (PreviewCanvas)sender;

            if (e.NewValue is BitmapSize value)
            {
                double width = control.ActualWidth;
                double height = control.ActualHeight;
                double scale = control.Scale2;
                BitmapSize size = value;

                control.UpdateRect(width, height, scale, size);
            }
        }));


        /// <summary> Gets or set the full-screen state for <see cref="PreviewCanvas"/>. </summary>
        public bool IsFullScreen
        {
            get => (bool)base.GetValue(IsFullScreenProperty);
            set => SetValue(IsFullScreenProperty, value);
        }
        /// <summary> Identifies the <see cref = "PreviewCanvas.IsFullScreen" /> dependency property. </summary>
        public static readonly DependencyProperty IsFullScreenProperty = DependencyProperty.Register(nameof(IsFullScreen), typeof(bool), typeof(PreviewCanvas), new PropertyMetadata(false));


        #endregion

        public PreviewCanvas()
        {
            base.SizeChanged += (s, e) =>
            {
                if (e.NewSize == Size.Empty) return;
                if (e.NewSize == e.PreviousSize) return;

                double width = e.NewSize.Width;
                double height = e.NewSize.Height;

                if (this.IsSpliterManipulationStarted)
                {
                }
                else if (this.IsAnimationStarted)
                {
                    this.Delta(height);
                }
                else
                {
                    BitmapSize size = this.Size2;
                    double scale = PreviewCanvas.GetScale(width, height, size);

                    if (this.Scale2 != scale)
                        this.Scale2 = scale;
                    else
                        this.UpdateRect(width, height, scale, size);
                }
            };
        }


        public void SetWindowHeight(double windowHeight)
        {
            if (this.IsFullScreen)
            {
                base.Height = windowHeight;
                base.MaxHeight = double.PositiveInfinity;
            }
            else
            {
                base.MaxHeight = windowHeight;
            }
        }
        public void FullScreenCompleted()
        {
            this.IsAnimationStarted = false;

            if (this.IsFullScreen)
            {
                this.IsFullScreen = false;
                this.Completed(276);
            }
            else
            {
                this.IsFullScreen = true;
                this.Completed(base.ActualHeight);
            }
        }
        public double FullScreenStarted(double windowHeight)
        {
            this.IsAnimationStarted = true;
            this.Started();

            if (this.IsFullScreen)
                return 276;
            else
                return windowHeight;
        }


        public void SpliterManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            this.IsSpliterManipulationStarted = true;
            this.Started();
        }
        public void SpliterManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            this.Delta(this.StartingHeight + e.Cumulative.Translation.Y);
        }
        public void SpliterManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            this.IsFullScreen = false;

            this.IsSpliterManipulationStarted = false;
            this.Completed(this.StartingHeight + e.Cumulative.Translation.Y);
        }


        private void Started()
        {
            double width = base.ActualWidth;
            double height = base.ActualHeight;
            BitmapSize size = this.Size2;
            double scale = this.Scale2;

            PreviewCanvas.GetRect(width, height, scale, size, out double w, out double h, out double x, out double y);

            this.StartingHeight = height;
            this.StartingRectW = w;
            this.StartingRectH = h;
            this.StartingRectX = x;
            this.StartingRectY = y;
        }
        private void Delta(double targetHeight)
        {
            double width = base.ActualWidth;
            double height = Math.Min(base.MaxHeight, Math.Max(base.MinHeight, targetHeight));
            BitmapSize size = this.Size2;
            double scale = PreviewCanvas.GetScale(width, height, size);

            PreviewCanvas.GetRect(width, height, scale, size, out double w, out double h, out double x, out double y);

            base.Height = height;
            foreach (FrameworkElement item in base.Children)
            {
                if (item.RenderTransform is CompositeTransform transform)
                {
                    transform.ScaleX = w / this.StartingRectW;
                    transform.ScaleY = h / this.StartingRectH;
                    transform.TranslateX = x - this.StartingRectX;
                    transform.TranslateY = y - this.StartingRectY;
                }
            }
        }
        private void Completed(double targetHeight)
        {
            double width = base.ActualWidth;
            double height = Math.Min(base.MaxHeight, Math.Max(base.MinHeight, targetHeight));
            BitmapSize size = this.Size2;
            double scale = PreviewCanvas.GetScale(width, height, size);

            base.Height = height;
            foreach (FrameworkElement item in base.Children)
            {
                if (item.RenderTransform is CompositeTransform transform)
                {
                    transform.TranslateX = 0;
                    transform.TranslateY = 0;
                    transform.ScaleX = 1;
                    transform.ScaleY = 1;
                }
            }

            if (this.Scale2 != scale)
                this.Scale2 = scale;
            else
                this.UpdateRect(width, height, scale, size);
        }


        private void UpdateRect(double width, double height, double scale, BitmapSize size)
        {
            PreviewCanvas.GetRect(width, height, scale, size, out double w, out double h, out double x, out double y);

            foreach (FrameworkElement item in base.Children)
            {
                item.Width = w;
                item.Height = h;
                Canvas.SetLeft(item, x);
                Canvas.SetTop(item, y);
            }
        }

        private static void GetRect(double width, double height, double scale, BitmapSize size, out double w, out double h, out double x, out double y)
        {
            w = scale * size.Width;
            h = scale * size.Height;
            x = (width - w) / 2;
            y = (height - h) / 2;
        }

        private static double GetScale(double width, double height, BitmapSize size)
        {
            return Math.Min(width / size.Width, height / size.Height);
        }

    }
}
