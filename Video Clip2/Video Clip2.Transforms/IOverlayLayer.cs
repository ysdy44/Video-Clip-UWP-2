namespace Video_Clip2.Transforms
{
    public interface IOverlayLayer : ITransform, IRenderTransform
    {
        bool IOverlayLayerCore { get; set; }
    }
}