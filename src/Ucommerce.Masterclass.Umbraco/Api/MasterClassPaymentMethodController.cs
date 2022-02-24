using System;
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
    public class MasterClassPaymentMethodController : UmbracoApiController
    {
        private readonly ITransactionClient _transactionClient;
        private readonly IBasketIdResolver _basketResolver;
        private readonly IPriceGroupIdResolver _priceGroupIdResolver;
        private readonly ICultureCodeResolver _cultureCodeResolver;

        public MasterClassPaymentMethodController(ITransactionClient transactionClient, IBasketIdResolver basketResolver, IPriceGroupIdResolver priceGroupIdResolver, ICultureCodeResolver cultureCodeResolver)
        {
            _transactionClient = transactionClient;
            _basketResolver = basketResolver;
            _priceGroupIdResolver = priceGroupIdResolver;
            _cultureCodeResolver = cultureCodeResolver;
        }

        [HttpPost]
        public async Task<IHttpActionResult> Update(CheckoutViewModel checkoutViewModel, CancellationToken ct)
        {
            var basketId = _basketResolver.GetBasketId(this.Request);
            if (string.IsNullOrWhiteSpace(basketId))
                throw new MissingBasketIdException("Couldn't read basket id from cookies.");

            if (checkoutViewModel?.PaymentViewModel?.SelectedPaymentMethod?.PaymentMethodId != null)
            {
                await _transactionClient.CreatePayment(basketId, _cultureCodeResolver.GetCultureCode(), checkoutViewModel.PaymentViewModel.SelectedPaymentMethod.PaymentMethodId.ToString(), _priceGroupIdResolver.PriceGroupId(), ct);

                return Ok();
            }

            return BadRequest("Missing Payment method");
        }
    }
}