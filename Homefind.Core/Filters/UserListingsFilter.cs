using Homefind.Core.DomainModels;

namespace Homefind.Core.Filters
{
    public class UserListingsFilter : FilterBase<EstateUnit>
    {
        public UserListingsFilter(string userName) 
            : base(x => !string.IsNullOrEmpty(userName) && x.PostedBy == userName)
        {
        }
    }
}
