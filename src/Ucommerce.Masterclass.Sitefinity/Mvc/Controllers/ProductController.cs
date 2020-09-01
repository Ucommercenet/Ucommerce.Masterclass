using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Ucommerce.Api;
using Ucommerce.Infrastructure;
using Ucommerce.Masterclass.Sitefinity.Mvc.Models;
using Ucommerce.Search;
using Ucommerce.Search.Models;

namespace Ucommerce.Masterclass.Sitefinity.Mvc.Controllers
{
    [Telerik.Sitefinity.Mvc.ControllerToolboxItem(Name = "Product", Title = "Product", SectionName = "MasterClass")]
    public class ProductController : Controller
    {
        public ICatalogContext CatalogContext => ObjectFactory.Instance.Resolve<ICatalogContext>();
        
        public ICatalogLibrary CatalogLibrary => ObjectFactory.Instance.Resolve<ICatalogLibrary>();


        public ITransactionLibrary TransactionLibrary => ObjectFactory.Instance.Resolve<ITransactionLibrary>();

        
        [System.Web.Mvc.HttpGet]
        public ActionResult Index()
        {
            var currentProduct = CatalogContext.CurrentProduct;
            
            var productModel = new ProductViewModel();
            productModel.PrimaryImageUrl = currentProduct.PrimaryImageUrl;
            productModel.Name = currentProduct.DisplayName;
            productModel.Sku = currentProduct.Sku;
            
            productModel.Prices = CatalogLibrary.CalculatePrices(new List<Guid>() {currentProduct.Guid}).Items;
            productModel.Variants = MapVariants(CatalogLibrary.GetVariants(currentProduct));            
            return View(productModel);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult Index(string sku, string variantSku, int quantity)
        {
            TransactionLibrary.AddToBasket(quantity, sku, variantSku);
            return Index();
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