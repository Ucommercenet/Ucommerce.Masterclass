using System.Net.Http;

namespace MC_Headless.Resolvers
{
    public interface IPaymentMethodIdResolver
    {
        string GetSelectedPaymentMethodId(HttpRequestMessage request);
    }
}