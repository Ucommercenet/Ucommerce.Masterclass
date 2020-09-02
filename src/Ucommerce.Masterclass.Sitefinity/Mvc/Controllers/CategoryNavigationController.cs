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
        public ICatalogLibrary CatalogLibrary => ObjectFactory.Instance.Resolve<ICatalogLibrary>();

        public IUrlService UrlService => ObjectFactory.Instance.Resolve<IUrlService>();

        public ICatalogContext CatalogContext => ObjectFactory.Instance.Resolve<ICatalogContext>();

        public ActionResult Index()
        {
            var catalogContext = ObjectFactory.Instance.Resolve<ICatalogContext>();
            var catalogLibrary = ObjectFactory.Instance.Resolve<ICatalogLibrary>();
            
            var model = new CategoryNavigationViewModel();

            model.CurrentCategoryGuid = catalogContext.CurrentCategory?.Guid ?? new Guid();
            model.Categories = MapCategories(catalogLibrary.GetRootCategories().Results);
            return View(model);
        }

        private IList<CategoryViewModel> MapCategories(IList<Category> categories)
        {
            var allSubCategoryIds = categories.SelectMany(cat => cat.Categories).Distinct().ToList();
            var subCategoriesById = CatalogLibrary.GetCategories(allSubCategoryIds).ToDictionary(cat => cat.Guid);
            
            return categories.Select(x => new CategoryViewModel()
            {
                Guid = x.Guid,
                Name = x.Name,
                Url = UrlService.GetUrl(CatalogContext.CurrentCatalog, new Category[] { x }),
                Categories = MapCategories(x.Categories
                    .Where(id => subCategoriesById.ContainsKey(id))
                    .Select(id => subCategoriesById[id]).ToList())
            }).ToList();
        }
    }
}