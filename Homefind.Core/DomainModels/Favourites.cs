using System;

namespace Homefind.Core.DomainModels
{
    public class Favourites : BaseEntity
    {
        public string UserId { get; set; }
        public long UserIdNumeric { get; set; }
        public long EstateUnitId { get; set; }
        public EstateUnit EstateUnit { get; set; }
        public DateTime DateAdded { get; set; }
        public int? Views { get; set; }
    }
}
