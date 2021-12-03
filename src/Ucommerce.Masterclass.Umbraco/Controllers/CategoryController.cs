using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ucommerce.Api;
using Ucommerce.Infrastructure;
using Ucommerce.Masterclass.Umbraco.Models;
using Ucommerce.Search.Extensions;
using Ucommerce.Search.Facets;
using Ucommerce.Search.Models;
using Ucommerce.Search.Slugs;
using Umbraco.Core;
using Umbraco.Web.Mvc;

namespace Ucommerce.Masterclass.Umbraco.Controllers
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

            parameters.RemoveAll(kvp =>
                new[] { "umbDebugShowTrace", "product", "variant", "category", "categories", "catalog" }
                    .Contains(kvp.Key));

            var facetsForQuerying = new List<Facet>();

            foreach (var parameter in parameters)
            {
                var facet = new Facet { FacetValues = new List<FacetValue>(), Name = parameter.Key };
                foreach (var value in parameter.Value.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    facet.FacetValues.Add(new FacetValue() { Value = value });
                }

                facetsForQuerying.Add(facet);
            }

            return facetsForQuerying;
        }
    }

    public class CategoryController : RenderMvcController
    {
        public ICatalogLibrary CatalogLibrary => ObjectFactory.Instance.Resolve<ICatalogLibrary>();

        public ICatalogContext CatalogContext => ObjectFactory.Instance.Resolve<ICatalogContext>();

        public ITransactionLibrary TransactionLibrary => ObjectFactory.Instance.Resolve<ITransactionLibrary>();

        public IUrlService UrlService => ObjectFactory.Instance.Resolve<IUrlService>();

        public CategoryController()
        {

        }

        [System.Web.Mvc.HttpPost]
        public ActionResult Index(string sku)
        {
            TransactionLibrary.AddToBasket(1, sku);
            return Index();
        }

        [System.Web.Mvc.HttpGet]
        public ActionResult Index()
        {
            var currentCategory = CatalogContext.CurrentCategory;

            var categoryModel = new CategoryViewModel();

            categoryModel.Name = currentCategory.Name;
            categoryModel.ImageMediaUrl = currentCategory.ImageMediaUrl;

            var facetDictionary = GetFacetsDictionary();

            FacetResultSet<Product> facetResultSet = CatalogLibrary.GetProducts(currentCategory.Guid, facetDictionary);

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
            var prices = CatalogLibrary.CalculatePrices(products.Select(x => x.Guid).ToList());

            return products.Select(product => new ProductViewModel()
            {
                LongDescription = product.LongDescription,
                IsVariant = product.ProductType == ProductType.Variant,
                Sellable = product.ProductType == ProductType.Product || product.ProductType == ProductType.Variant,
                PrimaryImageUrl = product.PrimaryImageUrl,
                Sku = product.Sku,
                Name = product.DisplayName,
                Prices = prices.Items.Where(price => price.ProductGuid == product.Guid && price.PriceGroupGuid == CatalogContext.CurrentPriceGroup.Guid).ToList(),
                ShortDescription = product.ShortDescription,
                Url = UrlService.GetUrl(CatalogContext.CurrentCatalog, new[] { CatalogContext.CurrentCategory }, product)
            }).ToList();
        }
    }
}