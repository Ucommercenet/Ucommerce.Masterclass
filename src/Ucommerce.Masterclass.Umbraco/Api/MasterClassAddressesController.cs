﻿using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Ucommerce.Masterclass.Umbraco.Headless;
using Ucommerce.Masterclass.Umbraco.Models;
using Umbraco.Web.WebApi;

namespace Ucommerce.Masterclass.Umbraco.Api
{
    public class MasterClassAddressesController : UmbracoApiController
    {
        private readonly ITransactionClient _transactionClient;

        public MasterClassAddressesController(ITransactionClient transactionClient)
        {
            _transactionClient = transactionClient;
        }

        [HttpPost]
        public async Task<IHttpActionResult> UpdateBilling(AddressViewModel address, CancellationToken ct)
        {
            await _transactionClient.EditBillingInformation(
                basketId: "",
                city: address.City ?? "",
                firstName: address.FirstName ?? "",
                lastName: address.LastName ?? "",
                postalCode: address.PostalCode ?? "",
                line1: address.Line1 ?? "",
                countryId: address.Country.CountryId,
                emailAddress: address.EmailAddress ?? "",
                state: address.State ?? "",
                mobilePhoneNumber: address.PhoneNumber ?? "",
                attention: address.Attention ?? "",
                company: address.CompanyName ?? "",
                ct
            );

            return Ok();
        }

        [System.Web.Mvc.HttpPost]
        public async Task<IHttpActionResult> UpdateShipping(AddressViewModel address, CancellationToken ct)
        {
            await _transactionClient.EditShippingInformation(
                basketId: "",
                cultureCodeId: "",
                priceGroupId: "",
                shippingMethodId: address.ShippingMethodId ?? "",
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
                countryId: address.Country.CountryId,
                ct
            );

            return Ok();
        }
    }
}