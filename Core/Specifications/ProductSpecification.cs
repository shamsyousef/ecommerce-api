using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public class ProductSpecification : BaseSpecification<Product>
    {
        public ProductSpecification(ProductSpecParams specparams) :base(x=>
        (string.IsNullOrEmpty(specparams.Search) || x.Name.ToLower().Contains(specparams.Search,StringComparison.InvariantCultureIgnoreCase)) &&
        (specparams.Brands.Count == 0 || specparams.Brands.Contains(x.Brand)) &&
        (specparams.Types.Count == 0 || specparams.Types.Contains(x.Type)))
            
        {
            ApplyPaging(specparams.PageSize * (specparams.PageIndex - 1), specparams.PageSize);
            switch (specparams.Sort)
            {
                case "priceAsc":
                    AddOrderBy(p => p.Price);
                    break;

                case "priceDesc":
                    AddOrderByDescending(p => p.Price);
                    break;

                default:
                    AddOrderBy(p => p.Name);
                    break;
            }
        }
        


    }
}
