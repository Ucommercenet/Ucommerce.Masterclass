﻿using System.Linq;
using System.Web.Mvc;
using Ucommerce.Api;
using Ucommerce.EntitiesV2;
using Ucommerce.Extensions;
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

        private CheckoutViewModel MapViewModel()
        {
            var checkoutModel = new CheckoutViewModel();
            
            var basket = _transactionLibrary.GetBasket();

            var countries = _transactionLibrary.GetCountries();
            var billingInformation = _transactionLibrary.GetBillingInformation();
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
            
            var availablePaymentMethods = _transactionLibrary.GetPaymentMethods(selectedCountry);
            var availableShippingMethods = _transactionLibrary.GetShippingMethods(selectedCountry);
            
            checkoutModel.Countries = countries.Select(x => new CountryViewModel() { Name = x.Name, CountryId = x.CountryId}).ToList();
            checkoutModel.PaymentViewModel.AvailablePaymentMethods = availablePaymentMethods.Select(x => new PaymentMethodViewModel() { Name = x.Name, PaymentMethodId = x.PaymentMethodId}).ToList();
            checkoutModel.ShippingViewModel.AvailableShippingMethods = availableShippingMethods.Select(x => new ShippingMethodViewModel() { Name = x.Name, ShippingMethodId = x.ShippingMethodId}).ToList();

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

        public CheckoutViewModel Get()
        {
            EnsureBasketForTesting();

            return MapViewModel();
        }

        [HttpGet]
        public PaymentRequestViewModel GetPaymentPageUrl()
        {
            var basket = _transactionLibrary.GetBasket();
            return new PaymentRequestViewModel()
            {
                PaymentPageUrl = _transactionLibrary.GetPaymentPageUrl(basket.Payments.First())
            };
        }

        [HttpPost]
        public CheckoutViewModel UpdateOrderLine(UpdateOrderLineRequest updateOrderLineRequest)
        {
            _transactionLibrary.UpdateLineItemByOrderLineId(updateOrderLineRequest.OrderLineId, updateOrderLineRequest.NewQuantity);
            _transactionLibrary.ExecuteBasketPipeline();

            return MapViewModel();
        }

        [HttpPost]
        public CheckoutViewModel Update(CheckoutViewModel checkoutViewModel)
        {
            var address = checkoutViewModel.AddressViewModel;
            _transactionLibrary.EditBillingInformation(
                firstName: address.FirstName ?? "",
                lastName: address.LastName ?? "",
                emailAddress: address.EmailAddress ?? "",
                phoneNumber: address.PhoneNumber,
                mobilePhoneNumber: address.PhoneNumber,
                company: address.CompanyName ?? "",
                line1: address.Line1 ?? "",
                line2: address.Line2 ?? "",
                postalCode: address.PostalCode ?? "",
                city: address.City ?? "", 
                attention: address.Attention ?? "",
                state: address.State ?? "",
                countryId: address.Country.CountryId
            );
            
            _transactionLibrary.EditShippingInformation(
                firstName: address.FirstName ?? "",
                lastName: address.LastName ?? "",
                emailAddress: address.EmailAddress ?? "",
                phoneNumber: address.PhoneNumber,
                mobilePhoneNumber: address.PhoneNumber,
                company: address.CompanyName ?? "",
                line1: address.Line1 ?? "",
                line2: address.Line2 ?? "",
                postalCode: address.PostalCode ?? "",
                city: address.City ?? "", 
                attention: address.Attention ?? "",
                state: address.State ?? "",
                countryId: address.Country.CountryId
            );

            if (checkoutViewModel?.ShippingViewModel?.SelectedShippingMethod?.ShippingMethodId != null)
            {
                _transactionLibrary.CreateShipment(checkoutViewModel.ShippingViewModel.SelectedShippingMethod.ShippingMethodId, Constants.DefaultShipmentAddressName, true);
            }
            
            if (checkoutViewModel?.PaymentViewModel?.SelectedPaymentMethod?.PaymentMethodId != null)
            {
                _transactionLibrary.CreatePayment(checkoutViewModel.PaymentViewModel.SelectedPaymentMethod.PaymentMethodId);
            }

            return MapViewModel();
        }

        private void EnsureBasketForTesting()
        {
            if (!_transactionLibrary.HasBasket())
            {
                _transactionLibrary.AddToBasket(1, "100-000-001", "001", executeBasketPipeline: true, addToExistingLine: false);
                _transactionLibrary.AddToBasket(1, "100-000-001", "001", executeBasketPipeline: true, addToExistingLine: false);
                _transactionLibrary.AddToBasket(1, "100-000-001", "001", executeBasketPipeline: true, addToExistingLine: false);
            } 
        }
    }
}