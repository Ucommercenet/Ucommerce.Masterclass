using System.Collections.Generic;
using System.Web.Mvc;

namespace Ucommerce.Masterclass.Umbraco.Models
{
    public class ShippingViewModel
    {
        public ShippingViewModel()
        {
            AvailableShippingMethods = new List<SelectListItem>();
        }
        public IList<SelectListItem> AvailableShippingMethods { get; set; }

        public int SelectedShippingMethodId { get; set; }
    }
}