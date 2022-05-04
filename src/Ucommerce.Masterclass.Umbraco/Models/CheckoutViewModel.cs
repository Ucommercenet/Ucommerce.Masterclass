using System.Collections.Generic;

namespace Ucommerce.Masterclass.Umbraco.Models
{
    public class CheckoutViewModel
    {
        public CheckoutViewModel()
        {
            BillingAddressViewModel = new AddressViewModel();
            ShippingAddressViewModel = new AddressViewModel();
            PaymentViewModel = new PaymentViewModel();
            ShippingViewModel = new ShippingViewModel();
        }

        public PurchaseOrderViewModel PurchaseOrderViewModel { get; set; }

        public AddressViewModel BillingAddressViewModel { get; set; }
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

