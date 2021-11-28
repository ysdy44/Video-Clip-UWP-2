using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Video_Clip2.Elements
{
    public sealed class TimerPlayRing : PlayRing
    {
        readonly DispatcherTimer Timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(2)
        };
        public TimerPlayRing()
            : base()
        {
            this.Timer.Tick += (s, e) =>
            {
                if (ToolTipService.GetToolTip(this) is ToolTip toolTip)
                {
                    toolTip.IsOpen = !toolTip.IsOpen;
                }
                base.Ding();
            };
        }
        public void Start()
        {
            if (ToolTipService.GetToolTip(this) is ToolTip toolTip)
            {
                toolTip.IsOpen = true;
            }
            this.Timer.Start();
        }
        public void Stop()
        {
            if (ToolTipService.GetToolTip(this) is ToolTip toolTip)
            {
                toolTip.IsOpen = false;
            }
            this.Timer.Stop();
        }
    }

    public partial class PlayRing : UserControl
    {
        public PlayRing()
        {
            this.InitializeComponent();
        }
        public void Ding() => this.Storyboard.Begin(); // Storyboard
    }
}