using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;
using Ucommerce.Api;
using Ucommerce.Headless.Domain;
using Ucommerce.Masterclass.Umbraco.Exceptions;
using Ucommerce.Masterclass.Umbraco.Headless;
using Ucommerce.Masterclass.Umbraco.Models;
using Ucommerce.Masterclass.Umbraco.Resolvers;
using Umbraco.Web.WebApi;

namespace Ucommerce.Masterclass.Umbraco.Api
{
    public class MasterClassBasketController : UmbracoApiController
    {
        private readonly ITransactionClient _transactionClient;
        private readonly IPriceGroupIdResolver _priceGroupIdResolver;
        private readonly ICultureCodeResolver _cultureCodeResolver;

        public MasterClassBasketController(ITransactionClient transactionClient, IPriceGroupIdResolver priceGroupIdResolver, ICultureCodeResolver cultureCodeResolver)
        {
            _transactionClient = transactionClient;
            _priceGroupIdResolver = priceGroupIdResolver;
            _cultureCodeResolver = cultureCodeResolver;
        }

        public async Task<CheckoutViewModel> Get(CancellationToken ct)
        {
            return await GetCheckoutModel(ct);
        }
        

        [System.Web.Mvc.HttpPost]
        public async Task<IHttpActionResult> UpdateOrderLine(UpdateOrderLineRequest updateOrderLineRequest, CancellationToken ct)
        {
            await _transactionClient.UpdateOrderLineQuantity(updateOrderLineRequest.OrderLineId,
                updateOrderLineRequest.NewQuantity, ct);

            return Ok();
        }

        private async Task<CheckoutViewModel> GetCheckoutModel(CancellationToken ct)
        {
            var checkoutModel = new CheckoutViewModel();

            var basketId = GetBasketCookieValue();
            if (string.IsNullOrEmpty(basketId))
                throw new MissingBasketIdException("Couldn't read basket id from cookies.");

            var basket = await _transactionClient.GetBasket(basketId, ct);
            checkoutModel.PurchaseOrderViewModel = MapPurchaseOrder(basket);

            MapTotals(checkoutModel, basket);

            //TODO: Task 02 -> Present the address details
            MapAddress(basket.BillingAddress, checkoutModel.AddressViewModel);
            MapAddress(basket.Shipments.FirstOrDefault()?.ShipmentAddress, checkoutModel.ShippingAddressViewModel);

            var countries = await _transactionClient.GetCountries(ct);
            checkoutModel.Countries = MapCountries(countries);

            checkoutModel.DifferentShippingAddress = IsAddressesDifferent(checkoutModel.ShippingAddressViewModel, checkoutModel.AddressViewModel);

            //TODO: Task 03 -> Present the available shipping methods
            var selectedCountry = checkoutModel.AddressViewModel.Country != null
                ? countries.Countries.First(x => x.Id.Equals(checkoutModel.AddressViewModel.Country.CountryId))
                : countries.Countries.First();

            if (checkoutModel.AddressViewModel.Country == null)
                checkoutModel.AddressViewModel.Country = new CountryViewModel()
                    { CountryId = selectedCountry.Id, Name = selectedCountry.Name };

            var availableShippingMethods = await _transactionClient.GetShippingMethods(_cultureCodeResolver.GetCultureCode(), selectedCountry.Id, _priceGroupIdResolver.PriceGroupId(), ct);
            checkoutModel.ShippingViewModel.AvailableShippingMethods = availableShippingMethods.ShippingMethods.Select(x => new ShippingMethodViewModel() { Name = x.Name, ShippingMethodId = new Guid(x.Id) }).ToList();

            PurchaseOrderShippingMethodOutput selectedShippingMethod = basket.Shipments.FirstOrDefault()?.ShippingMethod;
            if (selectedShippingMethod != null)
            {
                checkoutModel.ShippingViewModel.SelectedShippingMethod = new ShippingMethodViewModel()
                { Name = selectedShippingMethod.Name, ShippingMethodId = selectedShippingMethod.Id };
            }

            //TODO: Task 04 -> Present the available payment methods
            var availablePaymentMethods = await _transactionClient.GetPaymentMethods(_cultureCodeResolver.GetCultureCode(), selectedCountry.Id, _priceGroupIdResolver.PriceGroupId(), ct);
            checkoutModel.PaymentViewModel.AvailablePaymentMethods = availablePaymentMethods.PaymentMethods.Select(x => new PaymentMethodViewModel() { Name = x.Name, PaymentMethodId = x.Id }).ToList();

            dynamic selectedPaymentMethod = null;//basket.Payments.FirstOrDefault()?.PaymentMethod;
            if (selectedPaymentMethod != null)
            {
                checkoutModel.PaymentViewModel.SelectedPaymentMethod = new PaymentMethodViewModel()
                { Name = selectedPaymentMethod.Name, PaymentMethodId = selectedPaymentMethod.PaymentMethodId };
            }

            return checkoutModel;
        }

        private static List<CountryViewModel> MapCountries(CountriesOutput countries)
        {
            return countries.Countries.Select(x => new CountryViewModel() { Name = x.Name, CountryId = x.Id }).ToList();
        }

        private PurchaseOrderViewModel MapPurchaseOrder(Ucommerce.Headless.Domain.GetBasketOutput basket)
        {
            var model = new PurchaseOrderViewModel();

            model.OrderLines = basket.OrderLines.Select(orderLine => new OrderlineViewModel()
            {
                Quantity = orderLine.Quantity,
                ProductName = orderLine.ProductName,
                Discount = orderLine.Discount,
                Total = new Money(orderLine.Total.GetValueOrDefault(), basket.BillingCurrency.IsoCode).ToString(),
                TotalWithDiscount =
                    new Money(orderLine.Price - orderLine.Discount, basket.BillingCurrency.IsoCode).ToString(),
                OrderLineId = orderLine.Id
            }).ToList();

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

        private string GetBasketCookieValue()
        {
            return this.Request.Headers.GetCookies().Select(c => c["basketId"])
                .FirstOrDefault()?.Value ?? "";
        }
    }
}