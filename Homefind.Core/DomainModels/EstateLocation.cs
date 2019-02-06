namespace Homefind.Core.DomainModels
{
    public class EstateLocation : BaseEntity
    {
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
    }
}
