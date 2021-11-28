using System;
using System.Linq;
using System.Web.Mvc;
using Ucommerce.Api;
using Ucommerce.EntitiesV2;
using Ucommerce.Infrastructure;
using Ucommerce.Masterclass.Umbraco.Models;
using Umbraco.Web.Mvc;

namespace Ucommerce.Masterclass.Umbraco.Controllers
{
    public class OrderConfirmationController : RenderMvcController
    {
        public ITransactionLibrary TransactionLibrary => ObjectFactory.Instance.Resolve<ITransactionLibrary>();

        [System.Web.Mvc.HttpGet]
        public ActionResult Index()
        {
            var purchaseOrder = TransactionLibrary.GetPurchaseOrder(Guid.Parse(Request.QueryString["OrderGuid"]));

            return View(new PurchaseOrderViewModel()
            {
                TaxTotal = new Money(purchaseOrder.TaxTotal.GetValueOrDefault(), purchaseOrder.BillingCurrency.ISOCode).ToString(),
                BillingAddress = MapAddress(purchaseOrder.BillingAddress),
                ShippingAddress = MapAddress(purchaseOrder.Shipments.First().ShipmentAddress),
                OrderTotal = new Money(purchaseOrder.OrderTotal.GetValueOrDefault(), purchaseOrder.BillingCurrency.ISOCode).ToString(),
                ShippingTotal = new Money(purchaseOrder.ShippingTotal.GetValueOrDefault(), purchaseOrder.BillingCurrency.ISOCode).ToString(),
                SubTotal = new Money(purchaseOrder.SubTotal.GetValueOrDefault(), purchaseOrder.BillingCurrency.ISOCode).ToString(),
                OrderLines = purchaseOrder.OrderLines.Select(x => new OrderlineViewModel()
                {
                    Quantity = x.Quantity,
                    ProductName = x.ProductName,
                    OrderLineId = x.OrderLineId,
                    Sku = x.Sku,
                    Total = new Money(x.Total.GetValueOrDefault(), purchaseOrder.BillingCurrency.ISOCode).ToString(),
                    UnitPrice = new Money(x.Price, purchaseOrder.BillingCurrency.ISOCode).ToString(),
                    Tax = new Money(x.VAT, purchaseOrder.BillingCurrency.ISOCode).ToString(),
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
            addressModel.Country = new CountryViewModel();

            return addressModel;
        }
    }
}