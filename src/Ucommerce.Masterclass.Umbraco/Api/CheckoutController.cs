using System.Linq;
using System.Web.Http;
using Ucommerce.Api;
using Ucommerce.EntitiesV2;
using Umbraco.Web.WebApi;

namespace Ucommerce.Masterclass.Umbraco.Api
{
    public class CheckoutController : UmbracoApiController
    {
        public CheckoutController()
        {
        }

        [System.Web.Mvc.HttpGet]
        public IHttpActionResult GetPaymentPageUrl()
        {
            Ucommerce.EntitiesV2.PurchaseOrder basket = GetBasket(); 
            
            if (!basket.Payments.Any()) return BadRequest("Missing Payment");

            return Json("");
        }

        private PurchaseOrder GetBasket()
        {
            throw new System.NotImplementedException();
        }
    }
}