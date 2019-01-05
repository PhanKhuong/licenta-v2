using Homefind.Core.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Homefind.Core.Filters
{
    public class ReviewFilter : FilterBase<Review>
    {
        public ReviewFilter(string userId) 
            : base(x => x.RatedUserId == userId)
        {
        }
    }
}
