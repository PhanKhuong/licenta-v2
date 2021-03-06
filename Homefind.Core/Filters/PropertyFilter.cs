﻿using System;
using Homefind.Core.Constants;
using Homefind.Core.DomainModels;

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
                    && (string.IsNullOrEmpty(filterSpecs.City) || filterSpecs.City == Constants.Constants.SelectAllCities || x.EstateLocation.City == filterSpecs.City)
                    && (string.IsNullOrEmpty(filterSpecs.Address) || x.EstateLocation.Address.Contains(filterSpecs.Address))
                    && (filterSpecs.Reason == ListingType.All || x.Reason == filterSpecs.Reason)
                    && (!Convert.ToBoolean(filterSpecs.HasCarParking) || x.EstateFeature.HasCarParking)
                    && (!Convert.ToBoolean(filterSpecs.IsFurnished) || x.EstateFeature.IsFurnished)
                    && (!Convert.ToBoolean(filterSpecs.HasAirConditioning) || x.EstateFeature.HasAirConditioning)
                    && (!Convert.ToBoolean(filterSpecs.ArePetsAllowed) || x.EstateFeature.ArePetsAllowed)
                    && (!Convert.ToBoolean(filterSpecs.HasTv) || x.EstateFeature.HasTv)
                    && (!Convert.ToBoolean(filterSpecs.HasInternet) || x.EstateFeature.HasInternet)
                    )
        {
            Includes.Add(x => x.EstateType);
            Includes.Add(x => x.EstateFeature);
            Includes.Add(x => x.EstateLocation);
            Includes.Add(x => x.EstateImages);
        }
    }
}
