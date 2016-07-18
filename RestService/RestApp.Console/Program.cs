using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestDemo.Data;

namespace RestApp.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("Getting first page of species from SWAPI");

            using (var client = new HttpClient
            {
                BaseAddress = new Uri("http://swapi.co/api/"),
                DefaultRequestHeaders = {Accept = {MediaTypeWithQualityHeaderValue.Parse("application/json")}}
            })
            {
                var response = client.GetAsync("species").Result;

                var json = response.Content.ReadAsStringAsync().Result;

                var species = JsonConvert.DeserializeObject<SwapiResult<SwapiSpecies>>(json);

                foreach (var spec in species.Results)
                {
                    System.Console.WriteLine($"- {spec.Name}");
                }
            }
            System.Console.ReadLine();
        }
    }
}
