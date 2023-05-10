using System;
using System.Collections.Generic;

namespace Ucommerce.Masterclass.Umbraco.Models
{
    public class CategoryNavigationViewModel
    {
        public CategoryNavigationViewModel()
        {
            CategoriesViewModels = new List<CategoryViewModel>();
        }
        public IList<CategoryViewModel> CategoriesViewModels { get; set; }

        public Guid CurrentCategoryGuid { get; set; }
    }
}