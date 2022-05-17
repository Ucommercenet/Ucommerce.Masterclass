using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;
using MC_Headless.Exceptions;
using MC_Headless.Headless;
using Ucommerce.Masterclass.Umbraco.Models;
using MC_Headless.Resolvers;
using Umbraco.Web.WebApi;
using Ucommerce.Headless.Domain;
using Ucommerce;

namespace MC_Headless.Api
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
            var basketId = _basketIdResolver.GetBasketId(System.Web.HttpContext.Current.Request);
            if (string.IsNullOrWhiteSpace(basketId))
                throw new MissingBasketIdException("Couldn't read basket id from cookies.");

            var cultureCode = _cultureCodeResolver.GetCultureCode();
            var priceGroupId = _priceGroupIdResolver.PriceGroupId();
            var productCatalogId = _productCatalogIdResolver.ProductCatalogId();

            await _transactionClient.UpdateOrderLineQuantity(cultureCode, updateOrderLineRequest.NewQuantity,
                updateOrderLineRequest.Sku, updateOrderLineRequest.VariantSku, priceGroupId, productCatalogId, basketId,
                ct);

            return Ok();
        }

        private async Task<CheckoutViewModel> GetCheckoutModel(CancellationToken ct)
        {
            var checkoutModel = new CheckoutViewModel();

            var basketId = _basketIdResolver.GetBasketId(System.Web.HttpContext.Current.Request);
            if (string.IsNullOrWhiteSpace(basketId))
                throw new MissingBasketIdException("Couldn't read basket id from cookies.");

            var basket = await _transactionClient.GetBasket(basketId, ct);
            checkoutModel.PurchaseOrderViewModel = MapPurchaseOrder(basket);

            MapTotals(checkoutModel, basket);

            //TODO: Task 02 -> Present the address details
            checkoutModel.BillingAddressViewModel = MapAddress(basket.BillingAddress);
            checkoutModel.ShippingAddressViewModel = MapAddress(basket.Shipments.FirstOrDefault()?.ShipmentAddress);

            var countries = await _transactionClient.GetCountries(ct);
            checkoutModel.Countries = MapCountries(countries);

            checkoutModel.DifferentShippingAddress = IsAddressesDifferent(checkoutModel.ShippingAddressViewModel,
                checkoutModel.BillingAddressViewModel);

            //TODO: Task 03 -> Present the available shipping methods
            var selectedCountry = checkoutModel.BillingAddressViewModel.Country != null
                ? countries.Countries.First(x => x.Id.Equals(checkoutModel.BillingAddressViewModel.Country.CountryId))
                : countries.Countries.First();

            if (checkoutModel.BillingAddressViewModel.Country == null)
                checkoutModel.BillingAddressViewModel.Country = new CountryViewModel
                    { CountryId = selectedCountry.Id, Name = selectedCountry.Name };

            var availableShippingMethods = await _transactionClient.GetShippingMethods(
                _cultureCodeResolver.GetCultureCode(), selectedCountry.Id, _priceGroupIdResolver.PriceGroupId(), ct);
            checkoutModel.ShippingViewModel.AvailableShippingMethods = availableShippingMethods.ShippingMethods
                .Select(x => new ShippingMethodViewModel { Name = x.Name, ShippingMethodId = new Guid(x.Id) })
                .ToList();

            var selectedShippingMethod = basket.Shipments.FirstOrDefault()?.ShippingMethod;
            if (selectedShippingMethod != null)
            {
                checkoutModel.ShippingViewModel.SelectedShippingMethod = new ShippingMethodViewModel
                    { Name = selectedShippingMethod.Name, ShippingMethodId = selectedShippingMethod.Id };
            }

            //TODO: Task 04 -> Present the available payment methods
            var availablePaymentMethods = await _transactionClient.GetPaymentMethods(
                _cultureCodeResolver.GetCultureCode(), selectedCountry.Id, _priceGroupIdResolver.PriceGroupId(), ct);
            checkoutModel.PaymentViewModel.AvailablePaymentMethods = availablePaymentMethods.PaymentMethods
                .Select(x => new PaymentMethodViewModel { Name = x.Name, PaymentMethodId = x.Id }).ToList();

            return checkoutModel;
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
            checkoutModel.Discount =
                new Money(basket.Discount.GetValueOrDefault(), basket.BillingCurrency.IsoCode)
                    .ToString();
            checkoutModel.SubTotal =
                new Money(basket.SubTotal.GetValueOrDefault(), basket.BillingCurrency.IsoCode)
                    .ToString();
            checkoutModel.TaxTotal =
                new Money(basket.Vat.GetValueOrDefault(), basket.BillingCurrency.IsoCode)
                    .ToString();
            checkoutModel.ShippingTotal =
                new Money(basket.ShippingTotal.GetValueOrDefault(), basket.BillingCurrency.IsoCode).ToString();
            checkoutModel.PaymentTotal =
                new Money(basket.PaymentTotal.GetValueOrDefault(), basket.BillingCurrency.IsoCode).ToString();
            checkoutModel.OrderTotal =
                new Money(basket.OrderTotal.GetValueOrDefault(), basket.BillingCurrency.IsoCode)
                    .ToString();
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
                PhoneNumber = address.PhoneNumber
            };
        }

        private bool IsAddressesDifferent(AddressViewModel addressOne, AddressViewModel addressTwo)
        {
            return JsonConvert.SerializeObject(addressOne) != JsonConvert.SerializeObject(addressTwo);
        }
    }
}