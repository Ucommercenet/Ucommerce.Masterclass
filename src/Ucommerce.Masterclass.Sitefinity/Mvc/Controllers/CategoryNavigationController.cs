using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using Telerik.Sitefinity.Frontend.Mvc.Infrastructure.Controllers.Attributes;
using Ucommerce.Api;
using Ucommerce.Infrastructure;
using Ucommerce.Masterclass.Sitefinity.Mvc.Models;
using Ucommerce.Search;
using Ucommerce.Search.Models;
using Ucommerce.Search.Slugs;

namespace Ucommerce.Masterclass.Sitefinity.Mvc.Controllers
{
    [EnhanceViewEngines]
    [Telerik.Sitefinity.Mvc.ControllerToolboxItem(Name = "CategoryNavigation", Title = "CategoryNavigation", SectionName = "MasterClass")]
    public class CategoryNavigationController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        private IList<CategoryViewModel> MapCategories(IList<Category> categories)
        {
            return new List<CategoryViewModel>();
        }
    }
}