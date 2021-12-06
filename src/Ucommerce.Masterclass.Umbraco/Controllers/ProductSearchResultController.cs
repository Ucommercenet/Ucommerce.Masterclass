using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Ucommerce.Api;
using Ucommerce.Masterclass.Umbraco.Models;
using Ucommerce.Search;
using Ucommerce.Search.Models;
using Ucommerce.Search.Slugs;
using Umbraco.Web.Mvc;

namespace Ucommerce.Masterclass.Umbraco.Controllers
{
    public class ProductSearchResultController : RenderMvcController
    {
        public ProductSearchResultController()
        {

        }

        public ActionResult Index()
        {
            var model = new ProductListViewModel();

            var searchTerm = GetSearchTerm();

            // model.ProductViewModels = MapProducts(result.Results);

            return View(model);
        }

        private string GetSearchTerm()
        {
            return Request.QueryString["Query"];
        }

        private IList<ProductViewModel> MapProducts(IList<Product> products)
        {
            return new List<ProductViewModel>();
        }
    }
}