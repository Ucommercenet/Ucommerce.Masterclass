using System.Linq;
using System.Web.Mvc;
using Telerik.Sitefinity.Frontend.Mvc.Infrastructure.Controllers.Attributes;
using Ucommerce.Api;
using Ucommerce.Infrastructure;
using Ucommerce.Masterclass.Sitefinity.Mvc.Models;

namespace Ucommerce.Masterclass.Sitefinity.Mvc.Controllers
{
    [EnhanceViewEngines]
    [Telerik.Sitefinity.Mvc.ControllerToolboxItem(Name = "Payment", Title = "Payment", SectionName = "MasterClass")]
    public class PaymentController : Controller
    {
        public ITransactionLibrary TransactionLibrary => ObjectFactory.Instance.Resolve<ITransactionLibrary>();

        [System.Web.Mvc.HttpGet]
        public ActionResult Index()
        {
            var paymentViewModel = new PaymentViewModel();

            var selectedPaymentMethod = TransactionLibrary.GetBasket(false).Payments.FirstOrDefault()?.PaymentMethod;
            
            foreach (var paymentMethod in TransactionLibrary.GetPaymentMethods())
            {
                paymentViewModel.AvailablePaymentMethods.Add(new SelectListItem()
                {
                    Text = paymentMethod.Name,
                    Value = paymentMethod.PaymentMethodId.ToString(),
                    Selected = selectedPaymentMethod == paymentMethod
                });
            }

            return View(paymentViewModel);
        }


        [HttpPost]
        public ActionResult Index(int selectedPaymentMethodId)
        {
            TransactionLibrary.CreatePayment(selectedPaymentMethodId, -1, false, true);

            return Redirect("/preview");
        }
    }
}