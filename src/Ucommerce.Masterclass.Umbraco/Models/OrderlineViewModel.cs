namespace Ucommerce.Masterclass.Umbraco.Models
{
    public class OrderlineViewModel
    {
        public string Total { get; set; }

        public int Quantity { get; set; }

        public string Sku { get; set; }

        public string VariantSku { get; set; }

        public string ProductName { get; set; }
        public string TotalWithDiscount { get; set; }
        public decimal Discount { get; set; }
    }
}