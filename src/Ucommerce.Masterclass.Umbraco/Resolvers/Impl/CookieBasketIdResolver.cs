using System;
using System.Web;

namespace Ucommerce.Masterclass.Umbraco.Resolvers.Impl
{
    public class CookieBasketIdResolver : IBasketIdResolver
    {
        public string GetBasketId(HttpRequest request)
        {
            return request.Cookies["basketId"]?.Value ?? "";
        }
    }
}