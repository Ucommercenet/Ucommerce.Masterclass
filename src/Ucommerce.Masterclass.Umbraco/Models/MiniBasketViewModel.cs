namespace Ucommerce.Masterclass.Umbraco.Models
{
    public class MiniBasketViewModel
    {
        public bool Empty { get; set; }
        
        public string OrderTotal { get; set; }
        public int ItemsInCart { get; set; }
    }
}