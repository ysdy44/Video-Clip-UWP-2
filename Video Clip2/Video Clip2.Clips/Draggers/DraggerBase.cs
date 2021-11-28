using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Video_Clip2.Clips.Draggers
{
    public abstract partial class DraggerBase : UserControl
    {

        //@Abstract
        public abstract void Update(ListViewSelectionMode selectionMode, Trimmer trimmer, double trackHeight);


        #region DependencyProperty


        /// <summary> Gets or sets <see cref = "DraggerBase" />'s selection-mode. </summary>
        public ListViewSelectionMode SelectionMode
        {
            get => (ListViewSelectionMode)base.GetValue(SelectionModeProperty);
            set => base.SetValue(SelectionModeProperty, value);
        }
        /// <summary> Identifies the <see cref = "DraggerBase.SelectionMode" /> dependency property. </summary>
        public static readonly DependencyProperty SelectionModeProperty = DependencyProperty.Register(nameof(SelectionMode), typeof(ListViewSelectionMode), typeof(DraggerBase), new PropertyMetadata(ListViewSelectionMode.None, (sender, e) =>
        {
            DraggerBase control = (DraggerBase)sender;

            if (e.NewValue is ListViewSelectionMode value)
            {
                control.Update(value, control.Trimmer, control.TrackHeight);
            }
        }));


        /// <summary> Gets or sets <see cref = "DraggerBase" />'s trimmer. </summary>
        public Trimmer Trimmer
        {
            get => (Trimmer)base.GetValue(TrimmerProperty);
            set => base.SetValue(TrimmerProperty, value);
        }
        /// <summary> Identifies the <see cref = "DraggerBase.Trimmer" /> dependency property. </summary>
        public static readonly DependencyProperty TrimmerProperty = DependencyProperty.Register(nameof(Trimmer), typeof(Trimmer), typeof(DraggerBase), new PropertyMetadata(new Trimmer(), (sender, e) =>
        {
            DraggerBase control = (DraggerBase)sender;

            if (e.NewValue is Trimmer value)
            {
                control.Update(control.SelectionMode, value, control.TrackHeight);
            }
        }));


        /// <summary> Gets or sets <see cref = "DraggerBase" />'s track height. </summary>
        public double TrackHeight
        {
            get => (double)base.GetValue(TrackScaleProperty);
            set => base.SetValue(TrackScaleProperty, value);
        }
        /// <summary> Identifies the <see cref = "DraggerBase.TrackHeight" /> dependency property. </summary>
        public static readonly DependencyProperty TrackScaleProperty = DependencyProperty.Register(nameof(TrackHeight), typeof(double), typeof(DraggerBase), new PropertyMetadata(50d, (sender, e) =>
        {
            DraggerBase control = (DraggerBase)sender;

            if (e.NewValue is double value)
            {
                control.Update(control.SelectionMode, control.Trimmer, value);
            }
        }));


        #endregion

    }
}