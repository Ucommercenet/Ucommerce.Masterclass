using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Ucommerce.Masterclass.Umbraco.Models;
using Ucommerce.Search.Facets;
using Ucommerce.Search.Models;
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
            throw new NotImplementedException();
        }

        private IList<FacetsViewModel> MapFacets(IEnumerable<Facet> facets)
        {
            throw new NotImplementedException();
        }

        private IList<ProductViewModel> MapProducts(IList<Product> products)
        {
            throw new NotImplementedException();
        }
    }
}