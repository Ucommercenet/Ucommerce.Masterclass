using System.Net.Http;

namespace Ucommerce.Masterclass.Umbraco.Resolvers
{
    public interface IBasketIdResolver
    {
        string GetBasketId(HttpRequestMessage request);
    }
}
