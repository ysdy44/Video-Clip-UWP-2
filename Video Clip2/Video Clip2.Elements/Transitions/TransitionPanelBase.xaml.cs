using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Video_Clip2.Elements.Transitions
{
    public abstract partial class TransitionPanelBase : UserControl
    {

        #region DependencyProperty


        /// <summary> Gets or set the orientation for <see cref="TransitionPanelBase"/>. </summary>
        public Orientation Orientation
        {
            get => (Orientation)base.GetValue(OrientationProperty);
            set => SetValue(OrientationProperty, value);
        }
        /// <summary> Identifies the <see cref = "TransitionPanelBase.Orientation" /> dependency property. </summary>
        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(nameof(Orientation), typeof(Orientation), typeof(TransitionPanelBase), new PropertyMetadata(Orientation.Vertical));


        protected bool IsShowCore
        {
            get => (bool)base.GetValue(IsShowCoreProperty);
            set => SetValue(IsShowCoreProperty, value);
        }
        protected static readonly DependencyProperty IsShowCoreProperty = DependencyProperty.Register(nameof(IsShowCore), typeof(bool), typeof(TransitionPanelBase), new PropertyMetadata(false, (sender, e) =>
        {
            TransitionPanelBase control = (TransitionPanelBase)sender;

            if (e.NewValue is bool value)
            {
                (value ? control.ShowStoryboard : control.HideStoryboard).Begin(); // Storyboard
                switch (control.Orientation)
                {
                    case Orientation.Vertical:
                        (value ? control.ShowYStoryboard : control.HideYStoryboard).Begin(); // Storyboard
                        break;
                    case Orientation.Horizontal:
                        (value ? control.ShowXStoryboard : control.HideXStoryboard).Begin(); // Storyboard
                        break;
                    default:
                        break;
                }
            }
        }));


        #endregion


        public TransitionPanelBase()
        {
            this.InitializeComponent();
            this.HideStoryboard.Completed += (s, e) =>
            {
                if (base.Content is ITransitionElement child)
                {
                    child.OnNavigatedFrom();
                }
            };
            this.ShowStoryboard.Completed += (s, e) =>
            {
                if (base.Content is ITransitionElement child)
                {
                    child.OnNavigatedTo();
                }
            };
        }
    }
}