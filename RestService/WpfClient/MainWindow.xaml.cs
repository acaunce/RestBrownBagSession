using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;
using RestDemo.Data;

namespace WpfClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public ObservableCollection<SpeciesViewModel> Species { get; } = new ObservableCollection<SpeciesViewModel>();

        public MainWindow()
        {
            InitializeComponent();

            GetData();
        }

        private void GetData()
        {
            using (var client = new HttpClient
            {
                BaseAddress = new Uri("http://swapi.co/api/"),
                DefaultRequestHeaders = { Accept = { MediaTypeWithQualityHeaderValue.Parse("application/json") } }
            })
            {
                var response = client.GetAsync("species").Result;

                var json = response.Content.ReadAsStringAsync().Result;

                var species = JsonConvert.DeserializeObject<SwapiResult<SwapiSpecies>>(json);
                Species.Clear();
                foreach (var speciesViewModel in species.Results.Select(s => new SpeciesViewModel
                {
                    Classification = s.Classification,
                    Designation = s.Designation,
                    Favourite = false,
                    Language = s.Language,
                    Name = s.Name
                }).ToList())
                {
                    speciesViewModel.FavouriteCommand = new FavouriteCommand(GetFaves);
                    Species.Add(speciesViewModel);
                }
                GetFaves();
            }
        }
        public void GetFaves()
        {
            using (var client = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:50421/"),
                DefaultRequestHeaders = { Accept = { MediaTypeWithQualityHeaderValue.Parse("application/json") } }
            })
            {
                var response = client.GetAsync("favourites").Result;

                var json = response.Content.ReadAsStringAsync().Result;

                var species = JsonConvert.DeserializeObject<FavouriteSpecies[]>(json);

                foreach (var speciesViewModel in Species)
                {
                    speciesViewModel.Favourite = false;
                    foreach (var fave in species)
                    {
                        if (speciesViewModel.Name == fave.Name) speciesViewModel.Favourite = true;
                    }
                }
                OnPropertyChanged("Species");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Refresh(object sender, RoutedEventArgs e)
        {
            GetData();
        }
    }

    public class FavouriteCommand : ICommand
    {
        private readonly Action _getFaves = null;

        public FavouriteCommand(Action getFaves)
        {
            _getFaves = getFaves;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            using (var client = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:50421/"),
                DefaultRequestHeaders = { Accept = { MediaTypeWithQualityHeaderValue.Parse("application/json") } }
            })
            {
                var row = parameter as SpeciesViewModel;
                var result = client.PostAsync("favourites", new StringContent(JsonConvert.SerializeObject(new { Name = row.Name }), Encoding.UTF8, "application/json")).Result;
                row.Favourite = true;

                _getFaves.Invoke();
            }

        }

        public event EventHandler CanExecuteChanged;
    }


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
