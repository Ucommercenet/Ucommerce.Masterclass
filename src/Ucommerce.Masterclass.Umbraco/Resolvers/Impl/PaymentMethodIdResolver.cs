using System.Web;

namespace MC_Headless.Resolvers.Impl
{
    public class PaymentMethodIdResolver: IPaymentMethodIdResolver
    {
        public string GetSelectedPaymentMethodId(HttpRequest request)
        {
            return request.Cookies["SelectedPaymentMethodId"].Value ?? "";
        }
    }
}