using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
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
}
