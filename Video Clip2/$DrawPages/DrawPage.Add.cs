using Microsoft.Graphics.Canvas;
using System;
using System.Collections.Generic;
using System.Linq;
using Video_Clip2.Clips;
using Video_Clip2.Clips.Models;
using Video_Clip2.Medias.Models;
using Windows.Storage;
using Windows.Storage.Pickers;
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

                foreach (Clipping item in this.ViewModel.ObservableCollection)
                {
                    IClip clip2 = item.Self;

                    if (clip2.IsSelected) clip2.IsSelected = false;
                }

                IReadOnlyList<StorageFile> files = await FileUtil.PickMultipleImageFilesAsync(PickerLocationId.Desktop, ClipType.Video);
                if (files is null) return;
                if (files.Count() <= 0) return;

                this.IsLoading = true;
                foreach (StorageFile item in files)
                {
                    Video video = await Video.Instances.CreateAsync(ClipManager.CanvasDevice, item);

                    // Clip
                    Clipping clipping = Clipping.CreateByGuid();
                    IClip clip = new VideoClip(video, this.ViewModel.IsMuted, this.ViewModel.Position, this.ViewModel.Position, 0, this.ViewModel.TrackHeight, this.ViewModel.TrackScale)
                    {
                        Id = clipping.Id,
                        IsSelected = true
                    };
                    ClipBase.Instances.Add(clipping.Id, clip);

                    this.ViewModel.ObservableCollection.Add(clipping);
                }
                this.IsLoading = false;

                this.SelectionViewModel.SetMode(); // Selection
                this.ViewModel.Invalidate(); // Invalidate
            };


            this.AddAudioItem.Click += async (s, e) =>
            {
                this.ViewModel.IsPlaying = false;

                foreach (Clipping item in this.ViewModel.ObservableCollection)
                {
                    IClip clip2 = item.Self;

                    if (clip2.IsSelected) clip2.IsSelected = false;
                }

                IReadOnlyList<StorageFile> files = await FileUtil.PickMultipleImageFilesAsync(PickerLocationId.Desktop, ClipType.Audio);
                if (files is null) return;
                if (files.Count() <= 0) return;

                this.IsLoading = true;
                foreach (StorageFile item in files)
                {
                    Audio audio = await Audio.Instances.CreateAsync(ClipManager.CanvasDevice, item);

                    // Clip
                    Clipping clipping = Clipping.CreateByGuid();
                    IClip clip = new AudioClip(audio, this.ViewModel.IsMuted, this.ViewModel.Position, 0, this.ViewModel.TrackHeight, this.ViewModel.TrackScale)
                    {
                        Id = clipping.Id,
                        IsSelected = true
                    };
                    ClipBase.Instances.Add(clipping.Id, clip);

                    this.ViewModel.ObservableCollection.Add(clipping);
                }
                this.IsLoading = false;

                this.SelectionViewModel.SetMode(); // Selection
                this.ViewModel.Invalidate(); // Invalidate
            };


            this.AddImageItem.Click += async (s, e) =>
            {
                this.ViewModel.IsPlaying = false;

                foreach (Clipping item in this.ViewModel.ObservableCollection)
                {
                    IClip clip2 = item.Self;

                    if (clip2.IsSelected) clip2.IsSelected = false;
                }

                IReadOnlyList<StorageFile> files = await FileUtil.PickMultipleImageFilesAsync(PickerLocationId.Desktop, ClipType.Image);
                if (files is null) return;
                if (files.Count() <= 0) return;

                this.IsLoading = true;
                foreach (StorageFile item in files)
                {
                    Photo photo = await Photo.Instances.CreateAsync(ClipManager.CanvasDevice, item);
                    TimeSpan duration = TimeSpan.FromSeconds(10);

                    // Clip
                    Clipping clipping = Clipping.CreateByGuid();
                    IClip clip = new ImageClip(photo, this.ViewModel.IsMuted, this.ViewModel.Position, this.ViewModel.Position, duration, 0, this.ViewModel.TrackHeight, this.ViewModel.TrackScale)
                    {
                        Id = clipping.Id,
                        IsSelected = true
                    };
                    ClipBase.Instances.Add(clipping.Id, clip);

                    this.ViewModel.ObservableCollection.Add(clipping);
                }
                this.IsLoading = false;

                this.SelectionViewModel.SetMode(); // Selection
                this.ViewModel.Invalidate(); // Invalidate
            };


            this.AddColorItem.Click += (s, e) =>
            {
                this.ViewModel.IsPlaying = false;

                foreach (Clipping item in this.ViewModel.ObservableCollection)
                {
                    IClip clip2 = item.Self;

                    if (clip2.IsSelected) clip2.IsSelected = false;
                }

                Color color = this.ColorPicker.Color;
                TimeSpan duration = TimeSpan.FromSeconds(10);

                // Clip
                Clipping clipping = Clipping.CreateByGuid();
                IClip clip = new ColorClip(color, this.ViewModel.IsMuted, this.ViewModel.Position, duration, 0, this.ViewModel.TrackHeight, this.ViewModel.TrackScale)
                {
                    Id = clipping.Id,
                    IsSelected = true
                };
                ClipBase.Instances.Add(clipping.Id, clip);

                this.ViewModel.ObservableCollection.Add(clipping);

                this.SelectionViewModel.SetModeSingle(clip); // Selection
                this.ViewModel.Invalidate(); // Invalidate
            };


            this.AddTextItem.Click += (s, e) =>
            {
                this.ViewModel.IsPlaying = false;

                foreach (Clipping item in this.ViewModel.ObservableCollection)
                {
                    IClip clip2 = item.Self;

                    if (clip2.IsSelected) clip2.IsSelected = false;
                }

                string text = "Click to change text";
                TimeSpan duration = TimeSpan.FromSeconds(10);

                // Clip
                Clipping clipping = Clipping.CreateByGuid();
                IClip clip = new TextClip(text, this.ViewModel.IsMuted, this.ViewModel.Position, this.ViewModel.Position, duration, 0, this.ViewModel.TrackHeight, this.ViewModel.TrackScale)
                {
                    Id = clipping.Id,
                    IsSelected = true
                };
                ClipBase.Instances.Add(clipping.Id, clip);

                this.ViewModel.ObservableCollection.Add(clipping);

                this.SelectionViewModel.SetModeSingle(clip); // Selection
                this.ViewModel.Invalidate(); // Invalidate
            };


            this.AddSubtitleItem.Click += (s, e) =>
            {
                this.ViewModel.IsPlaying = false;

                foreach (Clipping item in this.ViewModel.ObservableCollection)
                {
                    IClip clip2 = item.Self;

                    if (clip2.IsSelected) clip2.IsSelected = false;
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

                // Clip
                Clipping clipping = Clipping.CreateByGuid();
                IClip clip = new SubtitleClip(subtitles, this.ViewModel.IsMuted, this.ViewModel.Position, this.ViewModel.Position, duration, 0, this.ViewModel.TrackHeight, this.ViewModel.TrackScale)
                {
                    Id = clipping.Id,
                    IsSelected = true
                };
                ClipBase.Instances.Add(clipping.Id, clip);

                this.ViewModel.ObservableCollection.Add(clipping);

                this.SelectionViewModel.SetModeSingle(clip); // Selection
                this.ViewModel.Invalidate(); // Invalidate
            };
        }

    }
}