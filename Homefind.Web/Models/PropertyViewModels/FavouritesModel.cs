using System;
using Homefind.Core.DomainModels;

namespace Homefind.Web.Models.PropertyViewModels
{
    public class FavouritesModel
    {
        public long Id { get; set; }
        public int? Views { get; set; }
        public int EstateUnitId { get; set; }
        public string Title { get; set; }
        public int Price { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public DateTime DateAdded { get; set; }
        public int AvatarImageId { get; set; }
    }
}
