using Windows.UI.Xaml;

namespace Video_Clip2.Clips.Draggers
{
    public sealed class LeftDragger : TrimDragger
    {
        public override CornerRadius Radius => new CornerRadius(4, 0, 0, 4);
        public override double GetValue(Trimmer trimmer) => trimmer.Left.Value - 22+2;
    }
}