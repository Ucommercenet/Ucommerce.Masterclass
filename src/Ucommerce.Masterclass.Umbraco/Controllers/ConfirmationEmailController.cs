﻿using System;
using System.Linq;
using System.Web.Mvc;
using Ucommerce.Api;
using Ucommerce.EntitiesV2;
using Ucommerce.Masterclass.Umbraco.Models;
using Umbraco.Core;
using Umbraco.Web.Mvc;

namespace Ucommerce.Masterclass.Umbraco.Controllers
{
    public class ConfirmationEmailController : RenderMvcController
    {
        private readonly ITransactionLibrary _transactionLibrary;

        public ConfirmationEmailController(ITransactionLibrary transactionLibrary)
        {
            _transactionLibrary = transactionLibrary;
        }

        [System.Web.Mvc.HttpGet]
        public ActionResult Index()
        {
            var orderGuidParameterFromQueryString = System.Web.HttpContext.Current.Request.QueryString["OrderGuid"];

            if (orderGuidParameterFromQueryString.IsNullOrWhiteSpace())
                return View(new PurchaseOrderViewModel());

            var basket = _transactionLibrary.GetPurchaseOrder(Guid.Parse(orderGuidParameterFromQueryString));

            var billingInformation = basket.GetBillingAddress();
            var shippingInformation = basket.GetShippingAddress("Shipment");
            var selectedCountry = billingInformation.Country;

            PurchaseOrderViewModel purchaseOrderViewModel = MapPurchaseOrder(basket);

            purchaseOrderViewModel.BillingAddress.FirstName = billingInformation.FirstName;
            purchaseOrderViewModel.BillingAddress.LastName = billingInformation.LastName;
            purchaseOrderViewModel.BillingAddress.Line1 = billingInformation.Line1;
            purchaseOrderViewModel.BillingAddress.City = billingInformation.City;
            purchaseOrderViewModel.BillingAddress.PostalCode = billingInformation.PostalCode;
            purchaseOrderViewModel.BillingAddress.Country = new CountryViewModel() { Name = selectedCountry.Name, CountryId = selectedCountry.CountryId };
            purchaseOrderViewModel.BillingAddress.EmailAddress = billingInformation.EmailAddress;
            purchaseOrderViewModel.BillingAddress.PhoneNumber = billingInformation.MobilePhoneNumber;

            var selectedShippingCountry = shippingInformation.Country;

            purchaseOrderViewModel.ShippingAddress.FirstName = shippingInformation.FirstName;
            purchaseOrderViewModel.ShippingAddress.LastName = shippingInformation.LastName;
            purchaseOrderViewModel.ShippingAddress.Line1 = shippingInformation.Line1;
            purchaseOrderViewModel.ShippingAddress.City = shippingInformation.City;
            purchaseOrderViewModel.ShippingAddress.PostalCode = shippingInformation.PostalCode;
            purchaseOrderViewModel.ShippingAddress.Country = new CountryViewModel() { Name = selectedShippingCountry.Name, CountryId = selectedShippingCountry.CountryId };
            purchaseOrderViewModel.ShippingAddress.EmailAddress = shippingInformation.EmailAddress;
            purchaseOrderViewModel.ShippingAddress.PhoneNumber = shippingInformation.MobilePhoneNumber;

            purchaseOrderViewModel.DiscountTotal =
                new Money(basket.Discount.GetValueOrDefault(), basket.BillingCurrency.ISOCode)
                    .ToString();
            purchaseOrderViewModel.SubTotal =
                new Money(basket.SubTotal.GetValueOrDefault(), basket.BillingCurrency.ISOCode)
                    .ToString();
            purchaseOrderViewModel.TaxTotal =
                new Money(basket.TaxTotal.GetValueOrDefault(), basket.BillingCurrency.ISOCode)
                    .ToString();
            purchaseOrderViewModel.ShippingTotal =
                new Money(basket.ShippingTotal.GetValueOrDefault(), basket.BillingCurrency.ISOCode).ToString();
            purchaseOrderViewModel.PaymentTotal =
                new Money(basket.PaymentTotal.GetValueOrDefault(), basket.BillingCurrency.ISOCode).ToString();
            purchaseOrderViewModel.OrderTotal =
                new Money(basket.OrderTotal.GetValueOrDefault(), basket.BillingCurrency.ISOCode).ToString();

            return View(purchaseOrderViewModel);
        }

        private PurchaseOrderViewModel MapPurchaseOrder(PurchaseOrder basket)
        {
            var model = new PurchaseOrderViewModel();

            model.OrderLines = basket.OrderLines.Select(orderLine => new OrderlineViewModel()
            {
                Quantity = orderLine.Quantity,
                ProductName = orderLine.ProductName,
                Total = new Money(orderLine.Total.GetValueOrDefault(), basket.BillingCurrency.ISOCode).ToString(),
                TotalWithDiscount =
                    new Money(orderLine.Total.GetValueOrDefault() - orderLine.Discount, basket.BillingCurrency.ISOCode).ToString(),
                Discount = orderLine.Discount,
                OrderLineId = orderLine.OrderLineId
            }).ToList();

            return model;
        }
    }
}