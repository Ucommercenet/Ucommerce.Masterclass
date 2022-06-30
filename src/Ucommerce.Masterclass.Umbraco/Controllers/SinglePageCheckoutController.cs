using System.Web.Mvc;
using Umbraco.Web.Mvc;

namespace Ucommerce.Masterclass.Umbraco.Controllers
{
    public class SinglePageCheckoutController : RenderMvcController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}