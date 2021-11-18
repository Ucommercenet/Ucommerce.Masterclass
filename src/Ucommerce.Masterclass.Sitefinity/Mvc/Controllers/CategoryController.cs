using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Sitefinity.Frontend.Mvc.Infrastructure.Controllers.Attributes;
using Ucommerce.Api;
using Ucommerce.Infrastructure;
using Ucommerce.Masterclass.Sitefinity.Mvc.Models;
using Ucommerce.Search.Extensions;
using Ucommerce.Search.Facets;
using Ucommerce.Search.Models;
using Ucommerce.Search.Slugs;

namespace Ucommerce.Masterclass.Sitefinity.Mvc.Controllers
{
    public static class FacetedQueryStringExtensions
    {
        public static IList<Facet> ToFacets(this NameValueCollection target)
        {
            var parameters = new Dictionary<string, string>();
            foreach (var queryString in HttpContext.Current.Request.QueryString.AllKeys)
            {
                parameters[queryString] = HttpContext.Current.Request.QueryString[queryString];
            }

            parameters.Where(kvp => !(new [] { "umbDebugShowTrace", "product", "variant", "category", "categories", "catalog"}.Contains(kvp.Key)));

            var facetsForQuerying = new List<Facet>();

            foreach (var parameter in parameters)
            {
                var facet = new Facet {FacetValues = new List<FacetValue>(), Name = parameter.Key};
                foreach (var value in parameter.Value.Split(new[] {'|'}, StringSplitOptions.RemoveEmptyEntries))
                {
                    facet.FacetValues.Add(new FacetValue() {Value = value});
                }

                facetsForQuerying.Add(facet);
            }

            return facetsForQuerying;
        }
    }

    [EnhanceViewEngines]
    [Telerik.Sitefinity.Mvc.ControllerToolboxItem(Name = "Category", Title = "Category", SectionName = "MasterClass")]
    public class CategoryController : Controller
    {
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
            return View();
        }

        private IList<FacetsViewModel> MapFacets(IList<Facet> facets)
        {
            var facetsToReturn = new List<FacetsViewModel>();

            return facetsToReturn;
        }

        private IList<ProductViewModel> MapProducts(IList<Product> products)
        {
            return new List<ProductViewModel>();
        }
    }
}