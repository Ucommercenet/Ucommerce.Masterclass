using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Ucommerce.Api;
using Ucommerce.Masterclass.Umbraco.Models;
using Ucommerce.Search.Models;
using Ucommerce.Search.Slugs;
using Umbraco.Web.Mvc;

namespace Ucommerce.Masterclass.Umbraco.Controllers
{
    public class CategoryNavigationController : SurfaceController
    {
        private readonly ICatalogLibrary _catalogLibrary;
        private readonly IUrlService _urlService;
        private readonly ICatalogContext _catalogContext;

        public CategoryNavigationController(ICatalogLibrary catalogLibrary, IUrlService urlService, ICatalogContext catalogContext)
        {
            _catalogLibrary = catalogLibrary;
            _urlService = urlService;
            _catalogContext = catalogContext;
        }
        public ActionResult CategoryNavigation()
        {
            var model = new CategoryNavigationViewModel();

            model.CurrentCategoryGuid = _catalogContext.CurrentCategory?.Guid ?? new Guid();
            model.Categories = MapCategories(_catalogLibrary.GetRootCategories().Results);
            return View("/views/CategoryNavigation/index.cshtml", model);
        }

        private IList<CategoryViewModel> MapCategories(IList<Category> categories)
        {
            var allSubCategoryIds = categories.SelectMany(cat => cat.Categories).Distinct().ToList();
            var subCategoriesById = _catalogLibrary.GetCategories(allSubCategoryIds).ToDictionary(cat => cat.Guid);

            return categories.Select(x => new CategoryViewModel()
            {
                Guid = x.Guid,
                Name = x.Name,
                Url = _urlService.GetUrl(_catalogContext.CurrentCatalog, new Category[] { x }),
                Categories = MapCategories(x.Categories
                    .Where(id => subCategoriesById.ContainsKey(id))
                    .Select(id => subCategoriesById[id]).ToList())
            }).ToList();
        }
    }
}