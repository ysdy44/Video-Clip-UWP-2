using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Numerics;
using Video_Clip2.Clips.ClipTracks;
using Video_Clip2.Effects;
using Windows.Graphics.Imaging;
using Windows.UI.Xaml;

namespace Video_Clip2.Clips
{
    public interface IClip : IDisposable, IGetActualTransformer
    {

        IClipTrack Track { get; }

        #region Instance

        string Id { get; set; }

        #endregion

        #region Property

        ClipType Type { get; }
        bool IsOverlayLayer { get; }
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
        ICanvasImage GetRender(bool isPlaying, TimeSpan position, Matrix3x2 matrix);

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

        IClip TrimClone(Clipping clipping, bool isMuted, BitmapSize size, TimeSpan position, double trackHeight, double trackScale);

    }
}