using System;
using System.Collections.Generic;

namespace Ucommerce.Masterclass.Umbraco.Models
{
    public class CategoryViewModel
    {
        public CategoryViewModel()
        {
            Categories = new List<CategoryViewModel>();
            ProductViewModels = new List<ProductViewModel>();
            Facets = new List<FacetsViewModel>();
        }

        public string Url { get; set; }

        public string Name { get; set; }

        public Guid Guid { get; set; }

        public IList<CategoryViewModel> Categories { get; set; }

        public IList<ProductViewModel> ProductViewModels { get; set; }

        public string ImageMediaUrl { get; set; }
        public int TotalProductsCount { get; set; }
        public IList<FacetsViewModel> Facets { get; set; }
    }
}