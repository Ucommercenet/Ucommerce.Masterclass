using System.Linq;
using System.Web.Mvc;
using Telerik.Sitefinity.Frontend.Mvc.Infrastructure.Controllers.Attributes;
using Ucommerce.Api;
using Ucommerce.Infrastructure;
using Ucommerce.Masterclass.Sitefinity.Mvc.Models;

namespace Ucommerce.Masterclass.Sitefinity.Mvc.Controllers
{
    [EnhanceViewEngines]
    [Telerik.Sitefinity.Mvc.ControllerToolboxItem(Name = "Minibasket", Title = "Minibasket", SectionName = "MasterClass")]
    public class MiniBasketController : Controller
    {
        public ITransactionLibrary TransactionLibrary => ObjectFactory.Instance.Resolve<ITransactionLibrary>();

        public ActionResult Index()
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