using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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
    public partial class MainWindow : Window
    {
        public ObservableCollection<SpeciesViewModel> Species { get; } = new ObservableCollection<SpeciesViewModel>();

        public MainWindow()
        {
            InitializeComponent();

            using (var client = new HttpClient
            {
                BaseAddress = new Uri("http://swapi.co/api/"),
                DefaultRequestHeaders = { Accept = { MediaTypeWithQualityHeaderValue.Parse("application/json") } }
            })
            {
                var response = client.GetAsync("species").Result;

                var json = response.Content.ReadAsStringAsync().Result;

                var species = JsonConvert.DeserializeObject<SwapiResult<SwapiSpecies>>(json);

                foreach (var speciesViewModel in species.Results.Select(s => new SpeciesViewModel
                {
                    Classification = s.Classification,
                    Designation = s.Designation,
                    Favourite = false,
                    Language = s.Language,
                    Name = s.Name
                }).ToList())
                {
                    Species.Add(speciesViewModel);
                }
            }
        }
    }

    public class SpeciesViewModel : SwapiSpecies
    {
        public bool Favourite { get; set; }
    }
}
