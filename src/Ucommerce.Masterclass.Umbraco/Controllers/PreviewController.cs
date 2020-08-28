using System.Web.Mvc;
using Ucommerce.Masterclass.Umbraco.Models;
using Umbraco.Web.Mvc;

namespace Ucommerce.Masterclass.Umbraco.Controllers
{
    public class PreviewController : RenderMvcController
    {
        
        [System.Web.Mvc.HttpGet]
        public ActionResult Index()
        {
            return View(new PurchaseOrderViewModel());
        }


        [HttpPost]
        public ActionResult Index(int selectedPaymentMethod)
        {
            return Redirect("/preview");
        }
    }
}