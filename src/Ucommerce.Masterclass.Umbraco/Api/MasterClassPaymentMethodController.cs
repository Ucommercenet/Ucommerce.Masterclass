using System;
using System.Web;
using System.Web.Http;
using MC_Headless.Exceptions;
using Umbraco.Web.WebApi;

namespace MC_Headless.Api
{
    public class MasterClassPaymentMethodController : UmbracoApiController
    {
        public MasterClassPaymentMethodController()
        {
        }

        [HttpPost]
        public IHttpActionResult Update()
        {
            var basketId = "";

            if (string.IsNullOrWhiteSpace(basketId))
                throw new MissingBasketIdException("Couldn't read basket id from cookies.");

            var selectedPaymentMethodCookie = new HttpCookie("SelectedPaymentMethodId", Guid.NewGuid().ToString());
            HttpContext.Current.Response.Cookies.Add(selectedPaymentMethodCookie);

            return Ok();
        }
    }
}