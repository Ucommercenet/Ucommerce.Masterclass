using System;
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
        public BasketController()
        {
        }

        public CheckoutViewModel Get()
        {
            var checkoutModel = new CheckoutViewModel();

            return checkoutModel;
        }

        [System.Web.Mvc.HttpPost]
        public IHttpActionResult UpdateOrderLine(UpdateOrderLineRequest updateOrderLineRequest)
        {
            return Ok();
        }

        private PurchaseOrderViewModel MapPurchaseOrder(Ucommerce.EntitiesV2.PurchaseOrder basket)
        {
            throw new NotImplementedException();
        }

        private void MapTotals(CheckoutViewModel checkoutModel, PurchaseOrder basket)
        {
            throw new NotImplementedException();
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