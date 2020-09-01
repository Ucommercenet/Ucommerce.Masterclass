using System.Collections.Generic;
using System.Web.Mvc;

namespace Ucommerce.Masterclass.Sitefinity.Mvc.Models
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