using System.Linq;
using System.Web.Mvc;
using Ucommerce.Api;
using Ucommerce.Infrastructure;
using Ucommerce.Masterclass.Sitefinity.Mvc.Models;

namespace Ucommerce.Masterclass.Sitefinity.Mvc.Controllers
{
    [Telerik.Sitefinity.Mvc.ControllerToolboxItem(Name = "Minibasket", Title = "Minibasket", SectionName = "MasterClass")]
    public class MiniBasketController : Controller
    {
        public ITransactionLibrary TransactionLibrary => ObjectFactory.Instance.Resolve<ITransactionLibrary>();

        public ActionResult Render()
        {
            var miniBasketViewModel = new MiniBasketViewModel();

            if (!TransactionLibrary.HasBasket())
            {
                miniBasketViewModel.Empty = true;
            
                return View(miniBasketViewModel);
            }
            
            var basket = TransactionLibrary.GetBasket(false);
            
            miniBasketViewModel.Empty = false;
            miniBasketViewModel.OrderTotal = new Money(basket.OrderTotal.GetValueOrDefault(0), basket.BillingCurrency.ISOCode).ToString();
            miniBasketViewModel.ItemsInCart = basket.OrderLines.Sum(x => x.Quantity);
            return View(miniBasketViewModel);
        }
    }
}