using Microsoft.Graphics.Canvas;
using System;
using Video_Clip2.Clips;
using Video_Clip2.Clips.Models;
using Video_Clip2.Elements;
using Video_Clip2.ViewModels;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;

namespace Video_Clip2.Menus
{
    public sealed partial class DurationMenu : UserControl, ITransitionElement
    {

        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;

        TimeSpan Position;

        public DurationMenu()
        {
            this.InitializeComponent();
            this.DurationRanger.PositionChanged += (s, position) =>
            {
                this.Position = position;
                this.ViewModel.Invalidate();
            };
        }

        public void Draw(CanvasDrawingSession drawingSession, Size previewSize)
        {
            foreach (Clipping item in this.ViewModel.ObservableCollection)
            {
                IClip clip = item.Self;

                if (clip.IsSelected)
                {
                    switch (clip.Type)
                    {
                        case ClipType.Video:
                            if (clip is VideoClip videoClip)
                            {
                                ICanvasImage image = videoClip.GetPlayerRender(this.Position, previewSize);

                                drawingSession.DrawImage(image);
                                break;
                            }
                            break;
                    }
                }
            }
        }

        public void OnNavigatedFrom()
        {
            foreach (Clipping item in this.ViewModel.ObservableCollection)
            {
                IClip clip = item.Self;

                if (clip.IsSelected)
                {
                    switch (clip.Type)
                    {
                        case ClipType.Video:
                            if (clip is VideoClip videoClip)
                            {
                                this.DurationRanger.GetDuration(out TimeSpan trimTimeFromStart, out TimeSpan trimTimeFromEnd);
                                videoClip.SetDuration(this.ViewModel.TrackScale, trimTimeFromStart, trimTimeFromEnd);

                                this.SelectionViewModel.SetMode(); // Selection
                                this.ViewModel.Invalidate(); // Invalidate
                                break;
                            }
                            break;
                    }
                }
            }
        }

        public void OnNavigatedTo()
        {
            foreach (Clipping item in this.ViewModel.ObservableCollection)
            {
                IClip clip = item.Self;

                if (clip.IsSelected)
                {
                    switch (clip.Type)
                    {
                        case ClipType.Video:
                            if (clip is VideoClip videoClip)
                            {
                                this.DurationRanger.SetDuration(videoClip.PlaybackRate, videoClip.OriginalDuration, videoClip.TrimTimeFromStart, videoClip.TrimTimeFromEnd, TimeSpan.FromSeconds(2));

                                this.ViewModel.Invalidate(); // Invalidate
                                break;
                            }
                            break;
                    }
                }
            }
        }
    }
}
