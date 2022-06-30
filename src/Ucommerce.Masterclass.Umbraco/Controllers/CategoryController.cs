using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Ucommerce.Api;
using Ucommerce.Masterclass.Umbraco.Extensions;
using Ucommerce.Masterclass.Umbraco.Models;
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
            //TODO: Task 01: Present the category
            var currentCategory = _catalogContext.CurrentCategory;
            var facetDictionary = GetFacetsDictionary();
            var facetResultSet = _catalogLibrary.GetProducts(currentCategory.Guid, facetDictionary);

            var categoryModel = new CategoryViewModel
            {
                Name = currentCategory.Name,
                ImageMediaUrl = currentCategory.ImageMediaUrl,
                //TODO: Task 02: Present the 'TotalProductsCount', 'Facets', and 'Products' within the current category
                Facets = MapFacets(facetResultSet.Facets),
                TotalProductsCount = facetResultSet.TotalCount,
                Products = MapProducts(facetResultSet.Results)
            };

            return View("/views/category/index.cshtml", categoryModel);
        }

        private FacetDictionary GetFacetsDictionary()
        {
            return System.Web.HttpContext.Current.Request.QueryString.ToFacets().ToFacetDictionary();
        }

        private IList<FacetsViewModel> MapFacets(IEnumerable<Facet> facets)
        {
            return facets.Select(facet => new FacetsViewModel
            {
                Key = facet.Name, DisplayName = facet.DisplayName,
                FacetValues = facet.FacetValues.Select(facetValue => new FacetValueViewModel
                    { Count = facetValue.Count, Key = facetValue.Value }).ToList()
            }).ToList();
        }

        private IList<ProductViewModel> MapProducts(IList<Product> products)
        {
            var prices = _catalogLibrary.CalculatePrices(products.Select(x => x.Guid).ToList());

            return products.Select(product => new ProductViewModel
            {
                LongDescription = product.LongDescription,
                IsVariant = product.ProductType == ProductType.Variant,
                Sellable = product.ProductType == ProductType.Product || product.ProductType == ProductType.Variant,
                PrimaryImageUrl = product.PrimaryImageUrl,
                Sku = product.Sku,
                Name = product.DisplayName,
                Prices = prices.Items.Where(price =>
                        price.ProductGuid == product.Guid &&
                        price.PriceGroupGuid == _catalogContext.CurrentPriceGroup.Guid)
                    .ToList(),
                ShortDescription = product.ShortDescription,
                Url = _urlService.GetUrl(_catalogContext.CurrentCatalog, new[] { _catalogContext.CurrentCategory },
                    product)
            }).ToList();
        }
    }
}