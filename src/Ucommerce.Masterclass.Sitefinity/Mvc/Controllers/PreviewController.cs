using System.Web.Mvc;
using Ucommerce.Masterclass.Sitefinity.Mvc.Models;

namespace Ucommerce.Masterclass.Sitefinity.Mvc.Controllers
{
    [Telerik.Sitefinity.Mvc.ControllerToolboxItem(Name = "Preview", Title = "Preview", SectionName = "MasterClass")]
    public class PreviewController : Controller
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