using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using Video_Clip2.Clips.ClipTracks;
using Video_Clip2.Effects;
using Windows.Foundation;
using Windows.UI.Xaml;

namespace Video_Clip2.Clips
{
    public interface IClip : IDisposable
    {

        IClipTrack Track { get; }

        #region Instance

        string Id { get; }

        #endregion

        #region Property

        ClipType Type { get; }
        string Name { get; set; }

        float Opacity { get; set; }

        Visibility Visibility { get; set; }

        bool IsMuted { get; }
        void SetIsMuted(bool isMuted, bool isMutedCore);

        bool IsSelected { get; set; }

        Effect Effect { get; }

        #endregion

        #region Render

        bool InRange(TimeSpan position);
        bool InRange(TimeSpan position, TimeSpan minDuration);
        void DrawThumbnail(CanvasControl sender, CanvasDrawEventArgs args);
        ICanvasImage GetRender(bool isPlaying, TimeSpan position, Size previewSize);

        int Index { get; }
        void CacheIndex();
        void AddIndex(double trackHeight, int move);

        TimeSpan Delay { get; }
        void CacheDelay(double trackScale);
        void AddDelay(double trackScale, double offset, TimeSpan position);

        TimeSpan Duration { get; }
        void CacheDuration(double trackScale);
        void TrimStart(double trackScale, double offset, TimeSpan position);
        void TrimEnd(double trackScale, double offset, TimeSpan position);

        #endregion

        IClip TrimClone(bool isMuted, TimeSpan position, double trackHeight, double trackScale);

    }
}