using System.Web.Mvc;
using Telerik.Sitefinity.Frontend.Mvc.Infrastructure.Controllers.Attributes;
using Ucommerce.Masterclass.Sitefinity.Mvc.Models;

namespace Ucommerce.Masterclass.Sitefinity.Mvc.Controllers
{
    [EnhanceViewEngines]
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