using Windows.Foundation;
using Windows.UI.Xaml.Controls;

namespace Video_Clip2.Clips
{
    public struct TrimmerValue
    {
        public double Value;
        public double MinValue;
        public double MaxValue;
        public double GetValue(double offset)
        {
            double value = this.Value + offset;
            if (value < this.MinValue) return this.MinValue;
            if (value > this.MaxValue) return this.MaxValue;
            return value;
        }
    }
    
    public struct Trimmer
    {

        public int Top;
        public int MinTop;
        public int MaxTop;

        public int Bottom;
        public int MinBottom;
        public int MaxBottom;

        public TrimmerValue Left;

        public TrimmerValue Right;

        public int Move(double trackHeight, double offsetY)
        {
            int move = (int)System.Math.Round(offsetY / trackHeight, System.MidpointRounding.ToEven);
            if (move < this.MinTop - this.Top) return this.MinTop - this.Top;
            if (move > this.MaxTop - this.Top) return this.MaxTop - this.Top;
            if (move < this.MinBottom - this.Bottom) return this.MinBottom - this.Bottom;
            if (move > this.MaxBottom - this.Bottom) return this.MaxBottom - this.Bottom;
            return move;
        }
        public double Move(double offsetX)
        {
            if (offsetX < this.Left.MinValue - this.Left.Value) return this.Left.MinValue - this.Left.Value;
            // if (offsetX > this.Left.MaxValue - this.Left.Value) return this.Left.MaxValue - this.Left.Value;
            // if (offsetX < this.Right.MinValue - this.Right.Value) return this.Right.MinValue - this.Right.Value;
            if (offsetX > this.Right.MaxValue - this.Right.Value) return this.Right.MaxValue - this.Right.Value;
            return offsetX;
        }

        public Rect ToRect(double trackHeight)
        {
            double top = trackHeight * this.Top;
            double bottom = trackHeight + trackHeight * this.Bottom;
            double left = this.Left.Value;
            double right = this.Right.Value;
            return new Rect(new Point(left, top), new Point(right, bottom));
        }
    }
}