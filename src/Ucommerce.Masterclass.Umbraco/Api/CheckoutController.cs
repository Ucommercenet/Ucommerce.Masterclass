using System.Linq;
using System.Web.Mvc;
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

        private CheckoutViewModel MapViewModel()
        {
            var checkoutModel = new CheckoutViewModel();
            
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
            
            checkoutModel.Countries = countries.Select(x => new CountryViewModel() { Name = x.Name, CountryId = x.CountryId}).ToList();
            checkoutModel.PaymentViewModel.AvailablePaymentMethods = availablePaymentMethods.Select(x => new PaymentMethodViewModel() { Name = x.Name, PaymentMethodId = x.PaymentMethodId}).ToList();
            checkoutModel.ShippingViewModel.AvailableShippingMethods = availableShippingMethods.Select(x => new ShippingMethodViewModel() { Name = x.Name, ShippingMethodId = x.ShippingMethodId}).ToList();

            var purchaseOrder = _transactionLibrary.GetBasket();
            checkoutModel.OrderTotal =
                new Money(purchaseOrder.OrderTotal.GetValueOrDefault(), purchaseOrder.BillingCurrency.ISOCode)
                    .ToString();
            
            return checkoutModel;
        }
        
        public CheckoutViewModel Get()
        {
            EnsureBasketForTesting();

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

            return MapViewModel();
        }

        private void EnsureBasketForTesting()
        {
            if (!_transactionLibrary.HasBasket())
            {
                _transactionLibrary.AddToBasket(1, "100-000-001", "001", executeBasketPipeline: true);
            } 
        }
    }
}