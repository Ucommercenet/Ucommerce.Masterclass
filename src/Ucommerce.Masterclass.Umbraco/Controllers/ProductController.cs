using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Ucommerce.Api;
using Ucommerce.Infrastructure;
using Ucommerce.Masterclass.Umbraco.Models;
using Ucommerce.Search;
using Ucommerce.Search.Models;
using Umbraco.Web.Mvc;

namespace Ucommerce.Masterclass.Umbraco.Controllers
{
    public class ProductController : RenderMvcController
    {
        public ICatalogContext CatalogContext => ObjectFactory.Instance.Resolve<ICatalogContext>();
        
        public ICatalogLibrary CatalogLibrary => ObjectFactory.Instance.Resolve<ICatalogLibrary>();

        [System.Web.Mvc.HttpGet]
        public ActionResult Index()
        {
            var currentProduct = CatalogContext.CurrentProduct;
            
            var productModel = new ProductViewModel();
            productModel.PrimaryImageUrl = currentProduct.PrimaryImageUrl;
            productModel.Name = currentProduct.DisplayName;

            productModel.Prices = CatalogLibrary.CalculatePrices(new List<Guid>() {currentProduct.Guid}).Items;
            productModel.Variants = MapVariants(CatalogLibrary.GetVariants(currentProduct));            
            return View(productModel);
        }

        private IList<ProductViewModel> MapVariants(ResultSet<Product> variants)
        {
           return variants.Select(x =>
                new ProductViewModel
                {
                    Name = x.DisplayName,
                    VariantSku = x.VariantSku
                }).ToList();
        }
    }
}