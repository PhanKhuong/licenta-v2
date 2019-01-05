using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Homefind.Web.Models.PropertyViewModels
{
    public class FavouritesModel
    {
        public int? Views { get; set; }
        public string Title { get; set; }
        public int Price { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public DateTime DateAdded { get; set; }
    }
}
