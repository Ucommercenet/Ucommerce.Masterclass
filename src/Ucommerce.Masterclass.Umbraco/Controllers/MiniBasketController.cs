using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using Ucommerce.Masterclass.Umbraco.Headless;
using Ucommerce.Masterclass.Umbraco.Resolvers;
using Ucommerce.Masterclass.Umbraco.Models;
using Umbraco.Web.Mvc;

namespace Ucommerce.Masterclass.Umbraco.Controllers
{
    public class MiniBasketController : SurfaceController
    {
        private readonly ITransactionClient _transactionClient;
        private readonly IBasketIdResolver _basketIdResolver;

        public MiniBasketController(ITransactionClient transactionClient, IBasketIdResolver basketIdResolver)
        {
            _transactionClient = transactionClient;
            _basketIdResolver = basketIdResolver;
        }

        public async Task<ActionResult> Render(CancellationToken ct)
        { 
            var request = System.Web.HttpContext.Current.Request;
            var basketId = _basketIdResolver.GetBasketId(request);
            
            var miniBasketViewModel = new MiniBasketViewModel();

            if (string.IsNullOrWhiteSpace(basketId))
            {
                miniBasketViewModel.Empty = true;

                return View("/views/Minibasket/index.cshtml", miniBasketViewModel);
            }


            var basket = await _transactionClient.GetOrder(basketId, ct);

            miniBasketViewModel.Empty = false;
            miniBasketViewModel.OrderTotal = new Money(basket.OrderTotal.GetValueOrDefault(0), basket.BillingCurrency.IsoCode).ToString();
            miniBasketViewModel.ItemsInCart = basket.OrderLines.Sum(x => x.Quantity);

            return View("/views/Minibasket/index.cshtml", miniBasketViewModel);
        }
    }
}