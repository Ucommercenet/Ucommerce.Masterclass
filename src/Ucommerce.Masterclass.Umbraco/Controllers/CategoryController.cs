using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Ucommerce.Api;
using Ucommerce.Infrastructure;
using Ucommerce.Masterclass.Umbraco.Models;
using Ucommerce.Search.Models;
using Ucommerce.Search.Slugs;
using Umbraco.Web.Mvc;

namespace Ucommerce.Masterclass.Umbraco.Controllers
{
    public class CategoryController : RenderMvcController
    {
        public ICatalogLibrary CatalogLibrary => ObjectFactory.Instance.Resolve<ICatalogLibrary>();

        public ICatalogContext CatalogContext => ObjectFactory.Instance.Resolve<ICatalogContext>();

        public IUrlService UrlService => ObjectFactory.Instance.Resolve<IUrlService>();

        public CategoryController()
        {
            
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult Index(string sku)
        {
            return Index();
        }

        [System.Web.Mvc.HttpGet]
        public ActionResult Index()
        {
            var currentCategory = CatalogContext.CurrentCategory;
            
            var categoryModel = new CategoryViewModel();

            categoryModel.Name = currentCategory.Name;
            categoryModel.ImageMediaUrl = currentCategory.ImageMediaUrl;

            categoryModel.Products = MapProducts(currentCategory);
            
            return View("/views/category/index.cshtml", categoryModel);
        }

        private IList<ProductViewModel> MapProducts(Category currentCategory)
        {
            return CatalogLibrary.GetProducts(currentCategory.Guid).Select(x => new ProductViewModel()
            {
                LongDescription = x.LongDescription,
                
                IsVariant = x.ProductType == ProductType.Variant,
                Sellable = x.ProductType == ProductType.Product || x.ProductType == ProductType.Variant,
                PrimaryImageUrl = x.PrimaryImageUrl,
                Sku = x.Sku,
                Name = x.DisplayName,
                ShortDescription = x.ShortDescription,
                Url = UrlService.GetUrl(CatalogContext.CurrentCatalog, new []{ CatalogContext.CurrentCategory }, x)
                
            }).ToList();
        }
    }
}