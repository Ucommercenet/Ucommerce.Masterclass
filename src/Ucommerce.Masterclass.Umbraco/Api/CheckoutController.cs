using System.Linq;
using System.Web.Mvc;
using Ucommerce.Api;
using Ucommerce.EntitiesV2;
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

        public CheckoutViewModel Get()
        {
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

        private CheckoutViewModel MapViewModel()
        {
            var checkoutModel = new CheckoutViewModel();

            var basket = _transactionLibrary.GetBasket();

            var countries = _transactionLibrary.GetCountries();
            var billingInformation = _transactionLibrary.GetBillingInformation();
            var selectedCountry = billingInformation.Country ?? countries.First();

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
    }
}