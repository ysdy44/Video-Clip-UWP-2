using System;
using System.Collections.Generic;
using System.Linq;
using Video_Clip2.Clips;
using Video_Clip2.Clips.Models;
using Video_Clip2.Medias;
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
                    Medium medium = video.ToMedium();

                    // Clip
                    Clipping clipping = Clipping.CreateByGuid();
                    VideoClip videoClip = new VideoClip
                    {
                        Id = clipping.Id,
                        IsSelected = true,

                        Medium = medium
                    };

                    videoClip.Initialize(1, this.ViewModel.IsMuted, this.ViewModel.Position, this.ViewModel.Position, 0, this.ViewModel.TrackHeight, this.ViewModel.TrackScale);
                    ClipBase.Instances.Add(clipping.Id, videoClip);

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
                    Medium medium = audio.ToMedium();

                    // Clip
                    Clipping clipping = Clipping.CreateByGuid();
                    AudioClip audioClip = new AudioClip
                    {
                        Id = clipping.Id,
                        IsSelected = true,

                        Medium = medium
                    };

                    audioClip.Initialize(1, this.ViewModel.IsMuted, this.ViewModel.Position, this.ViewModel.Position, 0, this.ViewModel.TrackHeight, this.ViewModel.TrackScale);
                    ClipBase.Instances.Add(clipping.Id, audioClip);

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
                    Medium medium = photo.ToMedium();
                    TimeSpan duration = TimeSpan.FromSeconds(10);

                    // Clip
                    Clipping clipping = Clipping.CreateByGuid();
                    ImageClip imageClip = new ImageClip
                    {
                        Id = clipping.Id,
                        IsSelected = true,

                        Medium = medium
                    };

                    imageClip.Initialize(this.ViewModel.IsMuted, this.ViewModel.Position, this.ViewModel.Position, duration, 0, this.ViewModel.TrackHeight, this.ViewModel.TrackScale);
                    ClipBase.Instances.Add(clipping.Id, imageClip);

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
                ColorClip clip = new ColorClip
                {
                    Id = clipping.Id,
                    IsSelected = true,

                    Color = color
                };

                clip.Initialize(this.ViewModel.IsMuted, this.ViewModel.Position, duration, 0, this.ViewModel.TrackHeight, this.ViewModel.TrackScale);
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

                TimeSpan duration = TimeSpan.FromSeconds(10);

                // Clip
                Clipping clipping = Clipping.CreateByGuid();
                TextClip textClip = new TextClip
                {
                    Id = clipping.Id,
                    IsSelected = true,

                    Text = "Click to change text"
                };

                textClip.Initialize(this.ViewModel.IsMuted, this.ViewModel.Position, this.ViewModel.Position, duration, 0, this.ViewModel.TrackHeight, this.ViewModel.TrackScale);
                ClipBase.Instances.Add(clipping.Id, textClip);

                this.ViewModel.ObservableCollection.Add(clipping);

                this.SelectionViewModel.SetModeSingle(textClip); // Selection
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

                TimeSpan duration = TimeSpan.FromSeconds(10);

                // Clip
                Clipping clipping = Clipping.CreateByGuid();
                SubtitleClip subtitleClip = new SubtitleClip
                {
                    Id = clipping.Id,
                    IsSelected = true,

                    Subtitles = new List<Subtitle>
                    {
                        new Subtitle
                        {
                            Text = "Click to change text",
                            Delay = TimeSpan.Zero,
                            Duration = duration
                        }
                    }
                };

                subtitleClip.Initialize(this.ViewModel.IsMuted, this.ViewModel.Position, this.ViewModel.Position, duration, 0, this.ViewModel.TrackHeight, this.ViewModel.TrackScale);
                ClipBase.Instances.Add(clipping.Id, subtitleClip);

                this.ViewModel.ObservableCollection.Add(clipping);

                this.SelectionViewModel.SetModeSingle(subtitleClip); // Selection
                this.ViewModel.Invalidate(); // Invalidate
            };
        }

    }
}