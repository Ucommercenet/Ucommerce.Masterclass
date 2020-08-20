using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Ucommerce.Api;
using Ucommerce.Infrastructure;
using Ucommerce.Masterclass.Umbraco.Models;
using Ucommerce.Search.Facets;
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

            var facetResultSet = CatalogLibrary.GetProducts(currentCategory.Guid, new FacetDictionary());
            
            categoryModel.Facets = MapFacets(facetResultSet.Facets);
            categoryModel.TotalProductsCount = facetResultSet.TotalCount;
            categoryModel.Products = MapProducts(facetResultSet.Results);
            
            return View("/views/category/index.cshtml", categoryModel);
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
            return products.Select(x => new ProductViewModel()
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