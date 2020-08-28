using System.Web.Mvc;
using Ucommerce.Masterclass.Umbraco.Models;
using Umbraco.Web.Mvc;

namespace Ucommerce.Masterclass.Umbraco.Controllers
{
    public class PaymentController : RenderMvcController
    {
        
        [System.Web.Mvc.HttpGet]
        public ActionResult Index()
        {
            return View(new PaymentViewModel());
        }


        [HttpPost]
        public ActionResult Index(int selectedPaymentMethod)
        {
            return Redirect("/preview");
        }
    }
}