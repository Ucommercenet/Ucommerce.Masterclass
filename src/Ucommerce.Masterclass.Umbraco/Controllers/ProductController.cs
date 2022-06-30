using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Ucommerce.Masterclass.Umbraco.Exceptions;
using Ucommerce.Masterclass.Umbraco.Headless;
using Ucommerce.Masterclass.Umbraco.Resolvers;
using Ucommerce.Api;
using Ucommerce.Masterclass.Umbraco.Models;
using Ucommerce.Search;
using Ucommerce.Search.Models;
using Umbraco.Web.Mvc;

namespace Ucommerce.Masterclass.Umbraco.Controllers
{
    public class ProductController : RenderMvcController
    {
        private readonly ICatalogContext _catalogContext;
        private readonly ICatalogLibrary _catalogLibrary;
        private readonly ITransactionClient _transactionClient;
        private readonly IBasketIdResolver _basketIdResolver;
        private readonly IPriceGroupIdResolver _priceGroupIdResolver;
        private readonly ICultureCodeResolver _cultureCodeResolver;
        private readonly IProductCatalogIdResolver _productCatalogIdResolver;

        public ProductController(ICatalogContext catalogContext, ICatalogLibrary catalogLibrary,
            ITransactionClient transactionClient, IBasketIdResolver basketIdResolver,
            IPriceGroupIdResolver priceGroupIdResolver, ICultureCodeResolver cultureCodeResolver,
            IProductCatalogIdResolver productCatalogIdResolver)
        {
            _catalogContext = catalogContext;
            _catalogLibrary = catalogLibrary;
            _transactionClient = transactionClient;
            _basketIdResolver = basketIdResolver;
            _priceGroupIdResolver = priceGroupIdResolver;
            _cultureCodeResolver = cultureCodeResolver;
            _productCatalogIdResolver = productCatalogIdResolver;
        }

        [System.Web.Mvc.HttpGet]
        public ActionResult Index()
        {
            //TODO: Task 01 - Fetch and present the current Product
            var currentProduct = _catalogContext.CurrentProduct;

            var productModel = new ProductViewModel
            {
                PrimaryImageUrl = currentProduct.PrimaryImageUrl,
                Name = currentProduct.DisplayName,
                Sku = currentProduct.Sku,
                Prices = _catalogLibrary.CalculatePrices(new List<Guid> { currentProduct.Guid }).Items,
                //TODO: Task 02 - Ensure your code accounts for product Families
                Variants = MapVariants(_catalogLibrary.GetVariants(currentProduct))
            };

            return View(productModel);
        }

        [System.Web.Mvc.HttpPost]
        public async Task<ActionResult> Index(UpdateOrderLineRequest updateOrderLineRequest,
            CancellationToken ct)
        {
            var basketId = _basketIdResolver.GetBasketId(System.Web.HttpContext.Current.Request);
            var cultureCode = _cultureCodeResolver.GetCultureCode();
            var currency = _catalogContext.CurrentPriceGroup.CurrencyISOCode;
            
            if (string.IsNullOrWhiteSpace(basketId))
            {
                var basket = await _transactionClient.CreateBasket(currency, cultureCode, ct);
                basketId = basket.BasketId.ToString();
                System.Web.HttpContext.Current.Response.AppendCookie(new HttpCookie("basketId", basketId));
            }


            var priceGroupId = _priceGroupIdResolver.PriceGroupId();
            var productCatalogId = _productCatalogIdResolver.ProductCatalogId();

            await _transactionClient.UpdateOrderLineQuantity(cultureCode, updateOrderLineRequest.NewQuantity,
                updateOrderLineRequest.Sku, updateOrderLineRequest.VariantSku, priceGroupId, productCatalogId, basketId,
                ct);
            return Index();
        }

        private IList<ProductViewModel> MapVariants(ResultSet<Product> variants)
        {
            return variants.Select(x =>
                new ProductViewModel
                {
                    Name = x.DisplayName,
                    VariantSku = x.VariantSku
                }).ToList();
        }
    }
}