using System.Linq;
using System.Web.Mvc;
using Ucommerce.Api;
using Ucommerce.EntitiesV2;
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
                BillingAddress = MapAddress(TransactionLibrary.GetBillingInformation()),
                ShippingAddress = MapAddress(TransactionLibrary.GetShippingInformation()),
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

        private AddressViewModel MapAddress(OrderAddress address)
        {
            var addressModel = new AddressViewModel();
            
            addressModel.FirstName = address.FirstName;
            addressModel.EmailAddress = address.EmailAddress;
            addressModel.LastName = address.LastName;
            addressModel.PhoneNumber = address.PhoneNumber;
            addressModel.MobilePhoneNumber = address.MobilePhoneNumber;
            addressModel.Line1 = address.Line1;
            addressModel.Line2 = address.Line2;
            addressModel.PostalCode = address.PostalCode;
            addressModel.City = address.City;
            addressModel.State = address.State;
            addressModel.Attention = address.Attention;
            addressModel.CompanyName = address.CompanyName;
            addressModel.CountryName = address.Country.Name;

            return addressModel;
        }


        [HttpPost]
        public ActionResult Index(int complete)
        {
            // TransactionLibrary.RequestPayments();
            return Redirect("/complete");
        }
    }
}