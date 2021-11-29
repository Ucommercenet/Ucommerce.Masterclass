using System.Collections.Generic;
using System.Web.Mvc;

namespace Ucommerce.Masterclass.Umbraco.Models
{
    public class ShippingViewModel
    {
        public ShippingViewModel()
        {
            AvailableShippingMethods = new List<ShippingMethodViewModel>();
        }
        public IList<ShippingMethodViewModel> AvailableShippingMethods { get; set; }

        public ShippingMethodViewModel SelectedShippingMethod { get; set; }
    }
}