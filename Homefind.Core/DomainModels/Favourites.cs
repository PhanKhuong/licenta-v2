using System;
using System.Collections.Generic;
using System.Text;

namespace Homefind.Core.DomainModels
{
    public class Favourites : BaseEntity
    {
        public string UserId { get; set; }
        public int EstateUnitId { get; set; }
        public EstateUnit EstateUnit { get; set; }
        public DateTime DateAdded { get; set; }
        public int? Views { get; set; }
    }
}
