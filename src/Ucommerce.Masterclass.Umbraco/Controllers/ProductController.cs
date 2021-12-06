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
            var productModel = new ProductViewModel();
            
            return View(productModel);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult Index(string sku, string variantSku, int quantity)
        {
            return Index();
        }

        private IList<ProductViewModel> MapVariants(ResultSet<Product> variants)
        {
            throw new NotImplementedException();
        }
    }
}