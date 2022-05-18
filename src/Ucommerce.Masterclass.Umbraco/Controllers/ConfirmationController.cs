using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using Ucommerce.Masterclass.Umbraco.Headless;
using Ucommerce.Headless.Domain;
using Ucommerce.Masterclass.Umbraco.Models;
using Umbraco.Core;
using Umbraco.Web.Mvc;

namespace Ucommerce.Masterclass.Umbraco.Controllers
{
    public class ConfirmationController : RenderMvcController
    {
        private readonly ITransactionClient _transactionClient;

        public ConfirmationController(ITransactionClient transactionClient)
        {
            _transactionClient = transactionClient;
        }

        [System.Web.Mvc.HttpGet]
        public async Task<ActionResult> Index(CancellationToken ct)
        {
            var orderGuidParameterFromQueryString = System.Web.HttpContext.Current.Request.QueryString["OrderGuid"];

            if (orderGuidParameterFromQueryString.IsNullOrWhiteSpace())
                return View(new PurchaseOrderViewModel());

            var order = await _transactionClient.GetOrder(orderGuidParameterFromQueryString, ct);

            var billingInformation = order.BillingAddress;
            var shippingInformation = order.Shipments.FirstOrDefault().ShipmentAddress;
            var selectedCountry = billingInformation.Country;

            var purchaseOrderViewModel = MapPurchaseOrder(order);

            purchaseOrderViewModel.BillingAddress.FirstName = billingInformation.FirstName;
            purchaseOrderViewModel.BillingAddress.LastName = billingInformation.LastName;
            purchaseOrderViewModel.BillingAddress.Line1 = billingInformation.Line1;
            purchaseOrderViewModel.BillingAddress.City = billingInformation.City;
            purchaseOrderViewModel.BillingAddress.PostalCode = billingInformation.PostalCode;
            purchaseOrderViewModel.BillingAddress.Country = new CountryViewModel
                { Name = selectedCountry.Name, CountryId = selectedCountry.Id };
            purchaseOrderViewModel.BillingAddress.EmailAddress = billingInformation.EmailAddress;
            purchaseOrderViewModel.BillingAddress.PhoneNumber = billingInformation.MobilePhoneNumber;

            var selectedShippingCountry = shippingInformation.Country;

            purchaseOrderViewModel.ShippingAddress.FirstName = shippingInformation.FirstName;
            purchaseOrderViewModel.ShippingAddress.LastName = shippingInformation.LastName;
            purchaseOrderViewModel.ShippingAddress.Line1 = shippingInformation.Line1;
            purchaseOrderViewModel.ShippingAddress.City = shippingInformation.City;
            purchaseOrderViewModel.ShippingAddress.PostalCode = shippingInformation.PostalCode;
            purchaseOrderViewModel.ShippingAddress.Country = new CountryViewModel
                { Name = selectedShippingCountry.Name, CountryId = selectedShippingCountry.Id };
            purchaseOrderViewModel.ShippingAddress.EmailAddress = shippingInformation.EmailAddress;
            purchaseOrderViewModel.ShippingAddress.PhoneNumber = shippingInformation.MobilePhoneNumber;

            purchaseOrderViewModel.DiscountTotal =
                new Money(order.Discount.GetValueOrDefault(), order.BillingCurrency.IsoCode)
                    .ToString();
            purchaseOrderViewModel.SubTotal =
                new Money(order.SubTotal.GetValueOrDefault(), order.BillingCurrency.IsoCode)
                    .ToString();
            purchaseOrderViewModel.TaxTotal =
                new Money(order.Vat.GetValueOrDefault(), order.BillingCurrency.IsoCode)
                    .ToString();
            purchaseOrderViewModel.ShippingTotal =
                new Money(order.ShippingTotal.GetValueOrDefault(), order.BillingCurrency.IsoCode).ToString();
            purchaseOrderViewModel.PaymentTotal =
                new Money(order.PaymentTotal.GetValueOrDefault(), order.BillingCurrency.IsoCode).ToString();
            purchaseOrderViewModel.OrderTotal =
                new Money(order.OrderTotal.GetValueOrDefault(), order.BillingCurrency.IsoCode).ToString();

            return View(purchaseOrderViewModel);
        }

        private PurchaseOrderViewModel MapPurchaseOrder(GetOrderOutput order)
        {
            return new PurchaseOrderViewModel
            {
                OrderLines = order.OrderLines.Select(orderLine => new OrderlineViewModel
                {
                    Quantity = orderLine.Quantity,
                    ProductName = orderLine.ProductName,
                    Total = new Money(orderLine.Total.GetValueOrDefault(), order.BillingCurrency.IsoCode).ToString(),
                    TotalWithDiscount =
                        new Money(orderLine.Total.GetValueOrDefault() - orderLine.Discount,
                            order.BillingCurrency.IsoCode).ToString(),
                    Discount = orderLine.Discount,
                }).ToList()
            };
        }
    }
}