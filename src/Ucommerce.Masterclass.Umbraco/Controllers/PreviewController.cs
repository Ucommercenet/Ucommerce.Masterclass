using System.Linq;
using System.Web.Mvc;
using Ucommerce.Api;
using Ucommerce.Infrastructure;
using Ucommerce.Masterclass.Umbraco.Models;
using Umbraco.Web.Mvc;

namespace Ucommerce.Masterclass.Umbraco.Controllers
{
    public class CheckoutPreviewController : RenderMvcController
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


        [HttpPost]
        public ActionResult Index(int selectedPaymentMethod)
        {
            return Redirect("/preview");
        }
    }
}