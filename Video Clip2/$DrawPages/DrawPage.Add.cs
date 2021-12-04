using Microsoft.Graphics.Canvas;
using System;
using System.Collections.Generic;
using System.Linq;
using Video_Clip2.Clips;
using Video_Clip2.Clips.Clips;
using Video_Clip2.Clips.Models;
using Video_Clip2.FileUtils;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Xaml.Controls;

namespace Video_Clip2
{
    public sealed partial class DrawPage : Page
    {

        private void ConstructAdd()
        {
            this.AddVideoItem.Click += async (s, e) =>
            {
                this.ViewModel.IsPlaying = false;

                foreach (IClip item in this.ViewModel.ObservableCollection)
                {
                    if (item.IsSelected) item.IsSelected = false;
                }

                IReadOnlyList<StorageFile> files = await FileUtil.PickMultipleImageFilesAsync(PickerLocationId.Desktop, ClipType.Video);
                if (files is null) return;
                if (files.Count() <= 0) return;

                this.IsLoading = true;
                foreach (StorageFile item in files)
                {
                    VideoProperties poperties = await item.Properties.GetVideoPropertiesAsync();
                    uint width = poperties.Width;
                    uint height = poperties.Height;
                    TimeSpan duration = poperties.Duration;
                    IList<CanvasBitmap> thumbnails = await VideoClip.LoadThumbnailsAsync(this.PreviewCanvas, item, duration, poperties.Width, poperties.Height);
                    this.ViewModel.ObservableCollection.Add(new VideoClip(item, width, height, thumbnails, this.ViewModel.IsMuted, this.ViewModel.Position, this.ViewModel.Position, duration, 0, this.ViewModel.TrackHeight, this.ViewModel.TrackScale, this.PreviewCanvas)
                    {
                        IsSelected = true
                    });
                }
                this.IsLoading = false;

                this.SelectionViewModel.SetMode(); // Selection
                this.ViewModel.Invalidate(); // Invalidate
            };


            this.AddAudioItem.Click += async (s, e) =>
            {
                this.ViewModel.IsPlaying = false;

                foreach (IClip item in this.ViewModel.ObservableCollection)
                {
                    if (item.IsSelected) item.IsSelected = false;
                }

                IReadOnlyList<StorageFile> files = await FileUtil.PickMultipleImageFilesAsync(PickerLocationId.Desktop, ClipType.Audio);
                if (files is null) return;
                if (files.Count() <= 0) return;

                this.IsLoading = true;
                foreach (StorageFile item in files)
                {
                    MusicProperties poperties = await item.Properties.GetMusicPropertiesAsync();
                    TimeSpan duration = poperties.Duration;
                    this.ViewModel.ObservableCollection.Add(new AudioClip(item, this.ViewModel.IsMuted, this.ViewModel.Position, duration, 0, this.ViewModel.TrackHeight, this.ViewModel.TrackScale)
                    {
                        IsSelected = true
                    });
                }
                this.IsLoading = false;

                this.SelectionViewModel.SetMode(); // Selection
                this.ViewModel.Invalidate(); // Invalidate
            };


            this.AddImageItem.Click += async (s, e) =>
            {
                this.ViewModel.IsPlaying = false;

                foreach (IClip item in this.ViewModel.ObservableCollection)
                {
                    if (item.IsSelected) item.IsSelected = false;
                }

                IReadOnlyList<StorageFile> files = await FileUtil.PickMultipleImageFilesAsync(PickerLocationId.Desktop, ClipType.Image);
                if (files is null) return;
                if (files.Count() <= 0) return;

                this.IsLoading = true;
                foreach (StorageFile item in files)
                {
                    using (IRandomAccessStream stream = await item.OpenReadAsync())
                    {
                        ICanvasResourceCreatorWithDpi resourceCreator = this.PreviewCanvas;
                        CanvasBitmap bitmap = await CanvasBitmap.LoadAsync(resourceCreator, stream);
                        TimeSpan duration = TimeSpan.FromSeconds(10);
                        this.ViewModel.ObservableCollection.Add(new ImageClip(bitmap, this.ViewModel.IsMuted, this.ViewModel.Position, this.ViewModel.Position, duration, 0, this.ViewModel.TrackHeight, this.ViewModel.TrackScale, this.PreviewCanvas)
                        {
                            IsSelected = true
                        });
                    }
                }
                this.IsLoading = false;

                this.SelectionViewModel.SetMode(); // Selection
                this.ViewModel.Invalidate(); // Invalidate
            };


            this.AddColorItem.Click += (s, e) =>
            {
                this.ViewModel.IsPlaying = false;

                foreach (IClip item in this.ViewModel.ObservableCollection)
                {
                    if (item.IsSelected) item.IsSelected = false;
                }

                Color color = this.ColorPicker.Color;
                TimeSpan duration = TimeSpan.FromSeconds(10);
                ColorClip clip = new ColorClip(color, this.ViewModel.IsMuted, this.ViewModel.Position, duration, 0, this.ViewModel.TrackHeight, this.ViewModel.TrackScale)
                {
                    IsSelected = true
                };
                this.ViewModel.ObservableCollection.Add(clip);

                this.SelectionViewModel.SetModeSingle(clip); // Selection
                this.ViewModel.Invalidate(); // Invalidate
            };


            this.AddTextItem.Click += (s, e) =>
            {
                this.ViewModel.IsPlaying = false;

                foreach (IClip item in this.ViewModel.ObservableCollection)
                {
                    if (item.IsSelected) item.IsSelected = false;
                }

                string text = "Click to change text";
                TimeSpan duration = TimeSpan.FromSeconds(10);
                TextClip clip = new TextClip(text, this.ViewModel.IsMuted, this.ViewModel.Position, this.ViewModel.Position, duration, 0, this.ViewModel.TrackHeight, this.ViewModel.TrackScale, this.PreviewCanvas)
                {
                    IsSelected = true
                };
                this.ViewModel.ObservableCollection.Add(clip);

                this.SelectionViewModel.SetModeSingle(clip); // Selection
                this.ViewModel.Invalidate(); // Invalidate
            };


            this.AddSubtitleItem.Click += (s, e) =>
            {
                this.ViewModel.IsPlaying = false;

                foreach (IClip item in this.ViewModel.ObservableCollection)
                {
                    if (item.IsSelected) item.IsSelected = false;
                }

                string text = "Click to change text";
                TimeSpan duration = TimeSpan.FromSeconds(10);
                IList<Subtitle> subtitles = new List<Subtitle>
                {
                    new Subtitle
                    {
                        Text =text,
                        Delay = TimeSpan.Zero,
                        Duration = duration
                    }
                };
                SubtitleClip clip = new SubtitleClip(subtitles, this.ViewModel.IsMuted, this.ViewModel.Position, this.ViewModel.Position, duration, 0, this.ViewModel.TrackHeight, this.ViewModel.TrackScale, this.PreviewCanvas)
                {
                    IsSelected = true
                };
                this.ViewModel.ObservableCollection.Add(clip);

                this.SelectionViewModel.SetModeSingle(clip); // Selection
                this.ViewModel.Invalidate(); // Invalidate
            };
        }

    }
}