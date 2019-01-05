using System;
using System.Collections.Generic;

namespace Homefind.Core.DomainModels
{
    public partial class EstateLocation : BaseEntity
    {
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
    }
}
