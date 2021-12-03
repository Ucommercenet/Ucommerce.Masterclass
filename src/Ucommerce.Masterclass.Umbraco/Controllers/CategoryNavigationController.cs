using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Ucommerce.Api;
using Ucommerce.Infrastructure;
using Ucommerce.Masterclass.Umbraco.Models;
using Ucommerce.Search.Models;
using Ucommerce.Search.Slugs;
using Umbraco.Web.Mvc;

namespace Ucommerce.Masterclass.Umbraco.Controllers
{
    public class CategoryNavigationController : SurfaceController
    {
        public ICatalogLibrary CatalogLibrary => ObjectFactory.Instance.Resolve<ICatalogLibrary>();

        public IUrlService UrlService => ObjectFactory.Instance.Resolve<IUrlService>();

        public ICatalogContext CatalogContext => ObjectFactory.Instance.Resolve<ICatalogContext>();

        public ActionResult CategoryNavigation()
        {
            var catalogContext = ObjectFactory.Instance.Resolve<ICatalogContext>();
            var catalogLibrary = ObjectFactory.Instance.Resolve<ICatalogLibrary>();

            var model = new CategoryNavigationViewModel();

            model.CurrentCategoryGuid = catalogContext.CurrentCategory?.Guid ?? new Guid();
            model.Categories = MapCategories(catalogLibrary.GetRootCategories().Results);
            return View("/views/CategoryNavigation/index.cshtml", model);
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