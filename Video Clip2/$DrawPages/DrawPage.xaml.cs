using System;
using Video_Clip2.Clips.Clips;
using Video_Clip2.ViewModels;
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
        private string TimeSpanToStringConverter(TimeSpan value) => value.ToString("mm':'ss'.'ff");
        private Symbol BooleanToMuteConverter(bool value) => value ? Symbol.Mute : Symbol.Volume;
        private Symbol BooleanToFreedomConverter(bool value) => value ? Symbol.MapPin : Symbol.Map;
        private Symbol BooleanToFullScreenConverter(bool value) => value ? Symbol.BackToWindow : Symbol.FullScreen;
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
                    control.TrackCanvas.Padding = new Thickness(0);
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

            base.Loaded += (s, e) =>
           {
               if (this.ViewModel.ObservableCollection.Count == 0)
                   this.AddRing.Start();
           };


            this.TrackComboBox.SelectionChanged += (s, e) =>
            {
                if (this.TrackComboBox.SelectedItem is int item)
                {
                    this.ViewModel.TrackHeight = item;
                }
            };

            this.PlayButton.Click += (s, e) =>
            {
                if (this.ViewModel.Position >= this.ViewModel.Duration)
                    this.ViewModel.Position = TimeSpan.Zero;

                this.ViewModel.IsPlaying = true;
                this.PlayRing.Ding();
                this.PauseButton.Focus(FocusState.Programmatic);
            };
            this.PauseButton.Click += (s, e) =>
            {
                this.ViewModel.IsPlaying = false;
                this.PlayRing.Ding();
                this.PlayButton.Focus(FocusState.Programmatic);
            };

            this.AddButton.Click += (s, e) => this.AddFlyout.ShowAt(this.AddButton);
            this.AddButton.RightTapped += (s, e) => this.AddFlyout.ShowAt(this.AddButton);
            this.AddButton.Holding += (s, e) => this.AddFlyout.ShowAt(this.AddButton);
            this.AddFlyout.Opened += (s, e) => this.AddRing.Stop();

            this.PinButton.Click += (s, e) => Controls.PinCanvas.Pin(this.ViewModel.Position, this.ViewModel.PinCollection);
            this.TrimButton.Click += (s, e) => this.ViewModel.MethodEditTrim();
            this.ColorPicker.ColorChanged += (s, e) =>
            {
                foreach (IClip item in this.ViewModel.ObservableCollection)
                {
                    if (item.IsSelected)
                    {
                        if (item is Clips.Models.ColorClip colorClip)
                        {
                            colorClip.SetColor(e.NewColor);
                        }
                    }
                }
                this.ViewModel.Invalidate(); // Invalidate
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