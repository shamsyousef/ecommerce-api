using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    // Used primary when the specification has a filtering condition (Criteria)
    public class BaseSpecification<T>(Expression<Func<T, bool>>? _Criteria) : ISpecification<T>
    {
        // Used when no filtering condition is needed.
        protected BaseSpecification() : this(null) { }

        public Expression<Func<T, bool>>? Criteria => _Criteria;
        public Expression<Func<T, object>>? OrderBy { get; private set; }
        public Expression<Func<T, object>>? OrderByDescending { get; private set; }
        public bool IsDistinct { get; private set; }
        protected void AddOrderBy(Expression<Func<T, object>> orderByExpression)
        {
            OrderBy = orderByExpression;
        }
        //protected method to be only used by the derived classes to set the OrderByDescending property
        protected void AddOrderByDescending(Expression<Func<T, object>> orderByDescExpression)
        {
            OrderByDescending = orderByDescExpression;
        }
        protected void ApplyDistinct()
        {
            IsDistinct = true;
        }
        public int take { get; private set; }
        public int skip { get; private set; }
        public bool IsPagingEnabled { get; private set; }

        protected void ApplyPaging(int skip, int take)
        {
            this.skip = skip;
            this.take = take;
            IsPagingEnabled = true;
        }
       public IQueryable<T> ApplyCriteria(IQueryable<T> query)
        {
            if (Criteria != null)
            {
                query = query.Where(Criteria);
            }
            return query;
        }
    }
    public class BaseSpecification<T, TResult>(Expression<Func<T, bool>>? criteria) : BaseSpecification<T>(criteria), ISpecification<T, TResult>
    {
        protected BaseSpecification() : this(null) { }
        public Expression<Func<T, TResult>>? Select { get;private set; }

        protected void AddSelect(Expression<Func<T, TResult>> selectExpression)
        {
            Select = selectExpression;
        }
      
    }
}
