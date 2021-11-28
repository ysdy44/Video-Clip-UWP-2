using Windows.UI.Xaml.Media.Animation;

namespace Video_Clip2.Easings
{
    /// <summary>
    /// https://docs.microsoft.com/zh-cn/dotnet/desktop/wpf/graphics-multimedia/easing-functions?view=netframeworkdesktop-4.8
    /// </summary>
    public enum EasingType
    {
        None,

        Sine, // SineEase：使用正弦公式创建加速和/或减速的动画。

        Quadratic, // QuadraticEase：使用公式 f(t) = t2创建加速和/或减速的动画。
        Cubic, // CubicEase：使用公式 f(t) = t3创建加速和/或减速的动画。
        Quartic, // QuarticEase：使用公式 f(t) = t4创建加速和/或减速的动画。

        Power, // PowerEase：使用公式 f(t) = tp 创建加速和/或减速的动画，其中 p 等于 Power 属性。
        Exponential, // ExponentialEase：使用指数公式创建加速和/或减速的动画。
        Circle, // CircleEase：使用循环函数创建加速和/或减速的动画。

        Back, // BackEase：先退动画运动，然后再开始在指示的路径中进行动画处理。
        Elastic, // ElasticEase：创建类似于弹簧前后振荡的动画，直到其进入 rest 状态。
        Bounce, // BounceEase：创建弹跳效果。
    };
}