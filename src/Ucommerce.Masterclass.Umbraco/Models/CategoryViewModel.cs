using System;
using System.Collections.Generic;

namespace Ucommerce.Masterclass.Umbraco.Models
{
    public class CategoryViewModel
    {
        public CategoryViewModel()
        {
            Categories = new List<CategoryViewModel>();
            Products = new List<ProductViewModel>();
        }
        public string Url { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public Guid Guid { get; set; }
        
        public IList<CategoryViewModel> Categories { get; set; }

        public IList<ProductViewModel> Products { get; set; }
        
        public string ImageMediaUrl { get; set; }
    }
}