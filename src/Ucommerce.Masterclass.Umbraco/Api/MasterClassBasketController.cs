using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;
using Ucommerce.Masterclass.Umbraco.Models;
using Umbraco.Web.WebApi;
using Ucommerce.Headless.Domain;
using Ucommerce.Masterclass.Umbraco.Headless;
using Ucommerce.Masterclass.Umbraco.Resolvers;

namespace Ucommerce.Masterclass.Umbraco.Api
{
    public class MasterClassBasketController : UmbracoApiController
    {
        private readonly ITransactionClient _transactionClient;
        private readonly IPriceGroupIdResolver _priceGroupIdResolver;
        private readonly ICultureCodeResolver _cultureCodeResolver;
        private readonly IProductCatalogIdResolver _productCatalogIdResolver;
        private readonly IBasketIdResolver _basketIdResolver;

        public MasterClassBasketController(ITransactionClient transactionClient,
            IPriceGroupIdResolver priceGroupIdResolver, ICultureCodeResolver cultureCodeResolver,
            IProductCatalogIdResolver productCatalogIdResolver, IBasketIdResolver basketIdResolver)
        {
            _transactionClient = transactionClient;
            _priceGroupIdResolver = priceGroupIdResolver;
            _cultureCodeResolver = cultureCodeResolver;
            _productCatalogIdResolver = productCatalogIdResolver;
            _basketIdResolver = basketIdResolver;
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
            return new CheckoutViewModel();
        }

        private List<CountryViewModel> MapCountries(CountriesOutput countries)
        {
            return countries.Countries.Select(x => new CountryViewModel { Name = x.Name, CountryId = x.Id }).ToList();
        }

        private PurchaseOrderViewModel MapPurchaseOrder(Ucommerce.Headless.Domain.GetBasketOutput basket)
        {
            var model = new PurchaseOrderViewModel
            {
                OrderLines = basket.OrderLines.Select(orderLine => new OrderlineViewModel
                {
                    Quantity = orderLine.Quantity,
                    ProductName = orderLine.ProductName,
                    Discount = orderLine.Discount,
                    Total = new Money(orderLine.Total.GetValueOrDefault(), basket.BillingCurrency.IsoCode).ToString(),
                    TotalWithDiscount =
                        new Money(orderLine.Price - orderLine.Discount, basket.BillingCurrency.IsoCode).ToString(),
                    Sku = orderLine.Sku,
                    VariantSku = orderLine.VariantSku
                }).ToList()
            };

            return model;
        }

        private void MapTotals(CheckoutViewModel checkoutModel, Ucommerce.Headless.Domain.GetBasketOutput basket)
        {
            // Implement this method
        }

        private AddressViewModel MapAddress(Ucommerce.Headless.Domain.OrderAddressOutput address)
        {
            if (address == null) return new AddressViewModel();
            return new AddressViewModel
            {
                FirstName = address.FirstName,
                LastName = address.LastName,
                Line1 = address.Line1,
                City = address.City,
                PostalCode = address.PostalCode,
                Country = new CountryViewModel { Name = address.Country?.Name, CountryId = address.Country?.Id },
                EmailAddress = address.EmailAddress,
                PhoneNumber = address.MobilePhoneNumber
            };
        }

        private bool IsAddressesDifferent(AddressViewModel addressOne, AddressViewModel addressTwo)
        {
            return JsonConvert.SerializeObject(addressOne) != JsonConvert.SerializeObject(addressTwo);
        }
    }
}