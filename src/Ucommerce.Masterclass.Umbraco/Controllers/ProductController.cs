using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using Ucommerce.Masterclass.Umbraco.Models;
using Ucommerce.Search;
using Ucommerce.Search.Models;
using Umbraco.Web.Mvc;

namespace Ucommerce.Masterclass.Umbraco.Controllers
{
    public class ProductController : RenderMvcController
    {

        public ProductController()
        {
        }

        [System.Web.Mvc.HttpGet]
        public ActionResult Index()
        {
            var productModel = new ProductViewModel();

            return View(productModel);
        }

        [System.Web.Mvc.HttpPost]
        public async Task<ActionResult> Index(UpdateOrderLineRequest updateOrderLineRequest,
            CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        private IList<ProductViewModel> MapVariants(ResultSet<Product> variants)
        {
            return variants.Select(x =>
                new ProductViewModel
                {
                    Name = x.DisplayName,
                    VariantSku = x.VariantSku
                }).ToList();
        }
    }
}