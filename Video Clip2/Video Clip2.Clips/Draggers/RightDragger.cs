using Windows.UI.Xaml;

namespace Video_Clip2.Clips.Draggers
{
    public sealed class RightDragger : TrimDragger
    {
        public override CornerRadius Radius => new CornerRadius(0, 4, 4, 0);
        public override double GetValue(Trimmer trimmer) => trimmer.Right.Value-2;
    }
}