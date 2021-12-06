using System.Web.Http;
using Ucommerce.Api;
using Ucommerce.Masterclass.Umbraco.Models;
using Umbraco.Web.WebApi;

namespace Ucommerce.Masterclass.Umbraco.Api
{
    public class PaymentMethodController : UmbracoApiController
    {
        private readonly ITransactionLibrary _transactionLibrary;

        public PaymentMethodController(ITransactionLibrary transactionLibrary)
        {
            _transactionLibrary = transactionLibrary;
        }

        [HttpPost]
        public IHttpActionResult Update(CheckoutViewModel checkoutViewModel)
        {
            if (checkoutViewModel?.PaymentViewModel?.SelectedPaymentMethod?.PaymentMethodId != null)
            {
                return Ok();
            }

            return BadRequest("Missing Payment method"); ;
        }
    }
}