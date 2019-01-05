using Homefind.Core.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Homefind.Core.Filters
{
    public class PropertyFilter : FilterBase<EstateUnit>
    {
        public PropertyFilter(PropertyFilterSpecification filterSpecs)
            : base(x => (!filterSpecs.Type.HasValue || x.EstateTypeId == filterSpecs.Type)
                    && (!filterSpecs.Bedrooms.HasValue || x.Bedrooms == filterSpecs.Bedrooms)
                    && (!filterSpecs.Bathrooms.HasValue || x.Bathrooms == filterSpecs.Bathrooms)
                    && (!filterSpecs.AreaFrom.HasValue || x.CarpetArea >= filterSpecs.AreaFrom)
                    && (!filterSpecs.AreaTo.HasValue || x.CarpetArea <= filterSpecs.AreaTo)
                    && (!filterSpecs.PriceFrom.HasValue || x.Price >= filterSpecs.PriceFrom)
                    && (!filterSpecs.PriceTo.HasValue || x.Price <= filterSpecs.PriceTo)
                    && (string.IsNullOrEmpty(filterSpecs.City) || filterSpecs.City == "All Cities" || x.EstateLocation.City == filterSpecs.City)
                    && (string.IsNullOrEmpty(filterSpecs.Status) || filterSpecs.Status == "Any Status" || x.Status == filterSpecs.Status)
                    && (!filterSpecs.HasCarParking || Convert.ToBoolean(x.EstateFeature.HasCarParking))
                    && (!filterSpecs.IsFurnished || Convert.ToBoolean(x.EstateFeature.IsFurnished))
                    && (!filterSpecs.HasAirConditioning || Convert.ToBoolean(x.EstateFeature.HasAirConditioning))
                    && (!filterSpecs.ArePetsAllowed || Convert.ToBoolean(x.EstateFeature.ArePetsAllowed))
                    && (!filterSpecs.HasTv || Convert.ToBoolean(x.EstateFeature.HasTv))
                    && (!filterSpecs.HasInternet || Convert.ToBoolean(x.EstateFeature.HasInternet))
                    )
        {
            Includes.Add(x => x.EstateType);
            Includes.Add(x => x.EstateFeature);
            Includes.Add(x => x.EstateLocation);
            Includes.Add(x => x.EstateImages);
        }
    }
}
