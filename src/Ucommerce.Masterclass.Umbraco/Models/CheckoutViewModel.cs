using System.Collections.Generic;
using System.Web.Mvc;

namespace Ucommerce.Masterclass.Umbraco.Models
{
    public class CheckoutViewModel 
    {
        public CheckoutViewModel()
        {
            AddressViewModel = new AddressViewModel();
            ShippingAddressViewModel = new AddressViewModel();
            PaymentViewModel = new PaymentViewModel();
            ShippingViewModel = new ShippingViewModel();
        }
        
        public PurchaseOrderViewModel PurchaseOrderViewModel { get; set; }
        
        public AddressViewModel AddressViewModel { get; set; }
        public AddressViewModel ShippingAddressViewModel { get; set; }

        public PaymentViewModel PaymentViewModel { get; set; }
        
        public IList<CountryViewModel> Countries { get; set; }

        public ShippingViewModel ShippingViewModel { get; set; }
        
        public string OrderTotal { get; set; }
        public string SubTotal { get; set; }
        public string TaxTotal { get; set; }
        public string Discount { get; set; }
        public bool DifferentShippingAddress { get; set; }
        public string ShippingTotal { get; set; }
        public string PaymentTotal { get; set; }
    }
}

