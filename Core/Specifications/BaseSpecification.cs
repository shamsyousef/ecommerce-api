using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    // Used primary when the specification has a filtering condition (Criteria).
    public class BaseSpecification<T>(Expression<Func<T, bool>> ? _Criteria) :ISpecification<T>
    {
        // Used when no filtering condition is needed.
        protected BaseSpecification():this(null) { }
        
        public Expression<Func<T, bool>> ?Criteria => _Criteria;
    }
}
