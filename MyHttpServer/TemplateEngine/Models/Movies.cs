
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHttpServer.Models
{
    public class Movies
    {
        public int Id { get; set; }
        public string film_name { get; set; }
        public int release_year { get; set; }
        public double rating { get; set; }
        public string description { get; set; }
        public string poster_url {  get; set; }
        public string card_url {  get; set; }

    }
}
