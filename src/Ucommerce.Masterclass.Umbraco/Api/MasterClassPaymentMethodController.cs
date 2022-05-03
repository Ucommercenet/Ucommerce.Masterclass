using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using MC_Headless.Exceptions;
using MC_Headless.Headless;
using Ucommerce.Masterclass.Umbraco.Models;
using MC_Headless.Resolvers;
using Umbraco.Web.WebApi;

namespace MC_Headless.Api
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

            var paymentMethodId = checkoutViewModel?.PaymentViewModel?.SelectedPaymentMethod?.PaymentMethodId;

            var cultureCode = _cultureCodeResolver.GetCultureCode();

            if (paymentMethodId == null) return BadRequest("Missing Payment method");
            var selectedPaymentMethodCookie = new HttpCookie("SelectedPaymentMethodId", paymentMethodId.ToString());
            HttpContext.Current.Response.Cookies.Add(selectedPaymentMethodCookie);

            return Ok();

        }
    }
}