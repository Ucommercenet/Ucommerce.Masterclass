using Ucommerce.Api;

namespace MC_Headless.Resolvers.Impl
{
    public class CatalogContextPriceGroupIdResolver : IPriceGroupIdResolver
    {
        private readonly ICatalogContext _catalogContext;

        public CatalogContextPriceGroupIdResolver(ICatalogContext catalogContext)
        {
            _catalogContext = catalogContext;
        }
        public string PriceGroupId()
        {
            return _catalogContext.CurrentCatalog.DefaultPriceGroup.ToString();
        }
    }
}