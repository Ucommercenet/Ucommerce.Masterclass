using System.Web.Mvc;
using Umbraco.Web.Mvc;

namespace Ucommerce.Masterclass.Umbraco.Controllers
{
    public class ProductSearchController : SurfaceController
    {
        public ActionResult ProductSearch()
        {
            return View("/views/ProductSearch/index.cshtml");
        }
    }
}