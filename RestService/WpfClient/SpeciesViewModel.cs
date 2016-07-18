using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using RestDemo.Data;

namespace WpfClient
{
    public class SpeciesViewModel : SwapiSpecies, INotifyPropertyChanged
    {
        private bool _favourite;
        public bool Favourite
        {
            get { return _favourite; }
            set
            {
                _favourite = value;
                OnPropertyChanged();
            }
        }
        public ICommand FavouriteCommand { get; set; }

        public SpeciesViewModel()
        {

        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}