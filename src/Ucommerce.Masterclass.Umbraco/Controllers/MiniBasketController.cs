using System.Linq;
using System.Web.Mvc;
using Ucommerce.Api;
using Ucommerce.Infrastructure;
using Ucommerce.Masterclass.Umbraco.Models;
using Umbraco.Web.Mvc;

namespace Ucommerce.Masterclass.Umbraco.Controllers
{
    public class MiniBasketController : SurfaceController
    {
        private readonly ITransactionLibrary _transactionLibrary;

        public MiniBasketController(ITransactionLibrary transactionLibrary)
        {
            _transactionLibrary = transactionLibrary;
        }

        public ActionResult Render()
        {
            var miniBasketViewModel = new MiniBasketViewModel();

            if (!_transactionLibrary.HasBasket())
            {
                miniBasketViewModel.Empty = true;

                return View("/views/Minibasket/index.cshtml", miniBasketViewModel);
            }

            var basket = _transactionLibrary.GetBasket(false);

            miniBasketViewModel.Empty = false;
            miniBasketViewModel.OrderTotal = new Money(basket.OrderTotal.GetValueOrDefault(0), basket.BillingCurrency.ISOCode).ToString();
            miniBasketViewModel.ItemsInCart = basket.OrderLines.Sum(x => x.Quantity);
            return View("/views/Minibasket/index.cshtml", miniBasketViewModel);
        }
    }
}