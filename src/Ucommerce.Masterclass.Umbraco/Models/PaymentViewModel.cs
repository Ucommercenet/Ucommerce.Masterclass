using System.Collections.Generic;
using System.Web.Mvc;

namespace Ucommerce.Masterclass.Umbraco.Models
{
    public class PaymentViewModel
    {
        public PaymentViewModel()
        {
            AvailablePaymentMethods = new List<PaymentMethodViewModel>();	
        }

        public IList<PaymentMethodViewModel> AvailablePaymentMethods { get; set; }

        public PaymentMethodViewModel SelectedPaymentMethod { get; set; }
    }
}