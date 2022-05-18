using System.Threading;
using System.Web;
using System.Web.Http;
using Ucommerce.Masterclass.Umbraco.Models;
using Ucommerce.Masterclass.Umbraco.Exceptions;
using Ucommerce.Masterclass.Umbraco.Resolvers;
using Umbraco.Web.WebApi;

namespace Ucommerce.Masterclass.Umbraco.Api
{
    public class MasterClassPaymentMethodController : UmbracoApiController
    {
        private readonly IBasketIdResolver _basketResolver;

        public MasterClassPaymentMethodController(IBasketIdResolver basketResolver)
        {
            _basketResolver = basketResolver;
        }

        [HttpPost]
        public IHttpActionResult Update(CheckoutViewModel checkoutViewModel, CancellationToken ct)
        {
            var basketId = _basketResolver.GetBasketId(System.Web.HttpContext.Current.Request);
            
            if (string.IsNullOrWhiteSpace(basketId))
                throw new MissingBasketIdException("Couldn't read basket id from cookies.");

            var selectedPaymentMethodId = checkoutViewModel?.PaymentViewModel?.SelectedPaymentMethod?.PaymentMethodId;

            if (selectedPaymentMethodId == null) return BadRequest("Missing Payment method");
            var selectedPaymentMethodCookie = new HttpCookie("SelectedPaymentMethodId", selectedPaymentMethodId.ToString());
            HttpContext.Current.Response.Cookies.Add(selectedPaymentMethodCookie);

            return Ok();

        }
    }
}