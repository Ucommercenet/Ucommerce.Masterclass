using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Telerik.Sitefinity.Frontend.Mvc.Infrastructure.Controllers.Attributes;
using Ucommerce.Api;
using Ucommerce.Infrastructure;
using Ucommerce.Masterclass.Sitefinity.Mvc.Models;
using Ucommerce.Search;
using Ucommerce.Search.Models;

namespace Ucommerce.Masterclass.Sitefinity.Mvc.Controllers
{
    [EnhanceViewEngines]
    [Telerik.Sitefinity.Mvc.ControllerToolboxItem(Name = "Product", Title = "Product", SectionName = "MasterClass")]
    public class ProductController : Controller
    {
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
           return variants.Select(x =>
                new ProductViewModel
                {

                }).ToList();
        }
    }
}