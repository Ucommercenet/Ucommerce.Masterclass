using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using MC_Headless.Headless;
using Ucommerce.Masterclass.Umbraco.Models;
using MC_Headless.Resolvers;
using Umbraco.Web.WebApi;

namespace MC_Headless.Api
{
    public class MasterClassAddressesController : UmbracoApiController
    {
        private readonly ITransactionClient _transactionClient;
        private readonly IBasketIdResolver _basketIdResolver;
        private readonly IPriceGroupIdResolver _priceGroupIdResolver;
        private readonly ICultureCodeResolver _cultureCodeResolver;

        public MasterClassAddressesController(ITransactionClient transactionClient, IBasketIdResolver basketIdResolver,
            IPriceGroupIdResolver priceGroupIdResolver, ICultureCodeResolver cultureCodeResolver)
        {
            _transactionClient = transactionClient;
            _basketIdResolver = basketIdResolver;
            _priceGroupIdResolver = priceGroupIdResolver;
            _cultureCodeResolver = cultureCodeResolver;
        }

        [HttpPost]
        public async Task<IHttpActionResult> UpdateBilling(AddressViewModel address, CancellationToken ct)
        {
            await _transactionClient.EditBillingInformation(
                basketId: _basketIdResolver.GetBasketId(System.Web.HttpContext.Current.Request),
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
                basketId: _basketIdResolver.GetBasketId(System.Web.HttpContext.Current.Request),
                cultureCodeId: _cultureCodeResolver.GetCultureCode(),
                priceGroupId: _priceGroupIdResolver.PriceGroupId(),
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