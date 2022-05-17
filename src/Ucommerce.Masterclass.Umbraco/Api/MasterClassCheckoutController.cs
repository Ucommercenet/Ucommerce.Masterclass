using System.Threading;
using System.Threading.Tasks;
using System.Web;
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

        public MasterClassCheckoutController(ITransactionClient transactionClient, IBasketIdResolver basketIdResolver, IPriceGroupIdResolver priceGroupIdResolver, ICultureCodeResolver cultureCodeResolver)
        {
            _transactionClient = transactionClient;
            _basketIdResolver = basketIdResolver;
            _priceGroupIdResolver = priceGroupIdResolver;
            _cultureCodeResolver = cultureCodeResolver;
        }

        [System.Web.Mvc.HttpGet]
        public async Task<IHttpActionResult> GetPaymentPageUrl(CancellationToken ct)
        {
            var paymentMethodId = HttpContext.Current.Request.Cookies["SelectedPaymentMethodId"].Value;
            var paymentUrl = await _transactionClient.CreatePayment(_basketIdResolver.GetBasketId(System.Web.HttpContext.Current.Request), _cultureCodeResolver.GetCultureCode(), paymentMethodId, _priceGroupIdResolver.PriceGroupId(), ct);

            if (paymentUrl.PaymentUrl == null) return BadRequest("Missing Payment");

            return Redirect(paymentUrl.PaymentUrl);
        }
    }
}