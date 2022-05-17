using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using MC_Headless.Exceptions;
using Ucommerce.Masterclass.Umbraco.Models;
using MC_Headless.Resolvers;
using Umbraco.Web.WebApi;

namespace MC_Headless.Api
{
    public class MasterClassPaymentMethodController : UmbracoApiController
    {
        private readonly IBasketIdResolver _basketResolver;

        public MasterClassPaymentMethodController(IBasketIdResolver basketResolver)
        {
            _basketResolver = basketResolver;
        }

        [HttpPost]
        public async Task<IHttpActionResult> Update(CheckoutViewModel checkoutViewModel, CancellationToken ct)
        {
            var basketId = _basketResolver.GetBasketId(System.Web.HttpContext.Current.Request);
            
            if (string.IsNullOrWhiteSpace(basketId))
                throw new MissingBasketIdException("Couldn't read basket id from cookies.");

            var paymentMethodId = checkoutViewModel?.PaymentViewModel?.SelectedPaymentMethod?.PaymentMethodId;

            if (paymentMethodId == null) return BadRequest("Missing Payment method");
            var selectedPaymentMethodCookie = new HttpCookie("SelectedPaymentMethodId", paymentMethodId.ToString());
            HttpContext.Current.Response.Cookies.Add(selectedPaymentMethodCookie);

            return Ok();

        }
    }
}