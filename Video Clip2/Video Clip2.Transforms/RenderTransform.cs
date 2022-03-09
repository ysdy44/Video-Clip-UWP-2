using System;
using System.Numerics;
using Windows.Foundation;
using Windows.Graphics.Imaging;
using Windows.UI.Xaml.Media;

namespace Video_Clip2.Transforms
{
    public sealed class RenderTransform
    {
        public Stretch Stretch = Stretch.Uniform;
        public Rotate Rotate;
        public bool IsXFlip;
        public bool IsYFlip;
        public readonly uint Width;
        public readonly uint Height;
        public BitmapSize Size;

        public RenderTransform(uint width, uint height, BitmapSize size)
        {
            this.Width = width;
            this.Height = height;
            this.Size = size;
            this.ReloadMatrix();
        }

        public Matrix3x2 Matrix { get; private set; }
        public void ReloadMatrix() => this.Matrix = this.Render();

        private Matrix3x2 Render()
        {
            // Flip
            Matrix3x2 matrixFlip =
                Matrix3x2.CreateTranslation(-this.Width / 2, -this.Height / 2) *
                Matrix3x2.CreateScale(this.IsXFlip ? -1 : 1, this.IsYFlip ? -1 : 1);

            uint widthRotated;
            uint heightRotated;
            switch (this.Rotate)
            {
                case Rotate.None:
                case Rotate.Rotate180:
                    widthRotated = this.Width;
                    heightRotated = this.Height;
                    break;
                default:
                    widthRotated = this.Height;
                    heightRotated = this.Width;
                    break;
            }

            // Rotate
            Matrix3x2 matrixRotated;
            switch (this.Rotate)
            {
                case Rotate.None:
                    matrixRotated = matrixFlip;
                    break;
                case Rotate.RotateLeft90:
                    matrixRotated = matrixFlip * Matrix3x2.CreateRotation(-(float)Math.PI / 2);
                    break;
                case Rotate.RotateRight90:
                    matrixRotated = matrixFlip * Matrix3x2.CreateRotation((float)Math.PI / 2);
                    break;
                default:
                    matrixRotated = matrixFlip * Matrix3x2.CreateRotation((float)Math.PI);
                    break;
            }

            if (this.Stretch == Stretch.None) return matrixRotated * Matrix3x2.CreateTranslation(widthRotated / 2, heightRotated / 2);

            float previewWidth = (float)this.Size.Width;
            float previewHeight = (float)this.Size.Height;
            float scaleX = previewWidth / widthRotated;
            float scaleY = previewHeight / heightRotated;

            // Scale
            Vector2 scale;
            switch (this.Stretch)
            {
                case Stretch.Uniform:
                    scale = new Vector2(Math.Min(scaleX, scaleY));
                    break;
                case Stretch.UniformToFill:
                    scale = new Vector2(Math.Max(scaleX, scaleY));
                    break;
                default:
                    scale = new Vector2(scaleX, scaleY);
                    break;
            }

            return matrixRotated *
                Matrix3x2.CreateScale(scale) *
                Matrix3x2.CreateTranslation(new Vector2(previewWidth / 2, previewHeight / 2));
        }

        public static Matrix3x2 UniformRender(uint width, uint height, Size previewSize)
        {
            float previewWidth = (float)previewSize.Width;
            float previewHeight = (float)previewSize.Height;

            float scale = Math.Min(previewWidth / width, previewHeight / height);

            return
                Matrix3x2.CreateScale(scale) *
                Matrix3x2.CreateTranslation((previewWidth - width) / 2 * scale, (previewHeight - height) / 2 * scale);
        }

        public static bool operator !=(RenderTransform left, RenderTransform right) => !(left == right);
        public static bool operator ==(RenderTransform left, RenderTransform right)
        {
            if (left.Stretch != right.Stretch) return false;
            if (left.Rotate != right.Rotate) return false;
            if (left.IsXFlip != right.IsXFlip) return false;
            if (left.IsYFlip != right.IsYFlip) return false;
            if (left.Width != right.Width) return false;
            if (left.Height != right.Height) return false;
            if (left.Size.Width != right.Size.Width) return false;
            if (left.Size.Height != right.Size.Height) return false;
            return true;
        }

        public override bool Equals(object obj) => base.Equals(obj);
        public override int GetHashCode() => base.GetHashCode();
        public override string ToString() => base.ToString();
    }
}