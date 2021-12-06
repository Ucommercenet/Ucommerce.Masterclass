using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Newtonsoft.Json;
using Ucommerce.Api;
using Ucommerce.EntitiesV2;
using Ucommerce.Masterclass.Umbraco.Models;
using Umbraco.Web.WebApi;

namespace Ucommerce.Masterclass.Umbraco.Api
{
    public class BasketController : UmbracoApiController
    {
        private readonly ITransactionLibrary _transactionLibrary;

        public BasketController(ITransactionLibrary transactionLibrary)
        {
            _transactionLibrary = transactionLibrary;
        }

        public CheckoutViewModel Get()
        {
            return GetCheckoutModel();
        }

        [System.Web.Mvc.HttpPost]
        public IHttpActionResult UpdateOrderLine(UpdateOrderLineRequest updateOrderLineRequest)
        {
            _transactionLibrary.UpdateLineItemByOrderLineId(updateOrderLineRequest.OrderLineId, updateOrderLineRequest.NewQuantity);
            _transactionLibrary.ExecuteBasketPipeline();

            return Ok();
        }

        private CheckoutViewModel GetCheckoutModel()
        {
            var checkoutModel = new CheckoutViewModel();

            //TODO: Task 01 -> Fetch basket and map it to view if customer has a basket
            if (!_transactionLibrary.HasBasket())
                return null;

            var basket = _transactionLibrary.GetBasket();
            checkoutModel.PurchaseOrderViewModel = MapPurchaseOrder(basket);
            MapTotals(checkoutModel, basket);

            //TODO: Task 02 -> Present the address details
            var countries = _transactionLibrary.GetCountries();
            var billingInformation = _transactionLibrary.GetBillingInformation();
            var shippingInformation = _transactionLibrary.GetShippingInformation();

            MapBillingAddress(billingInformation, countries, checkoutModel);
            MapShippingAddress(shippingInformation, countries, checkoutModel);
            checkoutModel.Countries = countries.Select(x => new CountryViewModel() { Name = x.Name, CountryId = x.CountryId }).ToList();

            checkoutModel.DifferentShippingAddress = IsAddressesDifferent(checkoutModel.ShippingAddressViewModel, checkoutModel.AddressViewModel);

            //TODO: Task 03 -> Present the available shipping methods
            var selectedCountry = billingInformation.Country ?? countries.First();
            
            var availableShippingMethods = _transactionLibrary.GetShippingMethods(selectedCountry);
            checkoutModel.ShippingViewModel.AvailableShippingMethods = availableShippingMethods.Select(x => new ShippingMethodViewModel() { Name = x.Name, ShippingMethodId = x.ShippingMethodId }).ToList();

            ShippingMethod selectedShippingMethod = basket.Shipments.FirstOrDefault()?.ShippingMethod;
            if (selectedShippingMethod != null)
            {
                checkoutModel.ShippingViewModel.SelectedShippingMethod = new ShippingMethodViewModel()
                { Name = selectedShippingMethod.Name, ShippingMethodId = selectedShippingMethod.ShippingMethodId };
            }

            //TODO: Task 04 -> Present the available payment methods
            var availablePaymentMethods = _transactionLibrary.GetPaymentMethods(selectedCountry);
            checkoutModel.PaymentViewModel.AvailablePaymentMethods = availablePaymentMethods.Select(x => new PaymentMethodViewModel() { Name = x.Name, PaymentMethodId = x.PaymentMethodId }).ToList();

            PaymentMethod selectedPaymentMethod = basket.Payments.FirstOrDefault()?.PaymentMethod;
            if (selectedPaymentMethod != null)
            {
                checkoutModel.PaymentViewModel.SelectedPaymentMethod = new PaymentMethodViewModel()
                { Name = selectedPaymentMethod.Name, PaymentMethodId = selectedPaymentMethod.PaymentMethodId };
            }

            return checkoutModel;
        }
        private PurchaseOrderViewModel MapPurchaseOrder(Ucommerce.EntitiesV2.PurchaseOrder basket)
        {
            var model = new PurchaseOrderViewModel();

            model.OrderLines = basket.OrderLines.Select(orderLine => new OrderlineViewModel()
            {
                Quantity = orderLine.Quantity,
                ProductName = orderLine.ProductName,
                Discount = orderLine.Discount,
                Total = new Money(orderLine.Total.GetValueOrDefault(), basket.BillingCurrency.ISOCode).ToString(),
                TotalWithDiscount =
                    new Money(orderLine.Price - orderLine.Discount, basket.BillingCurrency.ISOCode).ToString(),
                OrderLineId = orderLine.OrderLineId
            }).ToList();

            return model;
        }

