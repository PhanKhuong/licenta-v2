using Homefind.Core.Constants;
using Homefind.Core.DomainModels;
using System;
using System.Collections.Generic;

namespace Homefind.Web.Models.PropertyViewModels
{
    public class SubmitPropertyModel
    {
        public SubmitPropertyModel()
        {
            Images = new List<EstateImage>();
        }

        public ListingType Reason { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        public int Balconies { get; set; }
        public int CarpetArea { get; set; }
        public int FloorNumber { get; set; }
        public DateTime DateAvailable { get; set; }
        public string Status { get; set; }
        public int EstateTypeId { get; set; }
        public int EstateLocationId { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
        public bool HasCarParking { get; set; }
        public bool IsFurnished { get; set; }
        public bool HasAirConditioning { get; set; }
        public bool ArePetsAllowed { get; set; }
        public bool HasTv { get; set; }
        public bool HasInternet { get; set; }
        public bool HasSwimmingPool { get; set; }
        public ICollection<EstateImage> Images { get; set; }
    }
}
