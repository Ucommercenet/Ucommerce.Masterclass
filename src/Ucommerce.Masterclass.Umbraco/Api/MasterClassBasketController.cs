using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;
using Ucommerce.Masterclass.Umbraco.Models;
using Umbraco.Web.WebApi;
using Ucommerce.Headless.Domain;

namespace Ucommerce.Masterclass.Umbraco.Api
{
    public class MasterClassBasketController : UmbracoApiController
    {
        public MasterClassBasketController()
        {
        }

        public async Task<CheckoutViewModel> Get(CancellationToken ct)
        {
            return await GetCheckoutModel(ct);
        }


        [System.Web.Mvc.HttpPost]
        public async Task<IHttpActionResult> UpdateOrderLine(UpdateOrderLineRequest updateOrderLineRequest,
            CancellationToken ct)
        {
            return Ok();
        }

        private async Task<CheckoutViewModel> GetCheckoutModel(CancellationToken ct)
        {
            var checkoutModel = new CheckoutViewModel();

            return checkoutModel;
        }

        private static List<CountryViewModel> MapCountries(CountriesOutput countries)
        {
            throw new NotImplementedException();
        }

        private PurchaseOrderViewModel MapPurchaseOrder(Ucommerce.Headless.Domain.GetBasketOutput basket)
        {
            throw new NotImplementedException();
        }

        private void MapTotals(CheckoutViewModel checkoutModel, Ucommerce.Headless.Domain.GetBasketOutput basket)
        {
            throw new NotImplementedException();
        }

        private void MapAddress(Ucommerce.Headless.Domain.OrderAddressOutput address,
            AddressViewModel addressViewModel)
        {
            if (address == null) return;

            addressViewModel.FirstName = address.FirstName;
            addressViewModel.LastName = address.LastName;
            addressViewModel.Line1 = address.Line1;
            addressViewModel.City = address.City;
            addressViewModel.PostalCode = address.PostalCode;
            addressViewModel.Country = new CountryViewModel()
                { Name = address.Country?.Name, CountryId = address.Country?.Id };
            addressViewModel.EmailAddress = address.EmailAddress;
            addressViewModel.PhoneNumber = address.MobilePhoneNumber;
        }

        private bool IsAddressesDifferent(AddressViewModel addressOne, AddressViewModel addressTwo)
        {
            return JsonConvert.SerializeObject(addressOne) != JsonConvert.SerializeObject(addressTwo);
        }
    }
}