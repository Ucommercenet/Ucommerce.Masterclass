using System.Web.Mvc;
using Telerik.Sitefinity.Frontend.Mvc.Infrastructure.Controllers.Attributes;
using Ucommerce.Api;
using Ucommerce.Infrastructure;
using Ucommerce.Masterclass.Sitefinity.Mvc.Models;

namespace Ucommerce.Masterclass.Sitefinity.Mvc.Controllers
{
    [EnhanceViewEngines]
    [Telerik.Sitefinity.Mvc.ControllerToolboxItem(Name = "Shipping", Title = "Shipping", SectionName = "MasterClass")]
    public class ShippingController : Controller
    {
        [System.Web.Mvc.HttpGet]
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Index(int SelectedShippingMethodId)
        {
            return Redirect("/payment");
        }
    }
}