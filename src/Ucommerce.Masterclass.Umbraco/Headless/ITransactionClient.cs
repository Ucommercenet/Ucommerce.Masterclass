using System.Threading;
using System.Threading.Tasks;
using Ucommerce.Headless.Domain;

namespace Ucommerce.Masterclass.Umbraco.Headless
{
    public interface ITransactionClient
    {
        Task CreateShipment(string basketId, string paymentMethodId, string priceGroupId, CancellationToken ct);
        Task CreatePayment(string basketId, string cultureCode, string paymentMethodId, string priceGroupId,
            CancellationToken ct);
        Task<PaymentMethodsOutput> GetPaymentMethods(string cultureCode, string countryId, string priceGroupId,
            CancellationToken ct);
        Task<ShippingMethodsOutput> GetShippingMethods(string cultureCode, string countryId, string priceGroupId, CancellationToken ct);
        Task<CountriesOutput> GetCountries(CancellationToken ct);
        Task UpdateOrderLineQuantity(string orderLineId, int quantity, CancellationToken ct);
        Task<GetBasketOutput> GetBasket(string basketId, CancellationToken ct);
        Task EditBillingInformation(string basketId, string firstName, string lastName, string emailAddress, string phoneNumber, string mobilePhoneNumber, string company, string line1, string line2, string postalCode, string city, string attention, string state, string countryId, CancellationToken ct);
        Task EditShippingInformation(string basketId, string cultureCodeId, string priceGroupId, string shippingMethodId, string firstName, string lastName, string emailAddress, string phoneNumber, string mobilePhoneNumber, string company, string line1, string line2, string postalCode, string city, string attention, string state, string countryId, CancellationToken ct);
        Task<string> Checkout(string basketId, string cultureCode, string paymentMethodId, string priceGroupId, CancellationToken ct);
    }
}
