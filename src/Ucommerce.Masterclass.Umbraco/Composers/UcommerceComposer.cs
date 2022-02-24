using Ucommerce.Api;
using Ucommerce.Masterclass.Umbraco.Headless;
using Ucommerce.Masterclass.Umbraco.Resolvers;
using Ucommerce.Masterclass.Umbraco.Resolvers.Impl;
using Umbraco.Core.Composing;

namespace Ucommerce.Masterclass.Umbraco.Composers
{
    public class UcommerceComposer : IUserComposer
    {
        public void Compose(Composition composition)
        {
            //Always use transient lifetime scope as this is what the IOC container of Ucommerce is tailored for. If you register as singleton, you can get unforseen sideeffects.
            
            //Services for browse
            composition.RegisterFor<Ucommerce.Api.ICatalogLibrary, Ucommerce.Api.CatalogLibrary>(x => Ucommerce.Infrastructure.ObjectFactory.Instance.Resolve<ICatalogLibrary>());
            composition.RegisterFor<Ucommerce.Api.ICatalogContext, Ucommerce.Api.CatalogContext>(x => Ucommerce.Infrastructure.ObjectFactory.Instance.Resolve<ICatalogContext>());
            composition.RegisterFor<Ucommerce.Search.Slugs.IUrlService, Ucommerce.Search.Slugs.UrlService>(x => Ucommerce.Infrastructure.ObjectFactory.Instance.Resolve<Ucommerce.Search.Slugs.IUrlService>());
            
            //Services for checkout
            composition.RegisterFor<Ucommerce.Api.ITransactionLibrary, Ucommerce.Api.TransactionLibrary>(x => Ucommerce.Infrastructure.ObjectFactory.Instance.Resolve<ITransactionLibrary>());
            composition.RegisterFor<ITransactionClient, TransactionClient>();
            composition.RegisterFor<IBasketIdResolver, CookieBasketIdResolver>();
            composition.RegisterFor<IPriceGroupIdResolver, CatalogContextPriceGroupIdResolver>();
            composition.RegisterFor<ICultureCodeResolver, ThreadCultureCodeResolver>();
        }
    }
}