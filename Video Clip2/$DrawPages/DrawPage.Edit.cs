using System;
using Video_Clip2.Clips;
using Video_Clip2.Clips.Models;
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
                foreach (IClip item in this.ViewModel.ObservableCollection)
                {
                    if (item.IsSelected)
                    {
                        switch (item.Type)
                        {
                            case ClipType.Video:
                            case ClipType.Audio:
                                if (item is MediaClip mediaClip)
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
                foreach (IClip item in this.ViewModel.ObservableCollection)
                {
                    if (item.IsSelected)
                    {
                        switch (item.Type)
                        {
                            case ClipType.Video:
                            case ClipType.Audio:
                                item.CacheDuration(this.ViewModel.TrackScale);
                                break;
                        }
                    }
                }
            };
            this.SpeedSlider.ValueChangedDelta += (s, e) =>
            {
                double speed = e.NewValue;
                foreach (IClip item in this.ViewModel.ObservableCollection)
                {
                    if (item.IsSelected)
                    {
                        switch (item.Type)
                        {
                            case ClipType.Video:
                            case ClipType.Audio:
                                if (item is MediaClip mediaClip)
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
                foreach (IClip item in this.ViewModel.ObservableCollection)
                {
                    if (item.IsSelected)
                    {
                        switch (item.Type)
                        {
                            case ClipType.Image:
                            case ClipType.Video:
                                if (item is IStretchClip stretchClip)
                                {
                                    this.StretchListView.Stretch = stretchClip.Stretch;
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
                foreach (IClip item in this.ViewModel.ObservableCollection)
                {
                    if (item.IsSelected)
                    {
                        switch (item.Type)
                        {
                            case ClipType.Image:
                            case ClipType.Video:
                                if (item is IStretchClip stretchClip)
                                {
                                    stretchClip.Stretch = stretch;
                                }
                                break;
                        }
                    }
                }
                this.ViewModel.Invalidate(); // Invalidate

                this.StretchFlyout.Hide();
            };


            this.VolumeButton.Click += (s, e) =>
            {
                foreach (IClip item in this.ViewModel.ObservableCollection)
                {
                    if (item.IsSelected)
                    {
                        switch (item.Type)
                        {
                            case ClipType.Video:
                            case ClipType.Audio:
                                if (item is MediaClip mediaClip)
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
                foreach (IClip item in this.ViewModel.ObservableCollection)
                {
                    if (item.IsSelected)
                    {
                        switch (item.Type)
                        {
                            case ClipType.Video:
                            case ClipType.Audio:
                                if (item is MediaClip mediaClip)
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


            this.OpacityButton.Click += (s, e) =>
            {
                foreach (IClip item in this.ViewModel.ObservableCollection)
                {
                    if (item.IsSelected)
                    {
                        this.OpacitySlider.Value = item.Opacity * 100;
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
                foreach (IClip item in this.ViewModel.ObservableCollection)
                {
                    if (item.IsSelected)
                    {
                        item.Opacity = opacity;
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
                foreach (IClip item in this.ViewModel.ObservableCollection)
                {
                    if (item.IsSelected)
                    {
                        if (item is ColorClip colorClip)
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