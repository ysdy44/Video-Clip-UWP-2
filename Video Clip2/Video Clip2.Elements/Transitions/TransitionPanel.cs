using Windows.UI.Xaml;

namespace Video_Clip2.Elements.Transitions
{
    public sealed partial class TransitionPanel : TransitionPanelBase
    {

        #region DependencyProperty


        /// <summary> Gets or set the visual state for <see cref="TransitionPanel"/>, Default False. </summary>
        public bool? IsShow
        {
            get => (bool?)base.GetValue(IsShowProperty);
            set => SetValue(IsShowProperty, value);
        }
        /// <summary> Identifies the <see cref = "TransitionPanel.IsShow" /> dependency property. </summary>
        public static readonly DependencyProperty IsShowProperty = DependencyProperty.Register(nameof(IsShow), typeof(bool?), typeof(TransitionPanel), new PropertyMetadata(false, (sender, e) =>
        {
            TransitionPanel control = (TransitionPanel)sender;

            control.IsShowCore = (e.NewValue is bool value) ? value : false;
        }));


        #endregion

    }
}