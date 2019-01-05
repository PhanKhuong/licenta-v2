using System;
using System.Collections.Generic;
using System.Text;

namespace Homefind.Core.Filters
{
    public class PropertyFilterSpecification
    {
        public int? Type { get; set; }
        public int? Bedrooms { get; set; }
        public int? Bathrooms { get; set; }
        public int? AreaFrom { get; set; }
        public int? AreaTo { get; set; }
        public int? PriceFrom { get; set; }
        public int? PriceTo { get; set; }
        public string City { get; set; }
        public string Status { get; set; }
        public bool HasCarParking { get; set; }
        public bool IsFurnished { get; set; }
        public bool HasAirConditioning { get; set; }
        public bool ArePetsAllowed { get; set; }
        public bool HasTv { get; set; }
        public bool HasInternet { get; set; }
    }
}
