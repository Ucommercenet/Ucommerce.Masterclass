using Ucommerce.Api;

namespace MC_Headless.Resolvers.Impl
{
    public class CatalogContextProductCatalogIdResolver : IProductCatalogIdResolver
    {
        private readonly ICatalogContext _catalogContext;

        public CatalogContextProductCatalogIdResolver(ICatalogContext catalogContext)
        {
            _catalogContext = catalogContext;
        }
        public string ProductCatalogId()
        {
            return _catalogContext.CurrentCatalog.Guid.ToString();
        }
    }
}