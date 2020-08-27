using System.Collections.Generic;
using System.Web.Mvc;

namespace Ucommerce.Masterclass.Umbraco.Models
{
    public class AddressDetailsViewModel
    {
        public AddressDetailsViewModel()
        {
            ShippingAddress = new AddressViewModel();
            BillingAddress = new AddressViewModel();
            AvailableCountries = new List<SelectListItem>();
        }
        public AddressViewModel ShippingAddress { get; set; }

        public AddressViewModel BillingAddress { get; set; }
        
        public bool UseAlternativeAddress { get; set; }

        public IList<SelectListItem> AvailableCountries { get; set; }
    }
}