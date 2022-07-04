using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Ucommerce.Masterclass.Umbraco.Headless;
using Ucommerce.Masterclass.Umbraco.Resolvers;
using Umbraco.Web.WebApi;

namespace Ucommerce.Masterclass.Umbraco.Api
{
    public class MasterClassCheckoutController : UmbracoApiController
    {
        private readonly ITransactionClient _transactionClient;
        private readonly IBasketIdResolver _basketIdResolver;
        private readonly IPriceGroupIdResolver _priceGroupIdResolver;
        private readonly ICultureCodeResolver _cultureCodeResolver;
        private readonly IPaymentMethodIdResolver _paymentMethodIdResolver;

        public MasterClassCheckoutController(ITransactionClient transactionClient, IBasketIdResolver basketIdResolver, IPriceGroupIdResolver priceGroupIdResolver, ICultureCodeResolver cultureCodeResolver, IPaymentMethodIdResolver paymentMethodIdResolver)
        {
            _transactionClient = transactionClient;
            _basketIdResolver = basketIdResolver;
            _priceGroupIdResolver = priceGroupIdResolver;
            _cultureCodeResolver = cultureCodeResolver;
            _paymentMethodIdResolver = paymentMethodIdResolver;
        }

        [System.Web.Mvc.HttpGet]
        public async Task<IHttpActionResult> GetPaymentPageUrl(CancellationToken ct)
        {
            var request = System.Web.HttpContext.Current.Request;
            var paymentMethodId = _paymentMethodIdResolver.GetSelectedPaymentMethodId(request);
            var paymentUrl = await _transactionClient.CreatePayment(_basketIdResolver.GetBasketId(request), _cultureCodeResolver.GetCultureCode(), paymentMethodId, _priceGroupIdResolver.PriceGroupId(), ct);

            if (paymentUrl == null || string.IsNullOrWhiteSpace(paymentUrl.PaymentUrl)) return BadRequest("Missing Payment");

            var currentBasketIdCookie = HttpContext.Current.Request.Cookies["basketId"];
            HttpContext.Current.Response.Cookies.Remove("basketId");
            currentBasketIdCookie.Expires = DateTime.Now.AddDays(-10);
            currentBasketIdCookie.Value = null;
            HttpContext.Current.Response.SetCookie(currentBasketIdCookie);

            return Ok(paymentUrl.PaymentUrl);
        }
    }
}