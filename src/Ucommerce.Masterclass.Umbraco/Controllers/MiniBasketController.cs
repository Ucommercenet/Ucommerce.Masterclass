using System.Web.Mvc;
using Ucommerce.Api;
using Ucommerce.Infrastructure;
using Ucommerce.Masterclass.Umbraco.Models;
using Umbraco.Web.Mvc;

namespace Ucommerce.Masterclass.Umbraco.Controllers
{
    public class MiniBasketController : SurfaceController
    {
        public ITransactionLibrary TransactionLibrary => ObjectFactory.Instance.Resolve<ITransactionLibrary>();

        public ActionResult Render()
        {
            var miniBasketViewModel = new MiniBasketViewModel();

            if (!TransactionLibrary.HasBasket())
            {
                miniBasketViewModel.Empty = true;
            
                return View("/views/Minibasket/index.cshtml", miniBasketViewModel);
            }
            
            var basket = TransactionLibrary.GetBasket(false);
            
            miniBasketViewModel.Empty = false;
            
            return View("/views/Minibasket/index.cshtml", miniBasketViewModel);
        }
    }
}