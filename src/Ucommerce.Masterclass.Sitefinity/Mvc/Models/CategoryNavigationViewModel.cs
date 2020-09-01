using System;
using System.Collections.Generic;

namespace Ucommerce.Masterclass.Sitefinity.Mvc.Models
{
    public class CategoryNavigationViewModel
    {
        public CategoryNavigationViewModel()
        {
            Categories = new List<CategoryViewModel>();
        }
        public IList<CategoryViewModel> Categories { get; set; } 
        
        public Guid CurrentCategoryGuid { get; set; }
    }
}