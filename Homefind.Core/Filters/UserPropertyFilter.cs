using Homefind.Core.DomainModels;

namespace Homefind.Core.Filters
{
    public class UserPropertyFilter : FilterBase<EstateUnit>
    {
        public UserPropertyFilter(string userName)
            : base(x => string.IsNullOrEmpty(userName) || x.PostedBy == userName)
        {
            Includes.Add(x => x.EstateType);
            Includes.Add(x => x.EstateImages);
            Includes.Add(x => x.EstateLocation);
        }
    }
}
