using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Homefind.Web.Models.PropertyViewModels
{
    public class PropertyInfoModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Price { get; set; }
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        public int CarpetArea { get; set; }
        public string EstateTypeDes { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public int AvatarImageId { get; set; }
    }
}
