using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Ucommerce.Masterclass.Umbraco.Exceptions;
using Ucommerce.Masterclass.Umbraco.Headless;
using Ucommerce.Masterclass.Umbraco.Models;
using Ucommerce.Masterclass.Umbraco.Resolvers;
using Umbraco.Web.WebApi;

namespace Ucommerce.Masterclass.Umbraco.Api
{
    public class MasterClassShippingMethodController : UmbracoApiController
    {
        private readonly ITransactionClient _transactionClient;
        private readonly IBasketIdResolver _basketResolver;
        private readonly IPriceGroupIdResolver _priceGroupIdResolver;
        private readonly ICultureCodeResolver _cultureCodeResolver;

        public MasterClassShippingMethodController(ITransactionClient transactionClient,
            IBasketIdResolver basketResolver, IPriceGroupIdResolver priceGroupIdResolver,
            ICultureCodeResolver cultureCodeResolver)
        {
            _transactionClient = transactionClient;
            _basketResolver = basketResolver;
            _priceGroupIdResolver = priceGroupIdResolver;
            _cultureCodeResolver = cultureCodeResolver;
        }

        [HttpPost]
        public async Task<IHttpActionResult> Update(CheckoutViewModel checkoutViewModel, CancellationToken ct)
        {
            var basketId = _basketResolver.GetBasketId(System.Web.HttpContext.Current.Request);
            if (string.IsNullOrEmpty(basketId))
                throw new MissingBasketIdException("Couldn't read basket id from cookies.");

            var shippingAddressViewModel = checkoutViewModel.ShippingAddressViewModel;

            var shippingMethodId =
                checkoutViewModel.ShippingViewModel.SelectedShippingMethod.ShippingMethodId.ToString();

            if (shippingMethodId != null)
            {
                await _transactionClient.EditShippingInformation(basketId, _cultureCodeResolver.GetCultureCode(),
                    _priceGroupIdResolver.PriceGroupId(), shippingMethodId, shippingAddressViewModel.FirstName,
                    shippingAddressViewModel.LastName, shippingAddressViewModel.EmailAddress,
                    shippingAddressViewModel.PhoneNumber, shippingAddressViewModel.MobilePhoneNumber,
                    shippingAddressViewModel.CompanyName, shippingAddressViewModel.Line1,
                    shippingAddressViewModel.Line2, shippingAddressViewModel.PostalCode, shippingAddressViewModel.City,
                    shippingAddressViewModel.Attention, shippingAddressViewModel.State,
                    shippingAddressViewModel.Country.CountryId, ct);

                return Ok();
            }

            return BadRequest("Missing Shipping method");
        }
    }
}