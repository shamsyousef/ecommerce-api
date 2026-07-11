using Core.Entities;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Infrastructure.Data
{
    public class SpecificationEvaluator<T> where T : BaseEntity
    {
        public static IQueryable<T> GetQuery(IQueryable<T> query , ISpecification<T> spec)
        {
            if (spec.Criteria!=null)
            {
                query = query.Where(spec.Criteria);
            }
            if (spec.OrderBy != null)
            {
                query = query.OrderBy(spec.OrderBy);
            }
            if (spec.OrderByDescending != null) 
            {
                query = query.OrderByDescending(spec.OrderByDescending);
            }
            if (spec.IsDistinct)
            {
                query = query.Distinct();
            }
            if (spec.IsPagingEnabled)
            {
                query = query.Skip(spec.skip).Take(spec.take);
            }
            return query;
        }
        public static IQueryable<TResult> GetQuery<TSpec,TResult> (IQueryable<T> query , ISpecification<T,TResult> spec)
        {


            if (spec.Criteria != null)
            {
                query = query.Where(spec.Criteria);
            }
            if (spec.OrderBy != null)
            {
                query = query.OrderBy(spec.OrderBy);
            }
            if (spec.OrderByDescending != null)
            {
                query = query.OrderByDescending(spec.OrderByDescending);
            }
            var selectedquery= query as IQueryable <TResult>;
            if(spec.Select!= null)
            {
                selectedquery = query.Select(spec.Select);
            }
            if(spec.IsDistinct)
            {
                selectedquery = selectedquery?.Distinct();
            }
            if(spec.IsPagingEnabled)
            {
                selectedquery = selectedquery?.Skip(spec.skip).Take(spec.take);
            }

            return selectedquery ?? query.Cast<TResult>();

        }
    }
}
