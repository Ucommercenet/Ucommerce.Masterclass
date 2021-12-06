using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Ucommerce.Api;
using Ucommerce.Masterclass.Umbraco.Extensions;
using Ucommerce.Masterclass.Umbraco.Models;
using Ucommerce.Search;
using Ucommerce.Search.Extensions;
using Ucommerce.Search.Facets;
using Ucommerce.Search.Models;
using Ucommerce.Search.Slugs;
using Umbraco.Web.Mvc;

namespace Ucommerce.Masterclass.Umbraco.Controllers
{
    public class CategoryController : RenderMvcController
    {
        private readonly ICatalogLibrary _catalogLibrary;
        private readonly ICatalogContext _catalogContext;
        private readonly IUrlService _urlService;

        public CategoryController(ICatalogLibrary catalogLibrary, ICatalogContext catalogContext,
            IUrlService urlService)
        {
            _catalogLibrary = catalogLibrary;
            _catalogContext = catalogContext;
            _urlService = urlService;
        }

        [System.Web.Mvc.HttpGet]
        public ActionResult Index()
        {
            var categoryModel = new CategoryViewModel();

            return View("/views/category/index.cshtml", categoryModel);
        }

        private IList<ProductViewModel> MapProducts(IList<Product> products)
        {
            throw new NotImplementedException();
        }
    }
}