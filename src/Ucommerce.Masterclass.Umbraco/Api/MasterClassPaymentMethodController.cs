using System;
using System.Threading;
using System.Web;
using System.Web.Http;
using MC_Headless.Exceptions;
using Ucommerce.Masterclass.Umbraco.Models;
using Umbraco.Web.WebApi;

namespace MC_Headless.Api
{
    public class MasterClassPaymentMethodController : UmbracoApiController
    {
        public MasterClassPaymentMethodController()
        {
        }

        [HttpPost]
        public IHttpActionResult Update(CheckoutViewModel checkoutViewModel, CancellationToken ct)
        {
            var basketId = "";
            
            if (string.IsNullOrWhiteSpace(basketId))
                throw new MissingBasketIdException("Couldn't read basket id from cookies.");

            var paymentMethodId = Guid.NewGuid();

            if (paymentMethodId == null) return BadRequest("Missing Payment method");
            var selectedPaymentMethodCookie = new HttpCookie("SelectedPaymentMethodId", paymentMethodId.ToString());
            HttpContext.Current.Response.Cookies.Add(selectedPaymentMethodCookie);

            return Ok();

        }
    }
}