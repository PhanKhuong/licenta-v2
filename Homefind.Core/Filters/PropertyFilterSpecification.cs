using Homefind.Core.Constants;

namespace Homefind.Core.Filters
{
    public class PropertyFilterSpecification
    {
        public ListingType Reason { get; set; }
        public int? Type { get; set; }
        public int? Bedrooms { get; set; }
        public int? Bathrooms { get; set; }
        public int? AreaFrom { get; set; }
        public int? AreaTo { get; set; }
        public int? PriceFrom { get; set; }
        public int? PriceTo { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Status { get; set; }
        public string HasCarParking { get; set; }
        public string IsFurnished { get; set; }
        public string HasAirConditioning { get; set; }
        public string ArePetsAllowed { get; set; }
        public string HasTv { get; set; }
        public string HasInternet { get; set; }
    }
}
