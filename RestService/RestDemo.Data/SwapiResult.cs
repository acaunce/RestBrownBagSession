using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestDemo.Data
{
    public class SwapiResult<T>
    {
        public int Count { get; set; }
        public string Next{ get; set; }
        public string Previous { get; set; }
        public IList<T> Results { get; set; } 
    }

    public class SwapiSpecies
    {
        public string Name { get; set; }
        public string Classification { get; set; }
        public string Designation { get; set; }
        public string Language { get; set; }
    }

}
