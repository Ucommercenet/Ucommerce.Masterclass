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
        private readonly ICatalogContext _catalogContext;
        private readonly ICatalogLibrary _catalogLibrary;
        private readonly ITransactionLibrary _transactionLibrary;

        public ProductController(ICatalogContext catalogContext, ICatalogLibrary catalogLibrary, ITransactionLibrary transactionLibrary )
        {
            _catalogContext = catalogContext;
            _catalogLibrary = catalogLibrary;
            _transactionLibrary = transactionLibrary;
        }

        [System.Web.Mvc.HttpGet]
        public ActionResult Index()
        {
            var currentProduct = _catalogContext.CurrentProduct;

            var productModel = new ProductViewModel();
            productModel.PrimaryImageUrl = currentProduct.PrimaryImageUrl;
            productModel.Name = currentProduct.DisplayName;
            productModel.Sku = currentProduct.Sku;

            productModel.Prices = _catalogLibrary.CalculatePrices(new List<Guid>() { currentProduct.Guid }).Items;
            productModel.Variants = MapVariants(_catalogLibrary.GetVariants(currentProduct));
            return View(productModel);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult Index(string sku, string variantSku, int quantity)
        {
            _transactionLibrary.AddToBasket(quantity, sku, variantSku);
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