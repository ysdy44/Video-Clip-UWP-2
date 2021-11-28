using Windows.UI.Xaml;

namespace Video_Clip2.Elements.Transitions
{
    public sealed partial class IndexTransitionPanel : TransitionPanelBase
    {

        #region DependencyProperty


        /// <summary> Gets or set the group index for <see cref="IndexTransitionPanel"/>, Default -1. </summary>
        public int GroupIndex
        {
            get => (int)base.GetValue(GroupIndexProperty);
            set => SetValue(GroupIndexProperty, value);
        }
        /// <summary> Identifies the <see cref = "IndexTransitionPanel.GroupIndex" /> dependency property. </summary>
        public static readonly DependencyProperty GroupIndexProperty = DependencyProperty.Register(nameof(GroupIndex), typeof(int), typeof(IndexTransitionPanel), new PropertyMetadata(-1, (sender, e) =>
        {
            IndexTransitionPanel control = (IndexTransitionPanel)sender;
       
            if (e.NewValue is int value)
            {
                control.IsShowCore = value == control.Index;
            }
        }));


        /// <summary> Gets or set the curennt index for <see cref="IndexTransitionPanel"/>, Default 0. </summary>
        public int Index
        {
            get => (int)base.GetValue(IsShowProperty);
            set => SetValue(IsShowProperty, value);
        }
        /// <summary> Identifies the <see cref = "IndexTransitionPanel.Index" /> dependency property. </summary>
        public static readonly DependencyProperty IsShowProperty = DependencyProperty.Register(nameof(Index), typeof(int), typeof(IndexTransitionPanel), new PropertyMetadata(0, (sender, e) =>
        {
            IndexTransitionPanel control = (IndexTransitionPanel)sender;

            if (e.NewValue is int value)
            {
                control.IsShowCore = value == control.GroupIndex;
            }
        }));


        #endregion

    }
}