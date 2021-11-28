using System.Collections.Generic;
using System.Web.Mvc;

namespace Ucommerce.Masterclass.Umbraco.Models
{
    public class CheckoutViewModel 
    {
        public CheckoutViewModel()
        {
            AddressViewModel = new AddressViewModel();
            PaymentViewModel = new PaymentViewModel();
            ShippingViewModel = new ShippingViewModel();
        }
        public AddressViewModel AddressViewModel { get; set; }
        
        public PaymentViewModel PaymentViewModel { get; set; }
        
        public IList<CountryViewModel> Countries { get; set; }

        public ShippingViewModel ShippingViewModel { get; set; }
        
        public string OrderTotal { get; set; }
    }
}

