using System.Collections.Generic;

namespace Ucommerce.Masterclass.Umbraco.Models
{
    public class PurchaseOrderViewModel
    {
        public PurchaseOrderViewModel()
        {
            OrderLines = new List<OrderlineViewModel>();
        }

        public AddressViewModel BillingAddress { get; set; }

        public AddressViewModel ShippingAddress { get; set; }

        public IList<OrderlineViewModel> OrderLines { get; set; }

        public string OrderTotal { get; set; }

        public string SubTotal { get; set; }

        public string TaxTotal { get; set; }

        public string DiscountTotal { get; set; }

        public string ShippingTotal { get; set; }

        public string PaymentTotal { get; set; }

        public int RemoveOrderlineId { get; set; }
    }
}