using System.Web.Mvc;
using Ucommerce.Api;
using Ucommerce.Infrastructure;
using Ucommerce.Masterclass.Umbraco.Models;
using Ucommerce.Search;
using Ucommerce.Search.Facets;
using Ucommerce.Search.Models;
using Umbraco.Web.Mvc;

namespace Ucommerce.Masterclass.Umbraco.Controllers
{
    public class ShippingController : RenderMvcController
    {
        [System.Web.Mvc.HttpGet]
        public ActionResult Index()
        {
            ObjectFactory.Instance.Resolve<IIndex<Product>>().Find().Where(x => x.LongDescription == Match.FullText("input")).ToList();

            var shippingViewModel = new ShippingViewModel();
            
            return View(shippingViewModel);
        }


        [HttpPost]
        public ActionResult Index(int SelectedShippingMethodId)
        {
            return Redirect("/payment");
        }
    }
}