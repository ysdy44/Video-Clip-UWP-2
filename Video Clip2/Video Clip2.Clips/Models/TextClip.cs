using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Numerics;
using System.Xml.Linq;
using Video_Clip2.Clips.ClipTracks;
using Video_Clip2.Transforms;
using Windows.Graphics.Imaging;
using Windows.UI;
using Windows.UI.Xaml.Controls;

namespace Video_Clip2.Clips.Models
{
    public abstract class TextClipBase : FrameClip
    {

        public CanvasCommandList CommandList { get; protected set; }
        public string Text { get; set; }
        public Transformer Transformer { get; private set; }

        public Transformer GetActualTransformer() => this.Transformer;

        protected void InitializeTextClipBase(BitmapSize size, TimeSpan position, TimeSpan delay, TimeSpan duration)
        {
            uint width = size.Width;
            uint height = size.Height; 
            this.Transformer = new Transformer(width, height, Vector2.Zero);
            this.CommandList = new CanvasCommandList(ClipManager.CanvasDevice);
            if (this.Text != null) TextClip.Render(this.CommandList, this.Text);
            base.ChangeView(position, delay, duration);
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

    public class TextClip : TextClipBase, IClip
    {

        public override ClipType Type => ClipType.Text;
        public override bool IsOverlayLayer => false;
        public override IClipTrack Track { get; } = new ClipTrack(Colors.Orange, Symbol.FontSize);

        public void Initialize(bool isMuted, BitmapSize size, TimeSpan position, TimeSpan delay, TimeSpan duration, int index, double trackHeight, double trackScale)
        {
            base.InitializeClipBase(isMuted, delay, index, trackHeight, trackScale);
            base.InitializeFrameClip(duration, trackScale);
            this.InitializeTextClipBase(size, position, delay, duration);
        }

        public override void DrawThumbnail(CanvasControl sender, CanvasDrawEventArgs args)
        {
            args.DrawingSession.DrawText(this.Text, Vector2.One, Colors.White);
        }

        public override ICanvasImage GetRender(bool isPlaying, TimeSpan position, Matrix3x2 matrix)
        {
            if (this.Text == null) return null;
            else if (base.InRange(position) == false) return null;
            else return this.CommandList;
        }

        protected override IClip TrimClone(Clipping clipping, bool isMuted, BitmapSize size, TimeSpan position, TimeSpan nextDuration, double trackHeight, double trackScale)
        {
            // Clip
            TextClip textClip = new TextClip
            {
                Id = clipping.Id,
                IsSelected = true,

                Text = this.Text
            };

            textClip.InitializeClipBase(isMuted, position, base.Index, trackHeight, trackScale);
            textClip.InitializeFrameClip(nextDuration, trackScale);
            textClip.InitializeTextClipBase(size, position, position, nextDuration);
            return textClip;
        }

    }
}