using System.Web.Http;
using System.Web.Mvc;
using Ucommerce.Api;
using Ucommerce.Infrastructure;
using Ucommerce.Masterclass.Umbraco.Models;
using Umbraco.Web.Mvc;

namespace Ucommerce.Masterclass.Umbraco.Controllers
{
    public class CategoryController : RenderMvcController
    {
        public ICatalogLibrary CatalogLibrary => ObjectFactory.Instance.Resolve<ICatalogLibrary>();

        public ICatalogContext CatalogContext => ObjectFactory.Instance.Resolve<ICatalogContext>();

        public CategoryController()
        {
            
        }

        public ActionResult Index()
        {
            var currentCategory = CatalogContext.CurrentCategory;
            
            var categoryModel = new CategoryViewModel();

            categoryModel.Name = currentCategory.Name;
            categoryModel.ImageMediaUrl = currentCategory.ImageMediaUrl;
            
            return View("/views/category/index.cshtml", categoryModel);
        }
    }
}