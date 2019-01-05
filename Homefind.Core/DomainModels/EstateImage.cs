namespace Homefind.Core.DomainModels
{
    public partial class EstateImage : BaseEntity
    {
        public int EstateUnitId { get; set; }

        public string Name { get; set; }

        public byte[] Data { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public string ContentType { get; set; }
    }
}
