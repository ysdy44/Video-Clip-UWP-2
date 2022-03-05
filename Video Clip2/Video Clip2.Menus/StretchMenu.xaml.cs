using Video_Clip2.Clips;
using Video_Clip2.Elements;
using Video_Clip2.Transforms;
using Video_Clip2.ViewModels;
using Windows.UI.Xaml.Controls;

namespace Video_Clip2.Menus
{
    public sealed partial class StretchMenu : UserControl, ITransitionElement
    {

        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;

        public StretchMenu()
        {
            this.InitializeComponent();
            this.ListView.StretchChanged += (s, stretch) =>
            {
                foreach (Clipping item in this.ViewModel.ObservableCollection)
                {
                    IClip clip = item.Self;

                    if (clip.IsSelected)
                    {
                        switch (clip.Type)
                        {
                            case ClipType.Image:
                            case ClipType.Video:
                                if (clip is IRenderTransform transformClip)
                                {
                                    transformClip.RenderTransform.Stretch = stretch;
                                    transformClip.RenderTransform.ReloadMatrix();
                                }
                                break;
                        }
                    }
                }
                this.ViewModel.Invalidate(); // Invalidate
            };
            this.ListView.IsOverlayLayerChanged += (s, isOverlayLayer) =>
            {
                foreach (Clipping item in this.ViewModel.ObservableCollection)
                {
                    IClip clip = item.Self;

                    if (clip.IsSelected)
                    {
                        switch (clip.Type)
                        {
                            case ClipType.Image:
                            case ClipType.Video:
                                if (clip is IOverlayLayer transformClip)
                                {
                                    transformClip.IOverlayLayerCore = isOverlayLayer;
                                }
                                break;
                        }
                    }
                }
                this.ViewModel.Invalidate(); // Invalidate
            };
        }

        public void OnNavigatedFrom()
        {
            foreach (Clipping item in this.ViewModel.ObservableCollection)
            {
                IClip clip = item.Self;

                if (clip.IsSelected)
                {
                    switch (clip.Type)
                    {
                        case ClipType.Image:
                        case ClipType.Video:
                            if (clip is IOverlayLayer transformClip)
                            {
                                this.ListView.IsOverlayLayer = transformClip.IOverlayLayerCore;
                                this.ListView.Stretch = transformClip.RenderTransform.Stretch;
                                break;
                            }
                            break;
                    }
                }
            }
        }

        public void OnNavigatedTo()
        {
        }
    }
}