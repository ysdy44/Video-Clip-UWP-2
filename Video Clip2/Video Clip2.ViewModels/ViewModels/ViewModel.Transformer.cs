using System.ComponentModel;
using Windows.Graphics.Imaging;

namespace Video_Clip2.ViewModels
{
    public partial class ViewModel : INotifyPropertyChanged
    {


        public BitmapSize Size
        {
            get => this.size;
            set
            {
                this.size = value;
                this.OnPropertyChanged(nameof(Size)); // Notify 
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
            }
        }
        public double scale = 1d;


    }
}