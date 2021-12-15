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
        public CategoryController()
        {

        }

        [System.Web.Mvc.HttpGet]
        public ActionResult Index()
        {
            var categoryModel = new CategoryViewModel();

            return View("/views/category/index.cshtml", categoryModel);
        }
        
        private FacetDictionary GetFacetsDictionary()
        {
            return System.Web.HttpContext.Current.Request.QueryString.ToFacets().ToFacetDictionary();
        }

        private IList<ProductViewModel> MapProducts(IList<Product> products)
        {
            throw new NotImplementedException();
        }

        private IList<FacetsViewModel> MapFacets(IList<Facet> facets)
        {
            throw new NotImplementedException();
        }
    }
}