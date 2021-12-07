using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Numerics;
using Video_Clip2.Clips.ClipManagers;
using Video_Clip2.Clips.ClipTracks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml.Controls;

namespace Video_Clip2.Clips.Models
{
    public class TextClip : FrameClip, IClip
    {

        public CanvasCommandList CommandList { get; protected set; }
        public string Text { get; protected set; }

        public override ClipType Type => ClipType.Text;
        public override IClipTrack Track { get; } = new ClipTrack(Colors.Orange, Symbol.FontSize);

        protected TextClip(bool isMuted, TimeSpan delay, TimeSpan duration, int index, double trackHeight, double trackScale)
            : base(isMuted, delay, duration, index, trackHeight, trackScale)
        {
        }
        public TextClip(string text, bool isMuted, TimeSpan position, TimeSpan delay, TimeSpan duration, int index, double trackHeight, double trackScale)
            : base(isMuted, delay, duration, index, trackHeight, trackScale)
        {
            this.CommandList = new CanvasCommandList(ClipManager.CanvasDevice);
            this.Text = text;
            if (this.Text != null) TextClip.Render(this.CommandList, this.Text);
            base.ChangeView(position, delay, duration);
        }

        public override void DrawThumbnail(CanvasControl sender, CanvasDrawEventArgs args)
        {
            args.DrawingSession.DrawText(this.Text, Vector2.One, Colors.White);
        }

        public override ICanvasImage GetRender(bool isPlaying, TimeSpan position, Size previewSize)
        {
            if (this.Text == null) return null;
            else if (base.InRange(position) == false) return null;
            else return this.CommandList;
        }

        protected override IClip TrimClone(bool isMuted, TimeSpan position, TimeSpan nextDuration, double trackHeight, double trackScale)
        {
            return new TextClip(this.Text, isMuted, position, position, nextDuration, base.Index, trackHeight, trackScale);
        }

        public void Dispose()
        {
            this.CommandList.Dispose();
        }

        //@Static
        internal static void Render(CanvasCommandList commandList, string text)
        {
            using (CanvasDrawingSession drawingSession = commandList.CreateDrawingSession())
            {
                drawingSession.Clear(Colors.Transparent);
                drawingSession.DrawText(text, Vector2.Zero, Colors.Red);
            }
        }

    }
}