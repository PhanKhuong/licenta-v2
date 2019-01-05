using Homefind.Core.Filters;
using Homefind.Web.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Homefind.Web.Models.PropertyViewModels
{
    public class ListWithFilterModel
    {
        public PropertyFilterSpecification FilterSpecification { get; set; }
        public PagedCollection<PropertyInfoModel> Properties { get; set; }
    }
}
