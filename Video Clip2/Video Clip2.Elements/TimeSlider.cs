using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace Video_Clip2.Elements
{
    public sealed partial class TimeSlider : Slider
    {
        //@Delegate
        /// <summary> Occurs when the value changed starts. </summary>
        public event EventHandler ValueChangedStarted;
        /// <summary> Occurs when value changed. </summary>
        public event RangeBaseValueChangedEventHandler ValueChangedDelta;
        /// <summary> Occurs when the value changed is complete. </summary>
        public event EventHandler ValueChangedCompleted;

        VisualStateGroup CommonStates;
        VisualState Normal;
        VisualState PointerOver;
        VisualState Pressed;
        VisualState Disabled;

        //@Converter
        public double GetPercentage(double value) => (value + base.Minimum) / (base.Maximum - base.Minimum);
        public double Percentage
        {
            get => this.GetPercentage(base.Value);
            set => base.Value = value * (base.Maximum - base.Minimum) + base.Minimum;
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            //VisualStateGroup
            if (!(this.CommonStates is null)) this.CommonStates.CurrentStateChanged -= this.CommonStates_CurrentStateChanged;
            this.CommonStates = base.GetTemplateChild(nameof(CommonStates)) as VisualStateGroup;
            if (!(this.CommonStates is null)) this.CommonStates.CurrentStateChanged += this.CommonStates_CurrentStateChanged;
            //VisualState
            this.Normal = base.GetTemplateChild(nameof(Normal)) as VisualState;
            this.PointerOver = base.GetTemplateChild(nameof(PointerOver)) as VisualState;
            this.Pressed = base.GetTemplateChild(nameof(Pressed)) as VisualState;
            this.Disabled = base.GetTemplateChild(nameof(Disabled)) as VisualState;
        }

        private void CommonStates_CurrentStateChanged(object sender, VisualStateChangedEventArgs e)
        {
            if (e.OldState != this.Pressed && e.NewState == this.Pressed)
            {
                this.ValueChangedStarted?.Invoke(sender, new EventArgs());//Delegate
                base.ValueChanged += this.ValueChangedDelta;//Add Delegate
            }
            if (e.OldState == this.Pressed && e.NewState != this.Pressed)
            {
                this.ValueChangedCompleted?.Invoke(sender, new EventArgs());//Delegate
                base.ValueChanged -= this.ValueChangedDelta;//Remove Delegate
            }
        }
    }
}