using System.Linq;
using System.Net.Http;

namespace MC_Headless.Resolvers.Impl
{
    public class CookieBasketIdResolver : IBasketIdResolver
    {
        public string GetBasketId(HttpRequestMessage request)
        {
            return request.Headers.GetCookies().Select(c => c["basketId"])
                .FirstOrDefault()?.Value ?? "";
        }
    }
}