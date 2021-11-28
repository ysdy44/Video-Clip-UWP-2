using System.ComponentModel;
using Video_Clip2.Clips;
using Windows.UI.Xaml.Controls;

namespace Video_Clip2.ViewModels
{
    public partial class ViewModel : INotifyPropertyChanged
    {

        public ListViewSelectionMode SelectionMode
        {
            get => this.selectionMode;
            private set
            {
                switch (value)
                {
                    case ListViewSelectionMode.Single:
                        this.SelectionUnNone = true;
                        this.SelectionSingle = true;
                        this.SelectionMultiple = false;
                        break;
                    case ListViewSelectionMode.Multiple:
                        this.SelectionUnNone = true;
                        this.SelectionSingle = false;
                        this.SelectionMultiple = true;
                        break;
                    default:
                        this.SelectionUnNone = false;
                        this.SelectionSingle = false;
                        this.SelectionMultiple = false;
                        break;
                }
                this.OnPropertyChanged(nameof(SelectionUnNone)); // Notify 
                this.OnPropertyChanged(nameof(SelectionSingle)); // Notify 
                this.OnPropertyChanged(nameof(SelectionMultiple)); // Notify 


                this.selectionMode = value;
                this.OnPropertyChanged(nameof(SelectionMode)); // Notify 
            }
        }
        private ListViewSelectionMode selectionMode;

        public bool SelectionUnNone { get; private set; }
        public bool SelectionSingle { get; private set; }
        public bool SelectionMultiple { get; private set; }


        //////////////////////////

        
        public Trimmer Trimmer
        {
            get => this.trimmer;
            set
            {
                this.trimmer = value;
                this.OnPropertyChanged(nameof(Trimmer)); // Notify 
            }
        }
        private Trimmer trimmer;
        public Trimmer StartingTrimmer { get; private set; }
        public void CacheTrimmer() => this.StartingTrimmer = this.Trimmer;

    }
}