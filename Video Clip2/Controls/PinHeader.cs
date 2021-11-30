using System;
using Video_Clip2.Clips;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Video_Clip2.Controls
{
    public sealed class PinHeader : ContentControl
    {

        //@Delegate
        public event EventHandler<double> XChanged;

        #region DependencyProperty


        /// <summary> Gets or sets <see cref = "PinHeader" />'s x. </summary>
        public double X
        {
            get => (double)base.GetValue(XProperty);
            set => base.SetValue(XProperty, value);
        }
        /// <summary> Identifies the <see cref = "PinHeader.X" /> dependency property. </summary>
        public static readonly DependencyProperty XProperty = DependencyProperty.Register(nameof(X), typeof(double), typeof(PinHeader), new PropertyMetadata(0d, (sender, e) =>
        {
            PinHeader control = (PinHeader)sender;

            if (e.NewValue is double value)
            {
                Canvas.SetLeft(control, value + control.TrackLeftWidth - 6);
                control.XChanged?.Invoke(control, value); // Delegate
            }
        }));


        /// <summary> Gets or sets <see cref = "PinHeader" />'s track left width. </summary>
        public double TrackLeftWidth
        {
            get => (double)base.GetValue(TrackLeftWidthProperty);
            set => base.SetValue(TrackLeftWidthProperty, value);
        }
        /// <summary> Identifies the <see cref = "PinHeader.TrackLeftWidth" /> dependency property. </summary>
        public static readonly DependencyProperty TrackLeftWidthProperty = DependencyProperty.Register(nameof(TrackLeftWidth), typeof(double), typeof(PinHeader), new PropertyMetadata(72d, (sender, e) =>
        {
            PinHeader control = (PinHeader)sender;

            if (e.NewValue is double value)
            {
                Canvas.SetLeft(control, control.X + value - 6);
            }
        }));


        #endregion

        #region DependencyProperty


        /// <summary> Gets or sets <see cref = "PinHeader" />'s position. </summary>
        public TimeSpan Position
        {
            get => (TimeSpan)base.GetValue(PositionProperty);
            set => base.SetValue(PositionProperty, value);
        }
        /// <summary> Identifies the <see cref = "PinHeader.Position" /> dependency property. </summary>
        public static readonly DependencyProperty PositionProperty = DependencyProperty.Register(nameof(Position), typeof(TimeSpan), typeof(PinHeader), new PropertyMetadata(TimeSpan.FromMinutes(20), (sender, e) =>
        {
            PinHeader control = (PinHeader)sender;

            if (e.NewValue is TimeSpan value)
            {
                control.X = value.ToDouble(control.TrackScale);
            }
        }));


        /// <summary> Gets or sets <see cref = "PinHeader" />'s track scale. </summary>
        public double TrackScale
        {
            get => (double)base.GetValue(TrackScaleProperty);
            set => base.SetValue(TrackScaleProperty, value);
        }
        /// <summary> Identifies the <see cref = "PinHeader.TrackScale" /> dependency property. </summary>
        public static readonly DependencyProperty TrackScaleProperty = DependencyProperty.Register(nameof(TrackScale), typeof(double), typeof(PinHeader), new PropertyMetadata(16d, (sender, e) =>
        {
            PinHeader control = (PinHeader)sender;

            if (e.NewValue is double value)
            {
                control.X = control.Position.ToDouble(value);
            }
        }));


        #endregion

        public PinHeader()
        {
            Canvas.SetLeft(this, this.TrackLeftWidth - 6);
        }

    }
}