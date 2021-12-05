using Video_Clip2.Easings;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace Video_Clip2.Menus
{
    public sealed partial class EasingMenu : UserControl
    {

        //@Converter
        private Visibility BooleanToVisibilityConverter(bool value) => value ? Visibility.Visible : Visibility.Collapsed;


        #region DependencyProperty


        public EasingMode Mode
        {
            get => (EasingMode)base.GetValue(ModeProperty);
            set => SetValue(ModeProperty, value);
        }
        /// <summary> Identifies the <see cref = "EasingMenu.Mode" /> dependency property. </summary>
        public static readonly DependencyProperty ModeProperty = DependencyProperty.Register(nameof(Mode), typeof(EasingMode), typeof(EasingMenu), new PropertyMetadata(EasingMode.EaseIn));


        public EasingType Type
        {
            get => (EasingType)base.GetValue(TypeProperty);
            set => SetValue(TypeProperty, value);
        }
        /// <summary> Identifies the <see cref = "EasingMenu.Type" /> dependency property. </summary>
        public static readonly DependencyProperty TypeProperty = DependencyProperty.Register(nameof(Type), typeof(EasingType), typeof(EasingMenu), new PropertyMetadata(EasingType.None));


        #endregion

        #region DependencyProperty


        public string EaseIn
        {
            get => (string)base.GetValue(EaseInProperty);
            set => SetValue(EaseInProperty, value);
        }
        /// <summary> Identifies the <see cref = "EasingMenu.EaseIn" /> dependency property. </summary>
        public static readonly DependencyProperty EaseInProperty = DependencyProperty.Register(nameof(EaseIn), typeof(string), typeof(EasingMenu), new PropertyMetadata(nameof(EasingMode.EaseIn)));


        public string EaseOut
        {
            get => (string)base.GetValue(EaseOutProperty);
            set => SetValue(EaseOutProperty, value);
        }
        /// <summary> Identifies the <see cref = "EasingMenu.EaseOut" /> dependency property. </summary>
        public static readonly DependencyProperty EaseOutProperty = DependencyProperty.Register(nameof(EaseOut), typeof(string), typeof(EasingMenu), new PropertyMetadata(nameof(EasingMode.EaseOut)));


        public string EaseInOut
        {
            get => (string)base.GetValue(EaseInOutProperty);
            set => SetValue(EaseInOutProperty, value);
        }
        /// <summary> Identifies the <see cref = "EasingMenu.EaseInOut" /> dependency property. </summary>
        public static readonly DependencyProperty EaseInOutProperty = DependencyProperty.Register(nameof(EaseInOut), typeof(string), typeof(EasingMenu), new PropertyMetadata(nameof(EasingMode.EaseInOut)));


        #endregion

        #region DependencyProperty


        public string None
        {
            get => (string)base.GetValue(NoneProperty);
            set => SetValue(NoneProperty, value);
        }
        /// <summary> Identifies the <see cref = "EasingMenu.None" /> dependency property. </summary>
        public static readonly DependencyProperty NoneProperty = DependencyProperty.Register(nameof(None), typeof(string), typeof(EasingMenu), new PropertyMetadata(nameof(EasingType.None)));

        public string Sine
        {
            get => (string)base.GetValue(SineProperty);
            set => SetValue(SineProperty, value);
        }
        /// <summary> Identifies the <see cref = "EasingMenu.Sine" /> dependency property. </summary>
        public static readonly DependencyProperty SineProperty = DependencyProperty.Register(nameof(Sine), typeof(string), typeof(EasingMenu), new PropertyMetadata(nameof(EasingType.Sine)));



        public string Quadratic
        {
            get => (string)base.GetValue(QuadraticProperty);
            set => SetValue(QuadraticProperty, value);
        }
        /// <summary> Identifies the <see cref = "EasingMenu.Quadratic" /> dependency property. </summary>
        public static readonly DependencyProperty QuadraticProperty = DependencyProperty.Register(nameof(Quadratic), typeof(string), typeof(EasingMenu), new PropertyMetadata(nameof(EasingType.Quadratic)));

        public string Cubic
        {
            get => (string)base.GetValue(CubicProperty);
            set => SetValue(CubicProperty, value);
        }
        /// <summary> Identifies the <see cref = "EasingMenu.Cubic" /> dependency property. </summary>
        public static readonly DependencyProperty CubicProperty = DependencyProperty.Register(nameof(Cubic), typeof(string), typeof(EasingMenu), new PropertyMetadata(nameof(EasingType.Cubic)));

        public string Quartic
        {
            get => (string)base.GetValue(QuarticProperty);
            set => SetValue(QuarticProperty, value);
        }
        /// <summary> Identifies the <see cref = "EasingMenu.Quartic" /> dependency property. </summary>
        public static readonly DependencyProperty QuarticProperty = DependencyProperty.Register(nameof(Quartic), typeof(string), typeof(EasingMenu), new PropertyMetadata(nameof(EasingType.Quartic)));



        public string Power
        {
            get => (string)base.GetValue(PowerProperty);
            set => SetValue(PowerProperty, value);
        }
        /// <summary> Identifies the <see cref = "EasingMenu.Power" /> dependency property. </summary>
        public static readonly DependencyProperty PowerProperty = DependencyProperty.Register(nameof(Power), typeof(string), typeof(EasingMenu), new PropertyMetadata(nameof(EasingType.Power)));

        public string Exponential
        {
            get => (string)base.GetValue(ExponentialProperty);
            set => SetValue(ExponentialProperty, value);
        }
        /// <summary> Identifies the <see cref = "EasingMenu.Exponential" /> dependency property. </summary>
        public static readonly DependencyProperty ExponentialProperty = DependencyProperty.Register(nameof(Exponential), typeof(string), typeof(EasingMenu), new PropertyMetadata(nameof(EasingType.Exponential)));

        public string Circle
        {
            get => (string)base.GetValue(CircleProperty);
            set => SetValue(CircleProperty, value);
        }
        /// <summary> Identifies the <see cref = "EasingMenu.Circle" /> dependency property. </summary>
        public static readonly DependencyProperty CircleProperty = DependencyProperty.Register(nameof(Circle), typeof(string), typeof(EasingMenu), new PropertyMetadata(nameof(EasingType.Circle)));



        public string Back
        {
            get => (string)base.GetValue(BackProperty);
            set => SetValue(BackProperty, value);
        }
        /// <summary> Identifies the <see cref = "EasingMenu.Back" /> dependency property. </summary>
        public static readonly DependencyProperty BackProperty = DependencyProperty.Register(nameof(Back), typeof(string), typeof(EasingMenu), new PropertyMetadata(nameof(EasingType.Back)));

        public string Elastic
        {
            get => (string)base.GetValue(ElasticProperty);
            set => SetValue(ElasticProperty, value);
        }
        /// <summary> Identifies the <see cref = "EasingMenu.Elastic" /> dependency property. </summary>
        public static readonly DependencyProperty ElasticProperty = DependencyProperty.Register(nameof(Elastic), typeof(string), typeof(EasingMenu), new PropertyMetadata(nameof(EasingType.Elastic)));

        public string Bounce
        {
            get => (string)base.GetValue(BounceProperty);
            set => SetValue(BounceProperty, value);
        }
        /// <summary> Identifies the <see cref = "EasingMenu.Bounce" /> dependency property. </summary>
        public static readonly DependencyProperty BounceProperty = DependencyProperty.Register(nameof(Bounce), typeof(string), typeof(EasingMenu), new PropertyMetadata(nameof(EasingType.Bounce)));


        #endregion


        public EasingMenu()
        {
            this.InitializeComponent();
            this.InGridView.ItemClick += (s, e) => this.ItemClick(EasingMode.EaseIn, e.ClickedItem);
            this.OutGridView.ItemClick += (s, e) => this.ItemClick(EasingMode.EaseOut, e.ClickedItem);
            this.InOutGridView.ItemClick += (s, e) => this.ItemClick(EasingMode.EaseInOut, e.ClickedItem);
        }

        private void ItemClick(EasingMode mode, object clickedItem)
        {
            if (clickedItem is FrameworkElement item)
            {
                if (item.Tag is int index)
                {
                    EasingType type = (EasingType)index;
                    {
                        this.Mode = mode;
                        this.Type = type;
                    }
                }
            }
        }

    }
}