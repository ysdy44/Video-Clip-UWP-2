using System;
using Video_Clip2.Elements;
using Video_Clip2.ViewModels;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Video_Clip2
{
    public sealed partial class DrawPage : Page
    {

        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;
        bool IsCtrl => Window.Current.CoreWindow.GetKeyState(Windows.System.VirtualKey.Control) == Windows.UI.Core.CoreVirtualKeyStates.Down;
        bool IsWheelForTrackScale => this.IsCtrl;

        //@Converter
        private bool IntToBooleanConverter(int value) => value == 0;
        private string TimeSpanToStringConverter(TimeSpan value) => value.ToText();
        private Symbol BooleanToPinConverter(bool value) => value ? Symbol.UnPin : Symbol.Pin;
        private Symbol BooleanToMuteConverter(bool value) => value ? Symbol.Mute : Symbol.Volume;
        private Symbol BooleanToFreedomConverter(bool value) => value ? Symbol.MapPin : Symbol.Map;
        private Symbol BooleanToFullScreenConverter(bool value) => value ? Symbol.BackToWindow : Symbol.FullScreen;
        private Visibility IntToVisibilityConverter(int value) => value == 0 ? Visibility.Visible : Visibility.Collapsed;
        private Visibility ReverseIntToVisibilityConverter(int value) => value == 0 ? Visibility.Collapsed : Visibility.Visible;
        private Visibility BooleanToVisibilityConverter(bool value) => value ? Visibility.Visible : Visibility.Collapsed;
        private Visibility ReverseBooleanToVisibilityConverter(bool value) => value ? Visibility.Collapsed : Visibility.Visible;


        #region DependencyProperty


        /// <summary> Gets or set the group index for <see cref="DrawPage"/>, Default 0. </summary>
        public int GroupIndex
        {
            get => (int)base.GetValue(GroupIndexProperty);
            set => SetValue(GroupIndexProperty, value);
        }
        /// <summary> Identifies the <see cref = "DrawPage.GroupIndex" /> dependency property. </summary>
        public static readonly DependencyProperty GroupIndexProperty = DependencyProperty.Register(nameof(GroupIndex), typeof(int), typeof(DrawPage), new PropertyMetadata(0));


        /// <summary> Gets or set the loading state for <see cref="DrawPage"/>. </summary>
        public bool IsLoading
        {
            get => (bool)base.GetValue(IsLoadingProperty);
            set => SetValue(IsLoadingProperty, value);
        }
        /// <summary> Identifies the <see cref = "DrawPage.IsLoading" /> dependency property. </summary>
        public static readonly DependencyProperty IsLoadingProperty = DependencyProperty.Register(nameof(IsLoading), typeof(bool), typeof(DrawPage), new PropertyMetadata(false, (sender, e) =>
        {
            DrawPage control = (DrawPage)sender;

            if (e.NewValue is bool value)
            {
                if (value)
                {
                    control.LoadingDialog.ShowAsync(ContentDialogPlacement.InPlace).GetResults();
                }
                else
                {
                    control.LoadingDialog.Hide();
                }
            }
        }));


        /// <summary> Gets or set the freedom state for <see cref="DrawPage"/>. </summary>
        public bool IsDirectFreedom
        {
            get => (bool)base.GetValue(IsDirectFreedomProperty);
            set => SetValue(IsDirectFreedomProperty, value);
        }
        /// <summary> Identifies the <see cref = "DrawPage.IsDirectFreedom" /> dependency property. </summary>
        public static readonly DependencyProperty IsDirectFreedomProperty = DependencyProperty.Register(nameof(IsDirectFreedom), typeof(bool), typeof(DrawPage), new PropertyMetadata(false, (sender, e) =>
        {
            DrawPage control = (DrawPage)sender;

            if (e.NewValue is bool value)
            {
                if (value)
                {
                    control.UpdateTrackWidth();
                }
                else
                {
                    control.UpdateTrackWidth(control.TrackScrollViewer.ActualWidth);
                    control.UpdateTrackX();
                }
            }
        }));


        #endregion


        public DrawPage()
        {
            this.InitializeComponent();

            this.ConstructPreview();
            this.ConstructPosition();

            this.ConstructCanvas();
            this.ConstructScrollViewer();

            this.ConstructAdd();
            this.ConstructMenu();
            this.ConstructEdit();


            this.TrackComboBox.SelectionChanged += (s, e) =>
            {
                if (this.TrackComboBox.SelectedItem is int item)
                {
                    this.ViewModel.TrackHeight = item;
                }
            };
        }
    }

    public sealed partial class DrawPage : Page
    {

        //@BackRequested
        /// <summary> The current page no longer becomes an active page. </summary>
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.ViewModel.Canvas.Remove(this.PreviewCanvas);
        }
        /// <summary> The current page becomes the active page. </summary>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.ViewModel.Canvas.Add(this.PreviewCanvas);
        }

    }
}