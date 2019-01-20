using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Homefind.Web.Models.PropertyViewModels
{
    public class ReviewModel
    {
        public string RatedUserId { get; set; }
        public string Reviewer { get; set; }
        public string ReviewerName { get; set; }
        public string ReviewerEmail { get; set; }
        public string Rating { get; set; }
        public string Description { get; set; }
        public string Comment { get; set; }
        public DateTime Date { get; set; }
        [NotMapped]
        public string DateFormatted { get; set; }
        [NotMapped]
        public int ReviewedProperty { get; set; }
    }
}
