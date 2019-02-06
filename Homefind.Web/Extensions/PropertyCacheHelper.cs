using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Homefind.Core.DomainModels;
using Homefind.Web.Services;
using Microsoft.Extensions.Caching.Memory;

namespace Homefind.Web.Extensions
{
    public static class PropertyCacheHelper
    {
        public static async Task<IEnumerable<BaseEntity>> GetOrSetCacheEntry
            (IPropertyViewModelService propertyViewModelService,
            IMemoryCache cache,
            CacheKey key)
        {
            IEnumerable<BaseEntity> result;

            if (!cache.TryGetValue(key, out result))
            {
                switch (key)
                {
                    case CacheKey.Property:
                        result = await propertyViewModelService.GetPropertyTypes();
                        break;
                    case CacheKey.Location:
                        result = await propertyViewModelService.GetEstateLocations();
                        break;
                }

                var options = new MemoryCacheEntryOptions().SetAbsoluteExpiration(DateTime.Now.AddMinutes(30));
                cache.Set(key, result, options);
            }

            return result;
        }
    }

    public enum CacheKey
    {
        Property,
        Location
    }
}
