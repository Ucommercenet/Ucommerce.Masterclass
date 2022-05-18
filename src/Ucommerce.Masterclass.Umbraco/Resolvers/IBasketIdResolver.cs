using System.Web;


namespace Ucommerce.Masterclass.Umbraco.Resolvers
{
    public interface IBasketIdResolver
    {
        string GetBasketId(HttpRequest request);
    }
}