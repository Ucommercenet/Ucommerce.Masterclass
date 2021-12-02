﻿using System.Linq;
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
            return MapViewModel();
        }

        [System.Web.Mvc.HttpPost]
        public IHttpActionResult UpdateOrderLine(UpdateOrderLineRequest updateOrderLineRequest)
        {
            _transactionLibrary.UpdateLineItemByOrderLineId(updateOrderLineRequest.OrderLineId, updateOrderLineRequest.NewQuantity);
            _transactionLibrary.ExecuteBasketPipeline();

            return Ok();
        }

        private CheckoutViewModel MapViewModel()
        {
            var checkoutModel = new CheckoutViewModel();

            if (!_transactionLibrary.HasBasket())
                return null;

            var basket = _transactionLibrary.GetBasket();

            var countries = _transactionLibrary.GetCountries();
            var billingInformation = _transactionLibrary.GetBillingInformation();
            var shippingInformation = _transactionLibrary.GetShippingInformation();
            var selectedCountry = billingInformation.Country ?? countries.First();

            checkoutModel.PurchaseOrderViewModel = MapPurchaseOrder(basket);

            checkoutModel.AddressViewModel.FirstName = billingInformation.FirstName;
            checkoutModel.AddressViewModel.LastName = billingInformation.LastName;
            checkoutModel.AddressViewModel.Line1 = billingInformation.Line1;
            checkoutModel.AddressViewModel.City = billingInformation.City;
            checkoutModel.AddressViewModel.PostalCode = billingInformation.PostalCode;
            checkoutModel.AddressViewModel.Country = new CountryViewModel() { Name = selectedCountry.Name, CountryId = selectedCountry.CountryId };
            checkoutModel.AddressViewModel.EmailAddress = billingInformation.EmailAddress;
            checkoutModel.AddressViewModel.PhoneNumber = billingInformation.MobilePhoneNumber;

            var selectedShippingCountry = shippingInformation.Country ?? countries.First();

            checkoutModel.ShippingAddressViewModel.FirstName = shippingInformation.FirstName;
            checkoutModel.ShippingAddressViewModel.LastName = shippingInformation.LastName;
            checkoutModel.ShippingAddressViewModel.Line1 = shippingInformation.Line1;
            checkoutModel.ShippingAddressViewModel.City = shippingInformation.City;
            checkoutModel.ShippingAddressViewModel.PostalCode = shippingInformation.PostalCode;
            checkoutModel.ShippingAddressViewModel.Country = new CountryViewModel() { Name = selectedShippingCountry.Name, CountryId = selectedShippingCountry.CountryId };
            checkoutModel.ShippingAddressViewModel.EmailAddress = shippingInformation.EmailAddress;
            checkoutModel.ShippingAddressViewModel.PhoneNumber = shippingInformation.MobilePhoneNumber;

            checkoutModel.DifferentShippingAddress = IsAddressesDifferent(checkoutModel.ShippingAddressViewModel, checkoutModel.AddressViewModel);

            var availablePaymentMethods = _transactionLibrary.GetPaymentMethods(selectedCountry);
            var availableShippingMethods = _transactionLibrary.GetShippingMethods(selectedCountry);

            checkoutModel.Countries = countries.Select(x => new CountryViewModel() { Name = x.Name, CountryId = x.CountryId }).ToList();
            checkoutModel.PaymentViewModel.AvailablePaymentMethods = availablePaymentMethods.Select(x => new PaymentMethodViewModel() { Name = x.Name, PaymentMethodId = x.PaymentMethodId }).ToList();
            checkoutModel.ShippingViewModel.AvailableShippingMethods = availableShippingMethods.Select(x => new ShippingMethodViewModel() { Name = x.Name, ShippingMethodId = x.ShippingMethodId }).ToList();

            ShippingMethod selectedShippingMethod = basket.Shipments.FirstOrDefault()?.ShippingMethod;
            if (selectedShippingMethod != null)
            {
                checkoutModel.ShippingViewModel.SelectedShippingMethod = new ShippingMethodViewModel()
                { Name = selectedShippingMethod.Name, ShippingMethodId = selectedShippingMethod.ShippingMethodId };
            }

            PaymentMethod selectedPaymentMethod = basket.Payments.FirstOrDefault()?.PaymentMethod;
            if (selectedPaymentMethod != null)
            {
                checkoutModel.PaymentViewModel.SelectedPaymentMethod = new PaymentMethodViewModel()
                { Name = selectedPaymentMethod.Name, PaymentMethodId = selectedPaymentMethod.PaymentMethodId };
            }

            var purchaseOrder = basket;
            checkoutModel.OrderTotal =
                new Money(purchaseOrder.OrderTotal.GetValueOrDefault(), purchaseOrder.BillingCurrency.ISOCode)
                    .ToString();

            return checkoutModel;
        }

        private bool IsAddressesDifferent(AddressViewModel addressOne, AddressViewModel addressTwo)
        {
            return JsonConvert.SerializeObject(addressOne) != JsonConvert.SerializeObject(addressTwo);
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