using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Numerics;
using Video_Clip2.Clips.Clips;
using Video_Clip2.Clips.ClipTracks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml.Controls;

namespace Video_Clip2.Clips.Models
{
    public class TextClip : FrameClip, IClip
    {

        protected readonly ICanvasResourceCreatorWithDpi ResourceCreator;
        public CanvasCommandList CommandList { get; protected set; }
        public string Text { get; protected set; }

        public override ClipType Type => ClipType.Text;
        public override IClipTrack Track { get; } = new ClipTrack(Colors.Orange, Symbol.FontSize);

        protected TextClip(bool isMuted, TimeSpan delay, TimeSpan duration, int index, double trackHeight, double trackScale)
            : base(isMuted, delay, duration, index, trackHeight, trackScale)
        {
        }
        public TextClip(string text, bool isMuted, TimeSpan position, TimeSpan delay, TimeSpan duration, int index, double trackHeight, double trackScale, ICanvasResourceCreatorWithDpi resourceCreator, Size previewSize)
            : base(isMuted, delay, duration, index, trackHeight, trackScale)
        {
            this.ResourceCreator = resourceCreator;
            this.CommandList = new CanvasCommandList(resourceCreator);
            this.Text = text;
            if (this.Text != null) TextClip.Render(this.CommandList, this.Text);
            base.ChangeView(position, delay, duration);
        }

        public override void DrawThumbnail(CanvasControl sender, CanvasDrawEventArgs args)
        {
            args.DrawingSession.DrawText(this.Text, Vector2.One, Colors.White);
        }

        public override ICanvasImage GetRender(bool isPlaying, TimeSpan position, ICanvasResourceCreatorWithDpi resourceCreator, Size previewSize)
        {
            if (this.Text == null) return null;
            else if (base.InRange(position) == false) return null;
            else return this.CommandList;
        }

        public override void SetPreviewSize(Size previewSize)
        {
        }

        protected override IClip TrimClone(bool isMuted, TimeSpan position, TimeSpan nextDuration, double trackHeight, double trackScale, Size previewSize)
        {
            return new TextClip(this.Text, isMuted, position, position, nextDuration, base.Index, trackHeight, trackScale, this.ResourceCreator, previewSize);
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