using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using MC_Headless.Headless;
using MC_Headless.Resolvers;
using Umbraco.Web.WebApi;

namespace MC_Headless.Api
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

            return Redirect(paymentUrl.PaymentUrl);
        }
    }
}