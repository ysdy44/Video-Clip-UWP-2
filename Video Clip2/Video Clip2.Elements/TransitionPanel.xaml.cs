using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Video_Clip2.Elements
{
    /// <summary>
    /// Element of <see cref="TransitionPanel"/>.
    /// </summary>
    public interface ITransitionElement
    {
        /// <summary> The current page no longer becomes an active page. </summary>
        void OnNavigatedFrom();

        /// <summary> The current page becomes the active page. </summary>
        void OnNavigatedTo();
    }

    /// <summary>
    /// Represents a panel that with transition.
    /// </summary>
    public partial class TransitionPanel : UserControl
    {

        #region DependencyProperty


        /// <summary> Gets or set the orientation for <see cref="TransitionPanel"/>. </summary>
        public Orientation Orientation
        {
            get => (Orientation)base.GetValue(OrientationProperty);
            set => SetValue(OrientationProperty, value);
        }
        /// <summary> Identifies the <see cref = "TransitionPanel.Orientation" /> dependency property. </summary>
        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(nameof(Orientation), typeof(Orientation), typeof(TransitionPanel), new PropertyMetadata(Orientation.Vertical));


        /// <summary> Gets or set the visual state for <see cref="TransitionPanel"/>, Default False. </summary>
        public bool IsShow
        {
            get => (bool)base.GetValue(IsShowProperty);
            set => SetValue(IsShowProperty, value);
        }
        protected static readonly DependencyProperty IsShowProperty = DependencyProperty.Register(nameof(IsShow), typeof(bool), typeof(TransitionPanel), new PropertyMetadata(false, (sender, e) =>
        {
            TransitionPanel control = (TransitionPanel)sender;

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


        //@Construct
        /// <summary>
        /// Initializes a TransitionPanel. 
        /// </summary> 
        public TransitionPanel()
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