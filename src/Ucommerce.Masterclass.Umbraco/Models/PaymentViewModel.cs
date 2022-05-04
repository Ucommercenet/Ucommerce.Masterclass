using System.Collections.Generic;

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