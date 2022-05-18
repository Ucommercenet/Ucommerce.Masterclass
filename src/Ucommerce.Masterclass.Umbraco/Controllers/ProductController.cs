﻿using System;
using System.Collections.Generic;
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
        public ActionResult Index(string sku, string variantSku, int quantity)
        {
            return Index();
        }

        private IList<ProductViewModel> MapVariants(ResultSet<Product> variants)
        {
            throw new NotImplementedException();
        }
    }
}