using System.Web;

namespace Ucommerce.Masterclass.Umbraco.Resolvers
{
    public interface IPaymentMethodIdResolver
    {
        string GetSelectedPaymentMethodId(HttpRequest request);
    }
}