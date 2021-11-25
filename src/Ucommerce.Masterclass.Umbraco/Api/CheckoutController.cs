using System.Linq;
using System.Web.Mvc;
using Ucommerce.Api;
using Ucommerce.Masterclass.Umbraco.Models;
using Umbraco.Web.WebApi;

namespace Ucommerce.Masterclass.Umbraco.Api
{
    public class CheckoutController : UmbracoApiController
    {
        private readonly ITransactionLibrary _transactionLibrary;

        public CheckoutController(ITransactionLibrary transactionLibrary)
        {
            _transactionLibrary = transactionLibrary;
        }
        public CheckoutViewModel Get()
        {
            var checkoutModel = new CheckoutViewModel();
            var billingInformation = _transactionLibrary.GetBillingInformation();
            
            var availablePaymentMethods = _transactionLibrary.GetPaymentMethods();
            var availableShippingMethods = _transactionLibrary.GetShippingMethods();
            
            checkoutModel.PaymentViewModel.AvailablePaymentMethods = availablePaymentMethods.Select(x => new SelectListItem() { Text = x.Name, Value = x.PaymentMethodId.ToString()}).ToList();
            checkoutModel.ShippingViewModel.AvailableShippingMethods = availableShippingMethods.Select(x => new SelectListItem() { Text = x.Name, Value = x.ShippingMethodId.ToString()}).ToList();
            
            return checkoutModel;
        }
    }
}