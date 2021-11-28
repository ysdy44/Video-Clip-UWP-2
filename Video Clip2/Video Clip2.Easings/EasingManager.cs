using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace Video_Clip2.Easings
{
    public static class EasingManager
    {

        public static double Ease(EasingMode mode, EasingType type, double normalizedTime) => EasingManager.Ease(EasingManager.Dictionary[mode][type], normalizedTime);
        public static double Ease(EasingFunctionBase easingBase, double normalizedTime) => easingBase is null ? normalizedTime : easingBase.Ease(normalizedTime);

        public static PointCollection PointsConverter(EasingMode mode, EasingType type) => EasingManager.PointsConverter(EasingManager.Dictionary[mode][type], 26, 26);
        public static PointCollection PointsConverter(EasingMode mode, EasingType type, double width, double height) => EasingManager.PointsConverter(EasingManager.Dictionary[mode][type], width, height);
        public static PointCollection PointsConverter(EasingFunctionBase easingBase, double width, double height)
        {
            PointCollection points = new PointCollection();
            {
                points.Add(new Point(0, 0));

                if (!(easingBase is null))
                {
                    for (int x = 0; x < width; x++)
                    {
                        double y = easingBase.Ease(x / width) * height;
                        points.Add(new Point(x, (int)y));
                    }
                }

                points.Add(new Point(width, height));
            }
            return points;
        }
        

        internal readonly static Dictionary<EasingMode, Dictionary<EasingType, EasingFunctionBase>> Dictionary = new Dictionary<EasingMode, Dictionary<EasingType, EasingFunctionBase>>
        {
            [EasingMode.EaseIn] = new Dictionary<EasingType, EasingFunctionBase>
            {
                [EasingType.None] = null,

                [EasingType.Sine] = new SineEase { EasingMode = EasingMode.EaseIn },

                [EasingType.Quadratic] = new QuadraticEase { EasingMode = EasingMode.EaseIn },
                [EasingType.Cubic] = new CubicEase { EasingMode = EasingMode.EaseIn },
                [EasingType.Quartic] = new QuarticEase { EasingMode = EasingMode.EaseIn },

                [EasingType.Power] = new PowerEase { EasingMode = EasingMode.EaseIn },
                [EasingType.Exponential] = new ExponentialEase { EasingMode = EasingMode.EaseIn },
                [EasingType.Circle] = new CircleEase { EasingMode = EasingMode.EaseIn },

                [EasingType.Back] = new BackEase { EasingMode = EasingMode.EaseIn },
                [EasingType.Elastic] = new ElasticEase { EasingMode = EasingMode.EaseIn },
                [EasingType.Bounce] = new BounceEase { EasingMode = EasingMode.EaseIn },
            },

            [EasingMode.EaseOut] = new Dictionary<EasingType, EasingFunctionBase>
            {
                [EasingType.None] = null,

                [EasingType.Sine] = new SineEase { EasingMode = EasingMode.EaseOut },

                [EasingType.Quadratic] = new QuadraticEase { EasingMode = EasingMode.EaseOut },
                [EasingType.Cubic] = new CubicEase { EasingMode = EasingMode.EaseOut },
                [EasingType.Quartic] = new QuarticEase { EasingMode = EasingMode.EaseOut },

                [EasingType.Power] = new PowerEase { EasingMode = EasingMode.EaseOut },
                [EasingType.Exponential] = new ExponentialEase { EasingMode = EasingMode.EaseOut },
                [EasingType.Circle] = new CircleEase { EasingMode = EasingMode.EaseOut },

                [EasingType.Back] = new BackEase { EasingMode = EasingMode.EaseOut },
                [EasingType.Elastic] = new ElasticEase { EasingMode = EasingMode.EaseOut },
                [EasingType.Bounce] = new BounceEase { EasingMode = EasingMode.EaseOut },
            },

            [EasingMode.EaseInOut] = new Dictionary<EasingType, EasingFunctionBase>
            {
                [EasingType.None] = null,

                [EasingType.Sine] = new SineEase { EasingMode = EasingMode.EaseInOut },

                [EasingType.Quadratic] = new QuadraticEase { EasingMode = EasingMode.EaseInOut },
                [EasingType.Cubic] = new CubicEase { EasingMode = EasingMode.EaseInOut },
                [EasingType.Quartic] = new QuarticEase { EasingMode = EasingMode.EaseInOut },

                [EasingType.Power] = new PowerEase { EasingMode = EasingMode.EaseInOut },
                [EasingType.Exponential] = new ExponentialEase { EasingMode = EasingMode.EaseInOut },
                [EasingType.Circle] = new CircleEase { EasingMode = EasingMode.EaseInOut },

                [EasingType.Back] = new BackEase { EasingMode = EasingMode.EaseInOut },
                [EasingType.Elastic] = new ElasticEase { EasingMode = EasingMode.EaseInOut },
                [EasingType.Bounce] = new BounceEase { EasingMode = EasingMode.EaseInOut },
            },
        };

    }
}