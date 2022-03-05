using System;
using Video_Clip2.Elements;
using Video_Clip2.ViewModels;
using Windows.ApplicationModel.Core;
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
        private bool IntToBooleanConverter(int value) => value == -1;
        private string TimeSpanToStringConverter(TimeSpan value) => value.ToText();
        private Symbol BooleanToPinConverter(bool value) => value ? Symbol.UnPin : Symbol.Pin;
        private Symbol BooleanToMuteConverter(bool value) => value ? Symbol.Mute : Symbol.Volume;
        private Symbol BooleanToFreedomConverter(bool value) => value ? Symbol.MapPin : Symbol.Map;
        private Visibility ReverseIntToVisibilityConverter(int value) => value == -1 ? Visibility.Collapsed : Visibility.Visible;
        private bool ReverseIntToBooleanConverter(int value) => value != -1;
        private Visibility BooleanToVisibilityConverter(bool value) => value ? Visibility.Visible : Visibility.Collapsed;
        private Visibility ReverseBooleanToVisibilityConverter(bool value) => value ? Visibility.Collapsed : Visibility.Visible;


        #region DependencyProperty


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


        /// <summary> Gets or set the full-screen state for <see cref="DrawPage"/>. </summary>
        public bool IsFullScreen
        {
            get => (bool)base.GetValue(IsFullScreenProperty);
            set => SetValue(IsFullScreenProperty, value);
        }
        /// <summary> Identifies the <see cref = "DrawPage.IsFullScreen" /> dependency property. </summary>
        public static readonly DependencyProperty IsFullScreenProperty = DependencyProperty.Register(nameof(IsFullScreen), typeof(bool), typeof(DrawPage), new PropertyMetadata(false, (sender, e) =>
        {
            DrawPage control = (DrawPage)sender;

            if (e.NewValue is bool value)
            {
                control._vsIsFullScreen = value;
                control.VisualState = control.VisualState; // VisualState
            }
        }));


        #endregion


        //@VisualState
        bool _vsIsFullScreen = false;
        DeviceLayoutType _vsDeviceLayoutType = DeviceLayoutType.PC;
        /// <summary> 
        /// Represents the visual appearance of UI elements in a specific state.
        /// </summary>
        public VisualState VisualState
        {
            get
            {
                if (this._vsIsFullScreen) return this.FullScreen;
                else
                {
                    switch (this._vsDeviceLayoutType)
                    {
                        case DeviceLayoutType.PC: return this.PC;
                        case DeviceLayoutType.Pad: return this.Pad;
                        case DeviceLayoutType.Phone: return this.Phone;
                        default: return this.PC;
                    }
                }
            }
            set => VisualStateManager.GoToState(this, value.Name, true);
        }
        DeviceLayout DeviceLayout = DeviceLayout.Default;

        public DrawPage()
        {
            this.InitializeComponent();
            {
                // Extend TitleBar
                CoreApplicationView applicationView = CoreApplication.GetCurrentView();
                applicationView.TitleBar.ExtendViewIntoTitleBar = false;
            }
            base.SizeChanged += (s, e) =>
            {
                if (e.NewSize == Size.Empty) return;
                if (e.NewSize == e.PreviousSize) return;

                this._vsDeviceLayoutType = this.DeviceLayout.GetActualType(e.NewSize.Width);
                this.VisualState = this.VisualState; // VisualState
            };


            this.ConstructPreview();
            this.ConstructPosition();

            this.ConstructCanvas();
            this.ConstructScrollViewer();

            this.ConstructAdd();
            this.ConstructMenu();
            this.ConstructEdit();


            this.BackButton.Click += (s, e) => this.AppBarListView.SelectedIndex = -1;
            this.FullScreenButton.Click += (s, e) => this.IsFullScreen = true;
            this.UnFullScreenButton.Click += (s, e) => this.IsFullScreen = false;
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