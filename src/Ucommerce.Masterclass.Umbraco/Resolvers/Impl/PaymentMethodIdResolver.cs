using System.Linq;
using System.Net.Http;

namespace MC_Headless.Resolvers.Impl
{
    public class PaymentMethodIdResolver: IPaymentMethodIdResolver
    {
        public string GetSelectedPaymentMethodId(HttpRequestMessage request)
        {
            return request.Headers.GetCookies().Select(c => c["SelectedPaymentMethodId"])
                .FirstOrDefault()?.Value ?? "";
        }
    }
}