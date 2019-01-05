using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Homefind.Web.Models.PropertyViewModels
{
    public class ReviewModel
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
