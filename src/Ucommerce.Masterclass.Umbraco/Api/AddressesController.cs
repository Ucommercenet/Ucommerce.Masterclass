using System.Linq;
using System.Web.Http;
using Ucommerce.Api;
using Ucommerce.EntitiesV2;
using Ucommerce.Masterclass.Umbraco.Models;
using Umbraco.Web.WebApi;

namespace Ucommerce.Masterclass.Umbraco.Api
{
    public class AddressesController : UmbracoApiController
    {
        private readonly ITransactionLibrary _transactionLibrary;

        public AddressesController(ITransactionLibrary transactionLibrary)
        {
            _transactionLibrary = transactionLibrary;
        }

        [HttpPost]
        public IHttpActionResult UpdateBilling(CheckoutViewModel checkoutViewModel)
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

            return Ok();
        }

        [System.Web.Mvc.HttpPost]
        public IHttpActionResult UpdateShipping(CheckoutViewModel checkoutViewModel)
        {
            var address = checkoutViewModel.AddressViewModel;

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
            
            return Ok();
        }
    }
}