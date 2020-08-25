using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Ucommerce.Api;
using Ucommerce.Infrastructure;
using Ucommerce.Masterclass.Umbraco.Models;
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
            
            
            return View(productModel);
        }
    }
}