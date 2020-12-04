using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using Ucommerce.Api;
using Ucommerce.Infrastructure;
using Ucommerce.Masterclass.Umbraco.Models;
using Ucommerce.Search;
using Ucommerce.Search.Models;
using Ucommerce.Search.Slugs;
using Umbraco.Web.Mvc;

namespace Ucommerce.Masterclass.Umbraco.Controllers
{
    public class CategoryNavigationController : SurfaceController
    {
        public ActionResult CategoryNavigation()
        {
            var model = new CategoryNavigationViewModel();

            return View("/views/CategoryNavigation/index.cshtml", model);
        }

        private IList<CategoryViewModel> MapCategories(IList<Category> categories)
        {
            return new List<CategoryViewModel>();
        }
    }
}