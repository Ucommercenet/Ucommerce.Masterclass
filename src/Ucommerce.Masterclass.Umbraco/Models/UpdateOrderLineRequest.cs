namespace Ucommerce.Masterclass.Umbraco.Models
{
    public class UpdateOrderLineRequest
    {
        public int OrderLineId { get; set; }
        
        public int NewQuantity { get; set; }
    }
}