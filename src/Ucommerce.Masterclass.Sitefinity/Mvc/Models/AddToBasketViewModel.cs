namespace Ucommerce.Masterclass.Sitefinity.Mvc.Models
{
    /// <summary>
    /// Used when posting back selected product to be added
    /// </summary>
    public class AddToBasketViewModel
    {
        public string Sku { get; set; }

        public string VariantSku { get; set; }

        public int Quantity { get; set; }
    }
}