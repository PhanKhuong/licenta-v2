using System;
using System.Collections.Generic;
using System.Text;

namespace Homefind.Core.DomainModels
{
    public class Review : BaseEntity
    {
        public string RatedUserId { get; set; }
        public string Reviewer { get; set; }
        public string ReviewerEmail { get; set; }
        public string Rating { get; set; }
        public string Description { get; set; }
        public string Comment { get; set; }
        public DateTime Date { get; set; }
    }
}
