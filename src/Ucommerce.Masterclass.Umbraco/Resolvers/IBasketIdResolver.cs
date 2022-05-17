using System.Web;


namespace MC_Headless.Resolvers
{
    public interface IBasketIdResolver
    {
        string GetBasketId(HttpRequest request);
    }
}