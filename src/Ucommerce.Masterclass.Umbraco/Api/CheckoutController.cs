using System.Linq;
using System.Web.Http;
using Ucommerce.Api;
using Ucommerce.Masterclass.Umbraco.Models;
using Umbraco.Web.WebApi;

namespace Ucommerce.Masterclass.Umbraco.Api
{
    public class CheckoutController : UmbracoApiController
    {
        private readonly ITransactionLibrary _transactionLibrary;

        public CheckoutController(ITransactionLibrary transactionLibrary)
        {
            _transactionLibrary = transactionLibrary;
        }

        [System.Web.Mvc.HttpGet]
        public IHttpActionResult GetPaymentPageUrl()
        {
            var basket = _transactionLibrary.GetBasket();
            return Json(_transactionLibrary.GetPaymentPageUrl(basket.Payments.First()));
        }

        private PurchaseOrderViewModel MapPurchaseOrder(Ucommerce.EntitiesV2.PurchaseOrder basket)
        {
            var model = new PurchaseOrderViewModel();

            model.OrderLines = basket.OrderLines.Select(orderLine => new OrderlineViewModel()
            {
                Quantity = orderLine.Quantity,
                ProductName = orderLine.ProductName,
                Total = new Money(orderLine.Total.GetValueOrDefault(), basket.BillingCurrency.ISOCode).ToString(),
                OrderLineId = orderLine.OrderLineId
            }).ToList();
            
            return model;
        }
    }
}