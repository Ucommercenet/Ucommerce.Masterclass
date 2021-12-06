using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Ucommerce.Api;
using Ucommerce.Infrastructure;
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
            var currentCategory = _catalogContext.CurrentCategory;

            var categoryModel = new CategoryViewModel();

            categoryModel.Name = currentCategory.Name;
            categoryModel.ImageMediaUrl = currentCategory.ImageMediaUrl;

            var facetDictionary = GetFacetsDictionary();

            FacetResultSet<Product> facetResultSet = _catalogLibrary.GetProducts(currentCategory.Guid, facetDictionary);

            categoryModel.Facets = MapFacets(facetResultSet.Facets);
            categoryModel.TotalProductsCount = facetResultSet.TotalCount;
            categoryModel.Products = MapProducts(facetResultSet.Results);

            return View("/views/category/index.cshtml", categoryModel);
        }

        private static FacetDictionary GetFacetsDictionary()
        {
            return System.Web.HttpContext.Current.Request.QueryString.ToFacets().ToFacetDictionary();
        }

        private IList<FacetsViewModel> MapFacets(IList<Facet> facets)
        {
            var facetsToReturn = new List<FacetsViewModel>();

            foreach (var facet in facets)
            {
                var facetsViewModel = new FacetsViewModel();
                facetsViewModel.Key = facet.Name;
                facetsViewModel.DisplayName = facet.DisplayName;

                foreach (var facetValue in facet.FacetValues)
                {
                    facetsViewModel.FacetValues.Add(new FacetValueViewModel()
                    {
                        Count = facetValue.Count,
                        Key = facetValue.Value
                    });
                }

                facetsToReturn.Add(facetsViewModel);
            }

            return facetsToReturn;
        }

        private IList<ProductViewModel> MapProducts(IList<Product> products)
        {
            var prices = _catalogLibrary.CalculatePrices(products.Select(x => x.Guid).ToList());

            return products.Select(product => new ProductViewModel()
            {
                LongDescription = product.LongDescription,
                IsVariant = product.ProductType == ProductType.Variant,
                Sellable = product.ProductType == ProductType.Product || product.ProductType == ProductType.Variant,
                PrimaryImageUrl = product.PrimaryImageUrl,
                Sku = product.Sku,
                Name = product.DisplayName,
                Prices = prices.Items.Where(price => price.ProductGuid == product.Guid && price.PriceGroupGuid == _catalogContext.CurrentPriceGroup.Guid).ToList(),
                ShortDescription = product.ShortDescription,
                Url = _urlService.GetUrl(_catalogContext.CurrentCatalog, new[] { _catalogContext.CurrentCategory }, product)
            }).ToList();
        }
    }
}