using System.Net.Http;


namespace MC_Headless.Resolvers
{
    public interface IBasketIdResolver
    {
        string GetBasketId(HttpRequestMessage request);
    }
}