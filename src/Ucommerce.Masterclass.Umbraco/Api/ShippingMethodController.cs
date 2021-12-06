using System.Web.Http;
using Ucommerce.Api;
using Ucommerce.Masterclass.Umbraco.Models;
using Umbraco.Web.WebApi;

namespace Ucommerce.Masterclass.Umbraco.Api
{
    public class ShippingMethodController : UmbracoApiController
    {
        private readonly ITransactionLibrary _transactionLibrary;

        public ShippingMethodController(ITransactionLibrary transactionLibrary)
        {
            _transactionLibrary = transactionLibrary;
        }

        [HttpPost]
        public IHttpActionResult Update(CheckoutViewModel checkoutViewModel)
        {
            if (checkoutViewModel?.ShippingViewModel?.SelectedShippingMethod?.ShippingMethodId != null)
            {
                return Ok();
            }

            return BadRequest("Missing Shipping method");
        }
    }
}