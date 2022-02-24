using System.Threading;
using System.Threading.Tasks;
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

        public MasterClassCheckoutController(ITransactionClient transactionClient, IBasketIdResolver basketIdResolver, IPriceGroupIdResolver priceGroupIdResolver, ICultureCodeResolver cultureCodeResolver)
        {
            _transactionClient = transactionClient;
            _basketIdResolver = basketIdResolver;
            _priceGroupIdResolver = priceGroupIdResolver;
            _cultureCodeResolver = cultureCodeResolver;
        }

        [System.Web.Mvc.HttpGet]
        public  async Task<IHttpActionResult> GetPaymentPageUrl(CancellationToken ct)
        {
            var paymentMethodId = ""; //TODO:
            var paymentUrl = await _transactionClient.Checkout(_basketIdResolver.GetBasketId(this.Request), _cultureCodeResolver.GetCultureCode(), paymentMethodId, _priceGroupIdResolver.PriceGroupId(), ct);
            
            if (string.IsNullOrWhiteSpace(paymentUrl)) return BadRequest("Missing Payment");

            return Json(paymentUrl);
        }
    }
}