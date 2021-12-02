using System.Linq;
using System.Web.Http;
using Ucommerce.Api;
using Ucommerce.EntitiesV2;
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
                _transactionLibrary.CreateShipment(checkoutViewModel.ShippingViewModel.SelectedShippingMethod.ShippingMethodId, Constants.DefaultShipmentAddressName, true);
                
                return Ok();
            }

            return InternalServerError();
        }
    }
}