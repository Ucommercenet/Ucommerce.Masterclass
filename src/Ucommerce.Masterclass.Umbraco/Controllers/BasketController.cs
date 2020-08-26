using System;
using System.Linq;
using System.Web.Mvc;
using Ucommerce.Api;
using Ucommerce.EntitiesV2;
using Ucommerce.Infrastructure;
using Ucommerce.Marketing;
using Ucommerce.Marketing.Awards.AwardResolvers;
using Ucommerce.Marketing.TargetingContextAggregators;
using Ucommerce.Marketing.Targets.TargetResolvers;
using Ucommerce.Masterclass.Umbraco.Models;
using Umbraco.Web.Mvc;

namespace Ucommerce.Masterclass.Umbraco.Controllers
{
    public class BasketController : RenderMvcController
    {
        public ITransactionLibrary TransactionLibrary => ObjectFactory.Instance.Resolve<ITransactionLibrary>();

        [System.Web.Mvc.HttpGet]
        public ActionResult Index()
        {
            var basket = TransactionLibrary.GetBasket(false);

            return View(new PurchaseOrderViewModel()
            {
                TaxTotal = new Money(basket.TaxTotal.GetValueOrDefault(), basket.BillingCurrency.ISOCode).ToString(),
                OrderTotal = new Money(basket.OrderTotal.GetValueOrDefault(), basket.BillingCurrency.ISOCode).ToString(),
                ShippingTotal = new Money(basket.ShippingTotal.GetValueOrDefault(), basket.BillingCurrency.ISOCode).ToString(),
                SubTotal = new Money(basket.SubTotal.GetValueOrDefault(), basket.BillingCurrency.ISOCode).ToString(),
                OrderLines = basket.OrderLines.Select(x => new OrderlineViewModel()
                {
                    Quantity = x.Quantity,
                    ProductName = x.ProductName,
                    OrderLineId = x.OrderLineId,
                    Sku = x.Sku,
                    Total = new Money(x.Total.GetValueOrDefault(), basket.BillingCurrency.ISOCode).ToString(),
                    UnitPrice = new Money(x.Price, basket.BillingCurrency.ISOCode).ToString(),
                    Tax = new Money(x.VAT, basket.BillingCurrency.ISOCode).ToString(),
                }).ToList()
            });
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult Index(int quantity, int orderlineId)
        {
            TransactionLibrary.UpdateLineItemByOrderLineId(orderlineId, quantity);
            TransactionLibrary.ExecuteBasketPipeline();
            
            return Index();
        }
    }
}