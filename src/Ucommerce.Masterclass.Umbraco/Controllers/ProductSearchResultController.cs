﻿using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Ucommerce.Api;
using Ucommerce.Infrastructure;
using Ucommerce.Masterclass.Umbraco.Models;
using Ucommerce.Search;
using Ucommerce.Search.Models;
using Ucommerce.Search.Slugs;
using Umbraco.Web.Mvc;

namespace Ucommerce.Masterclass.Umbraco.Controllers
{
    public class ProductSearchResultController : RenderMvcController
    {
        public ICatalogLibrary CatalogLibrary => ObjectFactory.Instance.Resolve<ICatalogLibrary>();
        public IUrlService UrlService => ObjectFactory.Instance.Resolve<IUrlService>();

        public ICatalogContext CatalogContext => ObjectFactory.Instance.Resolve<ICatalogContext>();

        private string GetSearchTerm()
        {
            return Request.QueryString["Query"];
        }
        
        public ProductSearchResultController()
        {
            
        }

        public ActionResult Index()
        {
            var model = new ProductListViewModel();

            var searchTerm = GetSearchTerm();

            return View(model);
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
                Url = UrlService.GetUrl(CatalogContext.CurrentCatalog, product)
            }).ToList();
        }
    }
}