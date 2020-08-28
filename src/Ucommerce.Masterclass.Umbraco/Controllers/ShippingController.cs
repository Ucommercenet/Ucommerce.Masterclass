using System.Web.Mvc;
using Ucommerce.Api;
using Ucommerce.Infrastructure;
using Ucommerce.Masterclass.Umbraco.Models;
using Umbraco.Web.Mvc;

namespace Ucommerce.Masterclass.Umbraco.Controllers
{
    public class ShippingController : RenderMvcController
    {
        public ITransactionLibrary TransactionLibrary => ObjectFactory.Instance.Resolve<ITransactionLibrary>();

        [System.Web.Mvc.HttpGet]
        public ActionResult Index()
        {
            var shippingViewModel = new ShippingViewModel();

            var selectedShippingMethod = TransactionLibrary.GetShippingMethod();

            var shippingMethods = TransactionLibrary.GetShippingMethods();

            foreach (var shippingMethod in shippingMethods)
            {
                shippingViewModel.AvailableShippingMethods.Add(new SelectListItem()
                {
                   Text = shippingMethod.Name,
                   Value = shippingMethod.ShippingMethodId.ToString(),
                   Selected = selectedShippingMethod == shippingMethod
                });
            }

            return View(shippingViewModel);
        }


        [HttpPost]
        public ActionResult Index(int SelectedShippingMethodId)
        {
            TransactionLibrary.CreateShipment(SelectedShippingMethodId, Constants.DefaultShipmentAddressName, true);
            TransactionLibrary.ExecuteBasketPipeline();
            
            return Redirect("/payment");
        }
    }
}