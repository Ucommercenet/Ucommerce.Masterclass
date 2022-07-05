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

        public ProductSearchResultController(ICatalogLibrary catalogLibrary, IUrlService urlService,
            ICatalogContext catalogContext)
        {
            _catalogLibrary = catalogLibrary;
            _urlService = urlService;
            _catalogContext = catalogContext;
        }

        public ActionResult Index()
        {
            var searchTerm = GetSearchTerm();

            var result = new List<Product>();

            var model = new ProductListViewModel
            {
                ProductViewModels = MapProducts(result)
            };

            return View(model);
        }

        private string GetSearchTerm()
        {
            return Request.QueryString["Query"];
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
                Prices = prices.Items.Where(price => price.ProductGuid == product.Guid && price.PriceGroupGuid == _catalogContext.CurrentPriceGroup.Guid).ToList(),
                ShortDescription = product.ShortDescription,
                Url = _urlService.GetUrl(_catalogContext.CurrentCatalog, product)
            }).ToList();
        }
    }
}