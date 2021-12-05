using Video_Clip2.Clips;
using Video_Clip2.Clips.Draggers;
using Video_Clip2.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

namespace Video_Clip2.Tools.Models
{
    public class CursorTool : ITool
    {

        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;
        bool IsCtrl => Window.Current.CoreWindow.GetKeyState(Windows.System.VirtualKey.Control) == Windows.UI.Core.CoreVirtualKeyStates.Down;
        bool IsExtended => this.IsCtrl;


        double StartingScale;


        public ManipulationModes TrackManipulationMode => ManipulationModes.System | ManipulationModes.Scale;

        public void TrackManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e) => this.StartingScale = this.ViewModel.TrackScale;
        public void TrackManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e) => this.ViewModel.TrackScale = e.Cumulative.Scale * this.StartingScale;
        public void TrackManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e) { }


        public void ItemClick(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement element)
            {
                if (element.DataContext is IClip clip)
                {
                    if (this.IsExtended)
                    {
                        clip.IsSelected = !clip.IsSelected;
                        this.SelectionViewModel.SetMode(); // Selection
                    }
                    else
                    {
                        foreach (IClip item in this.ViewModel.ObservableCollection)
                        {
                            if (item.IsSelected) item.IsSelected = false;
                        }
                        clip.IsSelected = true;

                        this.SelectionViewModel.SetModeSingle(clip); // Selection
                    }
                }
            }
        }

        public void ItemManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            if (sender is FrameworkElement element)
            {
                if (element.DataContext is IClip clip)
                {
                    foreach (IClip item in this.ViewModel.ObservableCollection)
                    {
                        if (item.IsSelected) item.IsSelected = false;
                    }

                    clip.CacheIndex();
                    clip.CacheDelay(this.ViewModel.TrackScale);

                    this.SelectionViewModel.SetModeSingle(clip); // Selection
                    this.SelectionViewModel.CacheTrimmer(); // Selection
                    e.Handled = true;
                }
            }
        }
        public void ItemManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            if (sender is FrameworkElement element)
            {
                if (element.DataContext is IClip clip)
                {
                    int moveY = this.ViewModel.StartingTrimmer.Move(this.ViewModel.TrackHeight, e.Cumulative.Translation.Y);
                    double moveX = this.ViewModel.StartingTrimmer.Move(e.Cumulative.Translation.X);

                    clip.AddIndex(this.ViewModel.TrackHeight, moveY);
                    clip.AddDelay(this.ViewModel.TrackScale, moveX, this.ViewModel.Position);

                    this.SelectionViewModel.SetModeSingle(clip); // Selection
                    e.Handled = true;
                }
            }
        }
        public void ItemManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            if (sender is FrameworkElement element)
            {
                if (element.DataContext is IClip clip)
                {
                    clip.IsSelected = true;

                    this.SelectionViewModel.SetModeSingle(clip); // Selection
                    e.Handled = true;
                }
            }
        }


        public void ThumbDraggerManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            this.ViewModel.IsPlayingCore = false;

            if (sender is FrameworkElement element)
            {
                if (element.DataContext is IClip clip)
                {
                    if (clip.IsSelected == false)
                    {
                        foreach (IClip item in this.ViewModel.ObservableCollection)
                        {
                            if (item.IsSelected) item.IsSelected = false;
                        }
                        clip.IsSelected = true;
                        clip.CacheIndex();
                        clip.CacheDelay(this.ViewModel.TrackScale);
                        e.Handled = false;
                    }
                }
            }

            foreach (IClip item in this.ViewModel.ObservableCollection)
            {
                if (item.IsSelected)
                {
                    item.CacheIndex();
                    item.CacheDelay(this.ViewModel.TrackScale);
                }
            }

            this.SelectionViewModel.SetMode(); // Selection
            this.SelectionViewModel.CacheTrimmer(); // Selection
            e.Handled = true;
        }
        public void ThumbDraggerManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            int moveY = this.ViewModel.StartingTrimmer.Move(this.ViewModel.TrackHeight, e.Cumulative.Translation.Y);
            double moveX = this.ViewModel.StartingTrimmer.Move(e.Cumulative.Translation.X);

            if (sender is FrameworkElement element)
            {
                if (element.DataContext is IClip clip)
                {
                    if (clip.IsSelected == false) return;
                }
            }

            foreach (IClip item in this.ViewModel.ObservableCollection)
            {
                if (item.IsSelected)
                {
                    item.AddIndex(this.ViewModel.TrackHeight, moveY);
                    item.AddDelay(this.ViewModel.TrackScale, moveX, this.ViewModel.Position);
                }
            }

            this.SelectionViewModel.SetMode(); // Selection
            e.Handled = true;
        }
        public void ThumbDraggerManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            this.ViewModel.IsPlayingCore = this.ViewModel.IsPlaying;
            if (sender is FrameworkElement element)
            {
                if (element.DataContext is IClip clip)
                {
                    if (clip.IsSelected == false) return;
                }
            }

            this.SelectionViewModel.SetMode(); // Selection
            e.Handled = true;
        }

        public void LeftDraggerManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            this.ViewModel.IsPlayingCore = false;

            foreach (IClip item in this.ViewModel.ObservableCollection)
            {
                if (item.IsSelected)
                {
                    item.CacheDelay(this.ViewModel.TrackScale);
                    item.CacheDuration(this.ViewModel.TrackScale);

                    this.SelectionViewModel.SetModeSingle(item); // Selection

                    if (sender is TrimDragger dragger)
                    {
                        dragger.UpdateManipulationStarted();
                    }

                    e.Handled = true;
                    return;
                }
            }
        }
        public void LeftDraggerManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            foreach (IClip item in this.ViewModel.ObservableCollection)
            {
                if (item.IsSelected)
                {
                    item.TrimStart(this.ViewModel.TrackScale, e.Cumulative.Translation.X, this.ViewModel.Position);

                    this.SelectionViewModel.SetModeSingle(item); // Selection
                    e.Handled = true;
                    return;
                }
            }
        }
        public void LeftDraggerManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            this.ViewModel.IsPlayingCore = this.ViewModel.IsPlaying;

            this.SelectionViewModel.SetMode(); // Selection

            if (sender is TrimDragger dragger)
            {
                dragger.UpdateManipulationCompleted();
            }

            e.Handled = true;
        }

        public void RightDraggerManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            this.ViewModel.IsPlayingCore = false;

            foreach (IClip item in this.ViewModel.ObservableCollection)
            {
                if (item.IsSelected)
                {
                    item.CacheDelay(this.ViewModel.TrackScale);
                    item.CacheDuration(this.ViewModel.TrackScale);

                    this.SelectionViewModel.SetModeSingle(item); // Selection

                    if (sender is TrimDragger dragger)
                    {
                        dragger.UpdateManipulationStarted();
                    }

                    e.Handled = true;
                    return;
                }
            }
        }
        public void RightDraggerManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            foreach (IClip item in this.ViewModel.ObservableCollection)
            {
                if (item.IsSelected)
                {
                    item.TrimEnd(this.ViewModel.TrackScale, e.Cumulative.Translation.X, this.ViewModel.Position);

                    this.SelectionViewModel.SetModeSingle(item); // Selection
                    e.Handled = true;
                    return;
                }
            }
        }
        public void RightDraggerManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            this.ViewModel.IsPlayingCore = this.ViewModel.IsPlaying;

            this.SelectionViewModel.SetMode(); // Selection

            if (sender is TrimDragger dragger)
            {
                dragger.UpdateManipulationCompleted();
            }

            e.Handled = true;
        }

    }
}