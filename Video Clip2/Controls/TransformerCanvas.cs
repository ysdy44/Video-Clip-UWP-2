using FanKit.Transformers;
using System.Numerics;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace Video_Clip2.Controls
{
    public sealed class TransformerCanvas : Canvas
    {

        readonly Line LeftLineBlack;
        readonly Line TopLineBlack;
        readonly Line RightLineBlack;
        readonly Line BottomLineBlack;

        readonly Ellipse LeftTopEllipseBlack;
        readonly Ellipse LeftRightEllipseBlack;
        readonly Ellipse RightBottomEllipseBlack;
        readonly Ellipse LeftBottomEllipseBlack;


        readonly Line LeftLineWhite;
        readonly Line TopLineWhite;
        readonly Line RightLineWhite;
        readonly Line BottomLineWhite;

        readonly Ellipse LeftTopEllipseWhite;
        readonly Ellipse LeftRightEllipseWhite;
        readonly Ellipse RightBottomEllipseWhite;
        readonly Ellipse LeftBottomEllipseWhite;


        readonly Brush BorderBrush = new SolidColorBrush(Colors.Black);
        readonly Brush Foreground = new SolidColorBrush(Colors.White);


        #region DependencyProperty


        public double Scale2
        {
            get => (double)base.GetValue(Scale2Property);
            set => SetValue(Scale2Property, value);
        }
        /// <summary> Identifies the <see cref = "TransformerCanvas.Scale2" /> dependency property. </summary>
        public static readonly DependencyProperty Scale2Property = DependencyProperty.Register(nameof(Scale2), typeof(double), typeof(TransformerCanvas), new PropertyMetadata(1d, (sender, e) =>
        {
            TransformerCanvas control = (TransformerCanvas)sender;

            if (e.NewValue is double value)
            {
                float scale = (float)value;
                control.Transform(control.Transformer.LeftTop * scale, control.Transformer.RightTop * scale, control.Transformer.RightBottom * scale, control.Transformer.LeftBottom * scale);
            }
        }));


        public Transformer Transformer
        {
            get => (Transformer)base.GetValue(TransformerProperty);
            set => SetValue(TransformerProperty, value);
        }
        /// <summary> Identifies the <see cref = "TransformerCanvas.Transformer" /> dependency property. </summary>
        public static readonly DependencyProperty TransformerProperty = DependencyProperty.Register(nameof(Transformer), typeof(Transformer), typeof(TransformerCanvas), new PropertyMetadata(Transformer.One, (sender, e) =>
        {
            TransformerCanvas control = (TransformerCanvas)sender;

            if (e.NewValue is Transformer value)
            {
                float scale = (float)control.Scale2;
                control.Transform(value.LeftTop * scale, value.RightTop * scale, value.RightBottom * scale, value.LeftBottom * scale);
            }
        }));


        #endregion


        public TransformerCanvas()
        {
            this.LeftLineBlack = new Line { StrokeThickness = 3, Stroke = this.BorderBrush };
            this.TopLineBlack = new Line { StrokeThickness = 3, Stroke = this.BorderBrush };
            this.RightLineBlack = new Line { StrokeThickness = 3, Stroke = this.BorderBrush };
            this.BottomLineBlack = new Line { StrokeThickness = 3, Stroke = this.BorderBrush };

            this.LeftTopEllipseBlack = new Ellipse { Width = 20, Height = 20, Fill = this.BorderBrush };
            this.LeftRightEllipseBlack = new Ellipse { Width = 20, Height = 20, Fill = this.BorderBrush };
            this.RightBottomEllipseBlack = new Ellipse { Width = 20, Height = 20, Fill = this.BorderBrush };
            this.LeftBottomEllipseBlack = new Ellipse { Width = 20, Height = 20, Fill = this.BorderBrush };


            this.LeftLineWhite = new Line { StrokeThickness = 2, Stroke = this.Foreground };
            this.TopLineWhite = new Line { StrokeThickness = 2, Stroke = this.Foreground };
            this.RightLineWhite = new Line { StrokeThickness = 2, Stroke = this.Foreground };
            this.BottomLineWhite = new Line { StrokeThickness = 2, Stroke = this.Foreground };

            this.LeftTopEllipseWhite = new Ellipse { Width = 18, Height = 18, Fill = this.Foreground };
            this.LeftRightEllipseWhite = new Ellipse { Width = 18, Height = 18, Fill = this.Foreground };
            this.RightBottomEllipseWhite = new Ellipse { Width = 18, Height = 18, Fill = this.Foreground };
            this.LeftBottomEllipseWhite = new Ellipse { Width = 18, Height = 18, Fill = this.Foreground };



            base.Children.Add(this.LeftLineBlack);
            base.Children.Add(this.TopLineBlack);
            base.Children.Add(this.RightLineBlack);
            base.Children.Add(this.BottomLineBlack);

            base.Children.Add(this.LeftTopEllipseBlack);
            base.Children.Add(this.LeftRightEllipseBlack);
            base.Children.Add(this.RightBottomEllipseBlack);
            base.Children.Add(this.LeftBottomEllipseBlack);


            base.Children.Add(this.LeftLineWhite);
            base.Children.Add(this.TopLineWhite);
            base.Children.Add(this.RightLineWhite);
            base.Children.Add(this.BottomLineWhite);

            base.Children.Add(this.LeftTopEllipseWhite);
            base.Children.Add(this.LeftRightEllipseWhite);
            base.Children.Add(this.RightBottomEllipseWhite);
            base.Children.Add(this.LeftBottomEllipseWhite);
        }

        public void Transform(Vector2 leftTop, Vector2 rightTop, Vector2 rightBottom, Vector2 leftBottom)
        {
            this.LeftLineBlack.X1 = leftBottom.X;
            this.LeftLineBlack.Y1 = leftBottom.Y;
            this.LeftLineBlack.X2 = leftTop.X;
            this.LeftLineBlack.Y2 = leftTop.Y;

            this.TopLineBlack.X1 = leftTop.X;
            this.TopLineBlack.Y1 = leftTop.Y;
            this.TopLineBlack.X2 = rightTop.X;
            this.TopLineBlack.Y2 = rightTop.Y;

            this.RightLineBlack.X1 = rightTop.X;
            this.RightLineBlack.Y1 = rightTop.Y;
            this.RightLineBlack.X2 = rightBottom.X;
            this.RightLineBlack.Y2 = rightBottom.Y;

            this.BottomLineBlack.X1 = rightBottom.X;
            this.BottomLineBlack.Y1 = rightBottom.Y;
            this.BottomLineBlack.X2 = leftBottom.X;
            this.BottomLineBlack.Y2 = leftBottom.Y;

            Canvas.SetLeft(this.LeftTopEllipseBlack, leftTop.X - 10);
            Canvas.SetTop(this.LeftTopEllipseBlack, leftTop.Y - 10);

            Canvas.SetLeft(this.LeftRightEllipseBlack, rightTop.X - 10);
            Canvas.SetTop(this.LeftRightEllipseBlack, rightTop.Y - 10);

            Canvas.SetLeft(this.RightBottomEllipseBlack, rightBottom.X - 10);
            Canvas.SetTop(this.RightBottomEllipseBlack, rightBottom.Y - 10);

            Canvas.SetLeft(this.LeftBottomEllipseBlack, leftBottom.X - 10);
            Canvas.SetTop(this.LeftBottomEllipseBlack, leftBottom.Y - 10);


            this.LeftLineWhite.X1 = leftBottom.X;
            this.LeftLineWhite.Y1 = leftBottom.Y;
            this.LeftLineWhite.X2 = leftTop.X;
            this.LeftLineWhite.Y2 = leftTop.Y;

            this.TopLineWhite.X1 = leftTop.X;
            this.TopLineWhite.Y1 = leftTop.Y;
            this.TopLineWhite.X2 = rightTop.X;
            this.TopLineWhite.Y2 = rightTop.Y;

            this.RightLineWhite.X1 = rightTop.X;
            this.RightLineWhite.Y1 = rightTop.Y;
            this.RightLineWhite.X2 = rightBottom.X;
            this.RightLineWhite.Y2 = rightBottom.Y;

            this.BottomLineWhite.X1 = rightBottom.X;
            this.BottomLineWhite.Y1 = rightBottom.Y;
            this.BottomLineWhite.X2 = leftBottom.X;
            this.BottomLineWhite.Y2 = leftBottom.Y;

            Canvas.SetLeft(this.LeftTopEllipseWhite, leftTop.X - 9);
            Canvas.SetTop(this.LeftTopEllipseWhite, leftTop.Y - 9);

            Canvas.SetLeft(this.LeftRightEllipseWhite, rightTop.X - 9);
            Canvas.SetTop(this.LeftRightEllipseWhite, rightTop.Y - 9);

            Canvas.SetLeft(this.RightBottomEllipseWhite, rightBottom.X - 9);
            Canvas.SetTop(this.RightBottomEllipseWhite, rightBottom.Y - 9);

            Canvas.SetLeft(this.LeftBottomEllipseWhite, leftBottom.X - 9);
            Canvas.SetTop(this.LeftBottomEllipseWhite, leftBottom.Y - 9);
        }

    }
}