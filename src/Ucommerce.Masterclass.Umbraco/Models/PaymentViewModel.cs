using System.Collections.Generic;
using System.Web.Mvc;

namespace Ucommerce.Masterclass.Umbraco.Models
{
    public class PaymentViewModel
    {
        public PaymentViewModel()
        {
            AvailablePaymentMethods = new List<SelectListItem>();	
        }

        public IList<SelectListItem> AvailablePaymentMethods { get; set; }

        public int SelectedPaymentMethodId { get; set; }
    }
}