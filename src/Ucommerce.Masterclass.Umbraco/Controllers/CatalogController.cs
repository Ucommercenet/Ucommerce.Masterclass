using System;
using System.Collections.Generic;
using System.Linq;
using Ucommerce.Api;
using Ucommerce.Infrastructure;
using Ucommerce.Extensions;
using Ucommerce.Search;
using Ucommerce.Search.Facets;
using Ucommerce.Search.Models;

namespace Ucommerce.Masterclass.Umbraco.Controllers
{
    public class CatalogController
    {
        private readonly ICatalogContext _catalogContext;
        private readonly ICatalogLibrary _catalogLibrary;
        private readonly IIndex<Product> _productIndex;

        public CatalogController(ICatalogContext catalogContext, ICatalogLibrary catalogLibrary, IIndex<Ucommerce.Search.Models.Product> productIndex)
        {
            _catalogContext = catalogContext;
            _catalogLibrary = catalogLibrary;
            _productIndex = productIndex;

            Ucommerce.Search.Models.Category category = _catalogContext.CurrentCategory;

            _productIndex.Find().Where(x => x.Sku == "Sku").ToList();
            
            _productIndex.Find().Where(x => x["Sku"] == "Sku").ToList();

            _productIndex
                .Find()
                .Where(x => x.Categories
                    .Contains(_catalogContext.CurrentCatalog.Categories)).ToList();

            var facetDictionary = new FacetDictionary();
            facetDictionary["color"] = new[] {"red", "blue"};

            FacetResultSet<Product> facets = _productIndex.Find()
                .Where(x => x.LongDescription == Match.FullText("Search"))
                .Where(facetDictionary).Skip(0).Take(10000000).ToFacets();
            
        }
    }
}