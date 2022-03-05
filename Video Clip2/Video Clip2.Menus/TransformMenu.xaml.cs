using Video_Clip2.Clips;
using Video_Clip2.Elements;
using Video_Clip2.Transforms;
using Video_Clip2.ViewModels;
using Windows.UI.Xaml.Controls;

namespace Video_Clip2.Menus
{
    public sealed partial class TransformMenu : UserControl, ITransitionElement
    {

        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;

        public TransformMenu()
        {
            this.InitializeComponent();
            this.FlipHorizontalButton.Click += (s, e) =>
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
                                    transformClip.RenderTransform.IsXFlip = !transformClip.RenderTransform.IsXFlip;
                                    transformClip.RenderTransform.ReloadMatrix();
                                }
                                break;
                        }
                    }
                }
                this.ViewModel.Invalidate(); // Invalidate
            };
            this.FlipVerticalButton.Click += (s, e) =>
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
                                    transformClip.RenderTransform.IsYFlip = !transformClip.RenderTransform.IsYFlip;
                                    transformClip.RenderTransform.ReloadMatrix();
                                }
                                break;
                        }
                    }
                }
                this.ViewModel.Invalidate(); // Invalidate
            };
            this.RotateLeftButton.Click += (s, e) =>
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
                                    switch (transformClip.RenderTransform.Rotate)
                                    {
                                        case Rotate.None:
                                            transformClip.RenderTransform.Rotate = Rotate.RotateLeft90;
                                            break;
                                        case Rotate.RotateLeft90:
                                            transformClip.RenderTransform.Rotate = Rotate.Rotate180;
                                            break;
                                        case Rotate.RotateRight90:
                                            transformClip.RenderTransform.Rotate = Rotate.None;
                                            break;
                                        case Rotate.Rotate180:
                                            transformClip.RenderTransform.Rotate = Rotate.RotateRight90;
                                            break;
                                    }
                                    transformClip.RenderTransform.ReloadMatrix();
                                }
                                break;
                        }
                    }
                }
                this.ViewModel.Invalidate(); // Invalidate
            };
            this.RotateRightButton.Click += (s, e) =>
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
                                    switch (transformClip.RenderTransform.Rotate)
                                    {
                                        case Rotate.None:
                                            transformClip.RenderTransform.Rotate = Rotate.RotateRight90;
                                            break;
                                        case Rotate.RotateLeft90:
                                            transformClip.RenderTransform.Rotate = Rotate.None;
                                            break;
                                        case Rotate.RotateRight90:
                                            transformClip.RenderTransform.Rotate = Rotate.Rotate180;
                                            break;
                                        case Rotate.Rotate180:
                                            transformClip.RenderTransform.Rotate = Rotate.RotateLeft90;
                                            break;
                                    }
                                    transformClip.RenderTransform.ReloadMatrix();
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
        }

        public void OnNavigatedTo()
        {
        }
    }
}