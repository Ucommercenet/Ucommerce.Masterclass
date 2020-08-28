using System.Web.Mvc;
using Ucommerce.Masterclass.Umbraco.Models;
using Umbraco.Web.Mvc;

namespace Ucommerce.Masterclass.Umbraco.Controllers
{
    public class ShippingController : RenderMvcController
    {
        
        [System.Web.Mvc.HttpGet]
        public ActionResult Index()
        {
            return View(new ShippingViewModel());
        }


        [HttpPost]
        public ActionResult Index(int selectedShippingMethod)
        {
            return Redirect("/payment");
        }
    }
}