using Homefind.Core.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Homefind.Core.Filters
{
    public class SinglePropertyFilter : FilterBase<EstateUnit>
    {
        public SinglePropertyFilter(int propertyId) : base(x => x.Id == propertyId)
        {
            Includes.Add(x => x.EstateFeature);
            Includes.Add(x => x.EstateImages);
            Includes.Add(x => x.EstateLocation);
            Includes.Add(x => x.EstateType);
        }
    }
}
