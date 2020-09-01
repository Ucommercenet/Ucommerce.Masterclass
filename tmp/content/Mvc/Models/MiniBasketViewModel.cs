namespace Ucommerce.Masterclass.Sitefinity.Mvc.Models
{
    public class MiniBasketViewModel
    {
        public bool Empty { get; set; }
        
        public string OrderTotal { get; set; }
        public int ItemsInCart { get; set; }
    }
}