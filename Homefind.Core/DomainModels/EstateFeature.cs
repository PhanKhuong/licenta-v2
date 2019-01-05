namespace Homefind.Core.DomainModels
{
    public partial class EstateFeature : BaseEntity
    {
        public bool HasCarParking { get; set; }
        public bool IsFurnished { get; set; }
        public bool HasAirConditioning { get; set; }
        public bool ArePetsAllowed { get; set; }
        public bool HasTv { get; set; }
        public bool HasInternet { get; set; }
        public bool HasSwimmingPool { get; set; }
    }
}
