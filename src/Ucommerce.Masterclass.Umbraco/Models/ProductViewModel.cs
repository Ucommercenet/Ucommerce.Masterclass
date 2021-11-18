using System.Collections.Generic;
using Ucommerce.Catalog.Models;

namespace Ucommerce.Masterclass.Umbraco.Models
{
    public class ProductListViewModel
    {
        public ProductListViewModel()
        {
            ProductViewModels = new List<ProductViewModel>();
        }
        
        public IList<ProductViewModel> ProductViewModels { get; set; }
    }
    public class ProductViewModel
    {
        public ProductViewModel()
        {
            Variants = new List<ProductViewModel>();
        }
        public bool IsVariant { get; set; }

        public bool Sellable { get; set; }
        
        public string Name { get; set; }

        public string Url { get; set; }

        public string LongDescription { get; set; }
        
        public string ShortDescription { get; set; }
        
        public IList<ProductViewModel> Variants { get; set; }

        public string Sku { get; set; }

        public string VariantSku { get; set; }

        public IList<ProductPriceCalculationResult.Item> Prices { get; set; }
        public string PrimaryImageUrl { get; set; }
    }
}