using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Ucommerce.Api;
using Ucommerce.Masterclass.Umbraco.Models;
using Ucommerce.Search;
using Ucommerce.Search.Models;
using Ucommerce.Search.Slugs;
using Umbraco.Web.Mvc;

namespace Ucommerce.Masterclass.Umbraco.Controllers
{
    public class ProductSearchResultController : RenderMvcController
    {
        private readonly ICatalogLibrary _catalogLibrary;
        private readonly IUrlService _urlService;
        private readonly ICatalogContext _catalogContext;
        private readonly IIndex<Product> _productIndex;

        public ProductSearchResultController(ICatalogLibrary catalogLibrary, IUrlService urlService, 
            ICatalogContext catalogContext, IIndex<Ucommerce.Search.Models.Product> productIndex)
        {
            _catalogLibrary = catalogLibrary;
            _urlService = urlService;
            _catalogContext = catalogContext;
            _productIndex = productIndex;
        }

        public ActionResult Index()
        {
            var model = new ProductListViewModel();

            var searchTerm = GetSearchTerm();

            var result = _productIndex.Find<Ucommerce.Search.Models.Product>()
                .Where(
                    x =>
                        x.LongDescription == Match.FullText(searchTerm) ||
                        x.Name == Match.Wildcard($"*{searchTerm}*") ||
                        x.Sku == Match.Literal(searchTerm) ||
                        x.Name == Match.Literal(searchTerm) ||
                        x.DisplayName == Match.Wildcard($"*{searchTerm}*")).ToList();

            model.ProductViewModels = MapProducts(result.Results);

            return View(model);
        }

        private string GetSearchTerm()
        {
            return Request.QueryString["Query"];
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
                Url = _urlService.GetUrl(_catalogContext.CurrentCatalog, product)
            }).ToList();
        }
    }
}