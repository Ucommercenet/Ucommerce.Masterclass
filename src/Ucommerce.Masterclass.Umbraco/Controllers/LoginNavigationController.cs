using System.Web.Mvc;
using Umbraco.Web.Mvc;

namespace Ucommerce.Masterclass.Umbraco.Controllers
{
    public class LoginNavigationController : SurfaceController
    {
        public ActionResult LoginNavigation()
        {
            return View("/views/CategoryNavigation/index.cshtml");
        }
    }
}