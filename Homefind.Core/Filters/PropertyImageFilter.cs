using Homefind.Core.DomainModels;

namespace Homefind.Core.Filters
{
    public class PropertyImageFilter : FilterBase<EstateImage>
    {
        public PropertyImageFilter(long propertyId) 
            : base(x => x.EstateUnitId == propertyId)
        {
        }
    }
}
