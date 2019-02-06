using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Homefind.Core.Interfaces;

namespace Homefind.Core.Filters
{
    public abstract class FilterBase<T> : IFilter<T>
    {
        public FilterBase(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
        }

        public Expression<Func<T, bool>> Criteria { get; }
        public List<Expression<Func<T, object>>> Includes { get; }
            = new List<Expression<Func<T, object>>>();

        protected void IncludeSubsetExpression(Expression<Func<T, object>> expression)
        {
            Includes.Add(expression);
        }
    }
}
