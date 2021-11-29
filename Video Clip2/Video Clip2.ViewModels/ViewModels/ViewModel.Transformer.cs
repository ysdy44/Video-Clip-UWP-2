using System.ComponentModel;
using Video_Clip2.Clips.Clips;
using Windows.Foundation;
using Windows.Graphics.Imaging;

namespace Video_Clip2.ViewModels
{
    public partial class ViewModel : INotifyPropertyChanged
    {


        public Size PreviewSize
        {
            get => this.previewSize;
            set
            {
                this.previewSize = value;
                this.OnPropertyChanged(nameof(PreviewSize)); // Notify 

                this.Invalidate(); // Invalidate
            }
        }
        public Size previewSize = new Size(1920, 1080);


        public BitmapSize Size
        {
            get => this.size;
            set
            {
                this.size = value;
                this.OnPropertyChanged(nameof(Size)); // Notify 

                this.PreviewSize = new Size(this.Scale * value.Width, this.Scale * value.Height);
                this.OnPropertyChanged(nameof(Scale)); // Notify 
            }
        }
        private BitmapSize size = new BitmapSize
        {
            Width = 1920,
            Height = 1080
        };


        public double Scale
        {
            get => this.scale;
            set
            {
                this.scale = value;
                this.OnPropertyChanged(nameof(Scale)); // Notify 

                this.PreviewSize = new Size(value * this.Size.Width, value * this.Size.Height);
                this.OnPropertyChanged(nameof(Scale)); // Notify 
            }
        }
        public double scale = 1d;


    }
}