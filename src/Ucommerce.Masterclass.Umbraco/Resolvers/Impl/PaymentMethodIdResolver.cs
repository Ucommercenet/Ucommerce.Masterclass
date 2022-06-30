using System.Web;

namespace Ucommerce.Masterclass.Umbraco.Resolvers.Impl
{
    public class PaymentMethodIdResolver: IPaymentMethodIdResolver
    {
        public string GetSelectedPaymentMethodId(HttpRequest request)
        {
            return request.Cookies["SelectedPaymentMethodId"]?.Value ?? "";
        }
    }
}