using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Ucommerce.Api;
using Ucommerce.Masterclass.Umbraco.Exceptions;
using Ucommerce.Masterclass.Umbraco.Headless;
using Ucommerce.Masterclass.Umbraco.Models;
using Ucommerce.Masterclass.Umbraco.Resolvers;
using Umbraco.Web.WebApi;

namespace Ucommerce.Masterclass.Umbraco.Api
{
    public class MasterClassShippingMethodController : UmbracoApiController
    {
        private readonly ITransactionClient _transactionClient;
        private readonly IBasketIdResolver _basketResolver;
        private readonly IPriceGroupIdResolver _priceGroupIdResolver;

        public MasterClassShippingMethodController(ITransactionClient transactionClient, IBasketIdResolver basketResolver, IPriceGroupIdResolver priceGroupIdResolver)
        {
            _transactionClient = transactionClient;
            _basketResolver = basketResolver;
            _priceGroupIdResolver = priceGroupIdResolver;
        }

        [HttpPost]
        public async Task<IHttpActionResult> Update(CheckoutViewModel checkoutViewModel, CancellationToken ct)
        {
            var basketId = _basketResolver.GetBasketId(this.Request);
            if (string.IsNullOrEmpty(basketId))
                throw new MissingBasketIdException("Couldn't read basket id from cookies.");

            if (checkoutViewModel?.ShippingViewModel?.SelectedShippingMethod?.ShippingMethodId != null)
            {
                await _transactionClient.CreateShipment(basketId, checkoutViewModel.ShippingViewModel.SelectedShippingMethod.ShippingMethodId.ToString(), _priceGroupIdResolver.PriceGroupId(), ct);

                return Ok();
            }

            return BadRequest("Missing Shipping method");
        }

        private string GetBasketCookieValue()
        {
            return this.Request.Headers.GetCookies().Select(c => c["basketId"])
                .FirstOrDefault()?.Value ?? "";
        }
    }
}