using System;

namespace Ucommerce.Masterclass.Umbraco.Models
{
    public class UpdateOrderLineRequest
    {
        public string OrderLineId { get; set; }
        
        public int NewQuantity { get; set; }
    }
}