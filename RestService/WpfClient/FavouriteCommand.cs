using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Windows.Input;
using Newtonsoft.Json;

namespace WpfClient
{
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
}