using Video_Clip2.Clips;
using Video_Clip2.Clips.Models;
using Video_Clip2.Transforms;
using Windows.UI.Xaml.Controls;

namespace Video_Clip2
{
    public sealed partial class DrawPage : Page
    {

        private void ConstructEdit()
        {
            this.TrimButton.Click += (s, e) => this.ViewModel.MethodEditTrim();


            this.SpeedButton.Click += (s, e) =>
            {
                foreach (Clipping item in this.ViewModel.ObservableCollection)
                {
                    IClip clip = item.Self;

                    if (clip.IsSelected)
                    {
                        switch (clip.Type)
                        {
                            case ClipType.Video:
                            case ClipType.Audio:
                                if (clip is MediaClip mediaClip)
                                {
                                    this.SpeedSlider.Value = mediaClip.PlaybackRate;
                                    break;
                                }
                                break;
                        }
                    }
                }
                this.SpeedSlider.Width = this.AppBarRightScrollViewer.ActualWidth;
                this.SpeedFlyout.ShowAt(this.AppBarRightScrollViewer);
            };
            this.SpeedSlider.ValueChangedStarted += (s, e) =>
            {
                this.ViewModel.IsPlayingCore = false;
                foreach (Clipping item in this.ViewModel.ObservableCollection)
                {
                    IClip clip = item.Self;

                    if (clip.IsSelected)
                    {
                        switch (clip.Type)
                        {
                            case ClipType.Video:
                            case ClipType.Audio:
                                clip.CacheDuration(this.ViewModel.TrackScale);
                                break;
                        }
                    }
                }
            };
            this.SpeedSlider.ValueChangedDelta += (s, e) =>
            {
                double speed = e.NewValue;
                foreach (Clipping item in this.ViewModel.ObservableCollection)
                {
                    IClip clip = item.Self;

                    if (clip.IsSelected)
                    {
                        switch (clip.Type)
                        {
                            case ClipType.Video:
                            case ClipType.Audio:
                                if (clip is MediaClip mediaClip)
                                {
                                    mediaClip.SetPlaybackRate(speed, this.ViewModel.TrackScale);
                                }
                                break;
                        }
                    }
                }
                this.SelectionViewModel.SetMode(); // Selection
                this.ViewModel.Invalidate(); // Invalidate
            };
            this.SpeedSlider.ValueChangedCompleted += (s, e) =>
            {
                this.ViewModel.IsPlayingCore = this.ViewModel.IsPlaying;
            };


            this.StretchButton.Click += (s, e) =>
            {
                foreach (Clipping item in this.ViewModel.ObservableCollection)
                {
                    IClip clip = item.Self;

                    if (clip.IsSelected)
                    {
                        switch (clip.Type)
                        {
                            case ClipType.Image:
                            case ClipType.Video:
                                if (clip is IOverlayLayer transformClip)
                                {
                                    this.StretchListView.IsOverlayLayer = transformClip.IOverlayLayerCore;
                                    this.StretchListView.Stretch = transformClip.RenderTransform.Stretch;
                                    break;
                                }
                                break;
                        }
                    }
                }
                this.StretchListView.Width = this.AppBarRightScrollViewer.ActualWidth;
                this.StretchFlyout.ShowAt(this.AppBarRightScrollViewer);
            };
            this.StretchListView.StretchChanged += (s, stretch) =>
            {
                foreach (Clipping item in this.ViewModel.ObservableCollection)
                {
                    IClip clip = item.Self;

                    if (clip.IsSelected)
                    {
                        switch (clip.Type)
                        {
                            case ClipType.Image:
                            case ClipType.Video:
                                if (clip is IRenderTransform transformClip)
                                {
                                    transformClip.RenderTransform.Stretch = stretch;
                                    transformClip.RenderTransform.ReloadMatrix();
                                }
                                break;
                        }
                    }
                }
                this.ViewModel.Invalidate(); // Invalidate
            };
            this.StretchListView.IsOverlayLayerChanged += (s, isOverlayLayer) =>
            {
                foreach (Clipping item in this.ViewModel.ObservableCollection)
                {
                    IClip clip = item.Self;

                    if (clip.IsSelected)
                    {
                        switch (clip.Type)
                        {
                            case ClipType.Image:
                            case ClipType.Video:
                                if (clip is IOverlayLayer transformClip)
                                {
                                    transformClip.IOverlayLayerCore = isOverlayLayer;
                                }
                                break;
                        }
                    }
                }
                this.ViewModel.Invalidate(); // Invalidate
            };


            this.VolumeButton.Click += (s, e) =>
            {
                foreach (Clipping item in this.ViewModel.ObservableCollection)
                {
                    IClip clip = item.Self;

                    if (clip.IsSelected)
                    {
                        switch (clip.Type)
                        {
                            case ClipType.Video:
                            case ClipType.Audio:
                                if (clip is MediaClip mediaClip)
                                {
                                    this.VolumeSlider.Value = mediaClip.Volume * 100;
                                    break;
                                }
                                break;
                        }
                    }
                }
                this.VolumeSlider.Width = this.AppBarRightScrollViewer.ActualWidth;
                this.VolumeFlyout.ShowAt(this.AppBarRightScrollViewer);
            };
            this.VolumeSlider.ValueChangedStarted += (s, e) =>
            {
                this.ViewModel.IsPlayingCore = false;
            };
            this.VolumeSlider.ValueChangedDelta += (s, e) =>
            {
                double volume = e.NewValue / 100;
                bool isMuted = e.NewValue == 0;
                foreach (Clipping item in this.ViewModel.ObservableCollection)
                {
                    IClip clip = item.Self;

                    if (clip.IsSelected)
                    {
                        switch (clip.Type)
                        {
                            case ClipType.Video:
                            case ClipType.Audio:
                                if (clip is MediaClip mediaClip)
                                {
                                    mediaClip.SetVolume(volume);
                                    mediaClip.SetIsMuted(this.ViewModel.IsMuted, isMuted);
                                }
                                break;
                        }
                    }
                }
                this.ViewModel.Invalidate(); // Invalidate
            };
            this.VolumeSlider.ValueChangedCompleted += (s, e) =>
            {
                this.ViewModel.IsPlayingCore = this.ViewModel.IsPlaying;
            };


            this.TransformButton.Click += (s, e) =>
            {
                this.TransformFlyout.ShowAt(this.AppBarRightScrollViewer);
            };
            this.FlipHorizontalButton.Click += (s, e) =>
            {
                foreach (Clipping item in this.ViewModel.ObservableCollection)
                {
                    IClip clip = item.Self;

                    if (clip.IsSelected)
                    {
                        switch (clip.Type)
                        {
                            case ClipType.Image:
                            case ClipType.Video:
                                if (clip is IRenderTransform transformClip)
                                {
                                    transformClip.RenderTransform.IsXFlip = !transformClip.RenderTransform.IsXFlip;
                                    transformClip.RenderTransform.ReloadMatrix();
                                }
                                break;
                        }
                    }
                }
                this.ViewModel.Invalidate(); // Invalidate
            };
            this.FlipVerticalButton.Click += (s, e) =>
            {
                foreach (Clipping item in this.ViewModel.ObservableCollection)
                {
                    IClip clip = item.Self;

                    if (clip.IsSelected)
                    {
                        switch (clip.Type)
                        {
                            case ClipType.Image:
                            case ClipType.Video:
                                if (clip is IRenderTransform transformClip)
                                {
                                    transformClip.RenderTransform.IsYFlip = !transformClip.RenderTransform.IsYFlip;
                                    transformClip.RenderTransform.ReloadMatrix();
                                }
                                break;
                        }
                    }
                }
                this.ViewModel.Invalidate(); // Invalidate
            };
            this.RotateLeftButton.Click += (s, e) =>
            {
                foreach (Clipping item in this.ViewModel.ObservableCollection)
                {
                    IClip clip = item.Self;

                    if (clip.IsSelected)
                    {
                        switch (clip.Type)
                        {
                            case ClipType.Image:
                            case ClipType.Video:
                                if (clip is IRenderTransform transformClip)
                                {
                                    switch (transformClip.RenderTransform.Rotate)
                                    {
                                        case Rotate.None:
                                            transformClip.RenderTransform.Rotate = Rotate.RotateLeft90;
                                            break;
                                        case Rotate.RotateLeft90:
                                            transformClip.RenderTransform.Rotate = Rotate.Rotate180;
                                            break;
                                        case Rotate.RotateRight90:
                                            transformClip.RenderTransform.Rotate = Rotate.None;
                                            break;
                                        case Rotate.Rotate180:
                                            transformClip.RenderTransform.Rotate = Rotate.RotateRight90;
                                            break;
                                    }
                                    transformClip.RenderTransform.ReloadMatrix();
                                }
                                break;
                        }
                    }
                }
                this.ViewModel.Invalidate(); // Invalidate
            };
            this.RotateRightButton.Click += (s, e) =>
            {
                foreach (Clipping item in this.ViewModel.ObservableCollection)
                {
                    IClip clip = item.Self;

                    if (clip.IsSelected)
                    {
                        switch (clip.Type)
                        {
                            case ClipType.Image:
                            case ClipType.Video:
                                if (clip is IRenderTransform transformClip)
                                {
                                    switch (transformClip.RenderTransform.Rotate)
                                    {
                                        case Rotate.None:
                                            transformClip.RenderTransform.Rotate = Rotate.RotateRight90;
                                            break;
                                        case Rotate.RotateLeft90:
                                            transformClip.RenderTransform.Rotate = Rotate.None;
                                            break;
                                        case Rotate.RotateRight90:
                                            transformClip.RenderTransform.Rotate = Rotate.Rotate180;
                                            break;
                                        case Rotate.Rotate180:
                                            transformClip.RenderTransform.Rotate = Rotate.RotateLeft90;
                                            break;
                                    }
                                    transformClip.RenderTransform.ReloadMatrix();
                                }
                                break;
                        }
                    }
                }
                this.ViewModel.Invalidate(); // Invalidate
            };



            this.OpacityButton.Click += (s, e) =>
            {
                foreach (Clipping item in this.ViewModel.ObservableCollection)
                {
                    IClip clip = item.Self;

                    if (clip.IsSelected)
                    {
                        this.OpacitySlider.Value = clip.Opacity * 100;
                        break;
                    }
                }
                this.OpacitySlider.Width = this.AppBarRightScrollViewer.ActualWidth;
                this.OpacityFlyout.ShowAt(this.AppBarRightScrollViewer);
            };
            this.OpacitySlider.ValueChangedStarted += (s, e) =>
            {
                this.ViewModel.IsPlayingCore = false;
            };
            this.OpacitySlider.ValueChangedDelta += (s, e) =>
            {
                float opacity = (float)(e.NewValue / 100);
                foreach (Clipping item in this.ViewModel.ObservableCollection)
                {
                    IClip clip = item.Self;

                    if (clip.IsSelected)
                    {
                        clip.Opacity = opacity;
                    }
                }
                this.ViewModel.Invalidate(); // Invalidate
            };
            this.OpacitySlider.ValueChangedCompleted += (s, e) =>
            {
                this.ViewModel.IsPlayingCore = this.ViewModel.IsPlaying;
            };


            this.ColorPicker.ColorChanged += (s, e) =>
            {
                foreach (Clipping item in this.ViewModel.ObservableCollection)
                {
                    IClip clip = item.Self;

                    if (clip.IsSelected)
                    {
                        if (clip is ColorClip colorClip)
                        {
                            colorClip.SetColor(e.NewColor);
                        }
                    }
                }
                this.ViewModel.Invalidate(); // Invalidate
            };


            this.EditCutItem.Click += (s, e) =>
            {
                this.ViewModel.IsPlaying = false;

            };


            this.EditCopyItem.Click += (s, e) =>
            {
                this.ViewModel.IsPlaying = false;

            };


            this.EditPasteItem.Click += (s, e) =>
            {
                this.ViewModel.IsPlaying = false;

            };


            this.EditClearItem.Click += (s, e) => this.ViewModel.MethodEditClear();
        }

    }
}