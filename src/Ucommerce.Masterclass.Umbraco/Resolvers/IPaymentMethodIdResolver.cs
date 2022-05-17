using System.Web;

namespace MC_Headless.Resolvers
{
    public interface IPaymentMethodIdResolver
    {
        string GetSelectedPaymentMethodId(HttpRequest request);
    }
}