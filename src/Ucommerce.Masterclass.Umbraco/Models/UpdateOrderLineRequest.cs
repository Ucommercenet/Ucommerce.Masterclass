namespace Ucommerce.Masterclass.Umbraco.Models
{
    public class UpdateOrderLineRequest
    {
        public string Sku { get; set; }
        public string VariantSku { get; set; }
        public int NewQuantity { get; set; }
    }
}