        private void MapTotals(CheckoutViewModel checkoutModel, PurchaseOrder basket)
        {
            checkoutModel.Discount =
                new Money(basket.Discount.GetValueOrDefault(), basket.BillingCurrency.ISOCode)
                    .ToString();
            checkoutModel.SubTotal =
                new Money(basket.SubTotal.GetValueOrDefault(), basket.BillingCurrency.ISOCode)
                    .ToString();
            checkoutModel.TaxTotal =
                new Money(basket.TaxTotal.GetValueOrDefault(), basket.BillingCurrency.ISOCode)
                    .ToString();
            checkoutModel.ShippingTotal =
                new Money(basket.ShippingTotal.GetValueOrDefault(), basket.BillingCurrency.ISOCode).ToString();
            checkoutModel.PaymentTotal =
                new Money(basket.PaymentTotal.GetValueOrDefault(), basket.BillingCurrency.ISOCode).ToString();
            checkoutModel.OrderTotal =
                new Money(basket.OrderTotal.GetValueOrDefault(), basket.BillingCurrency.ISOCode)
                    .ToString();
        }

        private void MapShippingAddress(OrderAddress shippingInformation, ICollection<Country> countries,
            CheckoutViewModel checkoutModel)
        {
            var selectedShippingCountry = shippingInformation.Country ?? countries.First();

            checkoutModel.ShippingAddressViewModel.FirstName = shippingInformation.FirstName;
            checkoutModel.ShippingAddressViewModel.LastName = shippingInformation.LastName;
            checkoutModel.ShippingAddressViewModel.Line1 = shippingInformation.Line1;
            checkoutModel.ShippingAddressViewModel.City = shippingInformation.City;
            checkoutModel.ShippingAddressViewModel.PostalCode = shippingInformation.PostalCode;
            checkoutModel.ShippingAddressViewModel.Country = new CountryViewModel()
                { Name = selectedShippingCountry.Name, CountryId = selectedShippingCountry.CountryId };
            checkoutModel.ShippingAddressViewModel.EmailAddress = shippingInformation.EmailAddress;
            checkoutModel.ShippingAddressViewModel.PhoneNumber = shippingInformation.MobilePhoneNumber;
        }

        private void MapBillingAddress(OrderAddress billingInformation, ICollection<Country> countries,
            CheckoutViewModel checkoutModel)
        {
            var selectedCountry = billingInformation.Country ?? countries.First();

            checkoutModel.AddressViewModel.FirstName = billingInformation.FirstName;
            checkoutModel.AddressViewModel.LastName = billingInformation.LastName;
            checkoutModel.AddressViewModel.Line1 = billingInformation.Line1;
            checkoutModel.AddressViewModel.City = billingInformation.City;
            checkoutModel.AddressViewModel.PostalCode = billingInformation.PostalCode;
            checkoutModel.AddressViewModel.Country = new CountryViewModel()
                { Name = selectedCountry.Name, CountryId = selectedCountry.CountryId };
            checkoutModel.AddressViewModel.EmailAddress = billingInformation.EmailAddress;
            checkoutModel.AddressViewModel.PhoneNumber = billingInformation.MobilePhoneNumber;
        }

        private bool IsAddressesDifferent(AddressViewModel addressOne, AddressViewModel addressTwo)
        {
            return JsonConvert.SerializeObject(addressOne) != JsonConvert.SerializeObject(addressTwo);
        }
    }
}