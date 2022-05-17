using System.Threading;
using System.Threading.Tasks;
using Ucommerce.Headless.Domain;

namespace MC_Headless.Headless
{
    public interface ITransactionClient
    {
        Task<PaymentMethodsOutput> GetPaymentMethods(string cultureCode, string countryId, string priceGroupId,
            CancellationToken ct);

        Task<ShippingMethodsOutput> GetShippingMethods(string cultureCode, string countryId, string priceGroupId,
            CancellationToken ct);

        Task<CountriesOutput> GetCountries(CancellationToken ct);

        Task UpdateOrderLineQuantity(string cultureCode, int quantity, string sku, string variantSku,
            string priceGroupId, string productCatalogId, string basketId,
            CancellationToken ct);

        Task<GetBasketOutput> GetBasket(string basketId, CancellationToken ct);

        Task EditBillingInformation(string basketId, string city, string firstName, string lastName,
            string postalCode, string line1, string countryId, string emailAddress, string state,
            string mobilePhoneNumber,
            string attention, string company, CancellationToken ct);

        Task EditShippingInformation(string basketId, string cultureCodeId, string priceGroupId,
            string shippingMethodId, string firstName, string lastName, string emailAddress, string phoneNumber,
            string mobilePhoneNumber, string company, string line1, string line2, string postalCode, string city,
            string attention, string state, string countryId, CancellationToken ct);

        Task<CreatePaymentOutput> CreatePayment(string basketId, string cultureCode, string paymentMethodId, string priceGroupId,
            CancellationToken ct);
        
        Task<GetOrderOutput> GetOrder(string orderId, CancellationToken ct);
    }
}