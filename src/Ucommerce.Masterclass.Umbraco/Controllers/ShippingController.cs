using System.Web.Mvc;
using Ucommerce.Api;
using Ucommerce.Infrastructure;
using Ucommerce.Masterclass.Umbraco.Models;
using Umbraco.Web.Mvc;

namespace Ucommerce.Masterclass.Umbraco.Controllers
{
    public class ShippingController : RenderMvcController
    {
        [System.Web.Mvc.HttpGet]
        public ActionResult Index()
        {
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