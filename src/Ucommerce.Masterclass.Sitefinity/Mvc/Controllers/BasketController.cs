using System;
using System.Linq;
using System.Web.Mvc;
using Telerik.Sitefinity.Frontend.Mvc.Infrastructure.Controllers.Attributes;
using Telerik.Sitefinity.Mvc;
using Ucommerce.Api;
using Ucommerce.EntitiesV2;
using Ucommerce.Infrastructure;
using Ucommerce.Marketing;
using Ucommerce.Marketing.Awards.AwardResolvers;
using Ucommerce.Marketing.TargetingContextAggregators;
using Ucommerce.Marketing.Targets.TargetResolvers;
using Ucommerce.Masterclass.Sitefinity.Mvc.Models;

namespace Ucommerce.Masterclass.Sitefinity.Mvc.Controllers
{
    [EnhanceViewEngines]
    [ControllerToolboxItem(Name = "Basket", Title = "Basket", SectionName = "MasterClass")]
    public class BasketController : Controller
    {
        public ITransactionLibrary TransactionLibrary => ObjectFactory.Instance.Resolve<ITransactionLibrary>();

        [System.Web.Mvc.HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult Index(int quantity, int orderlineId)
        {
            return Index();
        }
    }
}