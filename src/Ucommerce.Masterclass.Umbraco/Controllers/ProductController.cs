using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Ucommerce.Api;
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

        public ProductController(ICatalogContext catalogContext, ICatalogLibrary catalogLibrary, ITransactionLibrary transactionLibrary)
        {
            _catalogContext = catalogContext;
            _catalogLibrary = catalogLibrary;
            _transactionLibrary = transactionLibrary;
        }

        [System.Web.Mvc.HttpGet]
        public ActionResult Index()
        {
            //TODO: Task 01 - Fetch and present the current Product
            var currentProduct = _catalogContext.CurrentProduct;

            var productModel = new ProductViewModel
            {
                PrimaryImageUrl = currentProduct.PrimaryImageUrl,
                Name = currentProduct.DisplayName,
                Sku = currentProduct.Sku,
                Prices = _catalogLibrary.CalculatePrices(new List<Guid> { currentProduct.Guid }).Items,
                //TODO: Task 02 - Ensure your code accounts for product Families
                Variants = MapVariants(_catalogLibrary.GetVariants(currentProduct))
            };

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