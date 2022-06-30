using System.Threading;
using System.Web.Http;
using Ucommerce.Masterclass.Umbraco.Models;
using Umbraco.Web.WebApi;

namespace Ucommerce.Masterclass.Umbraco.Api
{
    public class MasterClassPaymentMethodController : UmbracoApiController
    {

        public MasterClassPaymentMethodController()
        {
        }

        [HttpPost]
        public IHttpActionResult Update(CheckoutViewModel checkoutViewModel, CancellationToken ct)
        {
            return Ok();
        }
    }
}