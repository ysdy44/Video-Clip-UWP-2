using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;

namespace Video_Clip2.Effects
{
    public class Effect
    {
        public bool GaussianBlur_IsOn;
        public float GaussianBlur_Radius = 0;
        public EffectBorderMode GaussianBlur_BorderMode = EffectBorderMode.Soft;

        public static ICanvasImage Render(Effect effect, ICanvasImage image, float scale)
        {
            // GaussianBlur
            if (effect.GaussianBlur_IsOn)
            {
                image = new GaussianBlurEffect
                {
                    Source = image,
                    BlurAmount = effect.GaussianBlur_Radius * scale,
                    BorderMode = effect.GaussianBlur_BorderMode,
                };
            }

            return image;
        }
    }
}
