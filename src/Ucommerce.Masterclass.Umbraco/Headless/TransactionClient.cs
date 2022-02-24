using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Remoting;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Ucommerce.Headless.Domain;

namespace Ucommerce.Masterclass.Umbraco.Headless
{
    public class TransactionClient : UcommerceHttpClient, ITransactionClient
    {
        public async Task CreateShipment(string basketId, string paymentMethodId, string priceGroupId, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public async Task CreatePayment(string basketId, string cultureCode, string paymentMethodId, string priceGroupId,
            CancellationToken ct)
        {
            var client = await AuthorizeClient(ct);
            var errorMessage = string.Empty;

            using (var request = new HttpRequestMessage(new HttpMethod("POST"), "/api/v1/payments"))
            {
                if (request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {Auth.AccessToken}"))
                {
                    var dict = new Dictionary<string, string>
                    {
                        { "basketId", basketId },
                        { "cultureCode", cultureCode },
                        { "paymentMethodId", paymentMethodId },
                        { "priceGroupId", priceGroupId }
                    };

                    request.Content = new FormUrlEncodedContent(dict);
                    request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

                    var response = await client.SendAsync(request, ct);
                    if (response.IsSuccessStatusCode)
                        return;
                    errorMessage = await response.Content.ReadAsStringAsync();
                }
            }

            throw new ServerException($"Couldn't create payment. Message {errorMessage}");
        }

        public async Task<PaymentMethodsOutput> GetPaymentMethods(string cultureCode, string countryId, string priceGroupId, CancellationToken ct)
        {
            var client = await AuthorizeClient(ct);
            var errorMessage = string.Empty;

            using (var request = new HttpRequestMessage(new HttpMethod("GET"), $"/api/v1/payment-methods?cultureCode={cultureCode}&countryId={countryId}&priceGroupId={priceGroupId}"))
            {
                if (request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {Auth.AccessToken}"))
                {
                    var response = await client.SendAsync(request, ct);
                    if (response.IsSuccessStatusCode)
                        return await response.Content.ReadAsAsync<PaymentMethodsOutput>(ct);
                    errorMessage = await response.Content.ReadAsStringAsync();
                }
            }

            throw new ServerException($"Couldn't get payment methods. Message: {errorMessage}");
        }

        public async Task<ShippingMethodsOutput> GetShippingMethods(string cultureCode, string countryId, string priceGroupId, CancellationToken ct)
        {
            var client = await AuthorizeClient(ct);
            var errorMessage = string.Empty;
            using (var request = new HttpRequestMessage(new HttpMethod("GET"), $"/api/v1/shipping-methods?cultureCode={cultureCode}&countryId={countryId}&priceGroupId={priceGroupId}"))
            {
                if (request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {Auth.AccessToken}"))
                {
                    var response = await client.SendAsync(request, ct);
                    if (response.IsSuccessStatusCode)
                        return await response.Content.ReadAsAsync<ShippingMethodsOutput>(ct);
                    errorMessage = await response.Content.ReadAsStringAsync();
                }
            }

            throw new ServerException($"Couldn't get shipping methods. Message: {errorMessage}");
        }

        public async Task<CountriesOutput> GetCountries(CancellationToken ct)
        {
            var client = await AuthorizeClient(ct);
            var errorMessage = string.Empty;

            using (var request = new HttpRequestMessage(new HttpMethod("GET"), $"/api/v1/countries"))
            {
                if (request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {Auth.AccessToken}"))
                {
                    var response = await client.SendAsync(request, ct);
                    if (response.IsSuccessStatusCode)
                        return await response.Content.ReadAsAsync<CountriesOutput>(ct);
                    errorMessage = await response.Content.ReadAsStringAsync();
                }
            }

            throw new ServerException($"Couldn't get countries. Message: {errorMessage}");
        }

        public async Task UpdateOrderLineQuantity(string orderLineId, int quantity, CancellationToken ct)
        {
            var client = await AuthorizeClient(ct); 

            throw new NotImplementedException();

            throw new ServerException($"Couldn't update quantity for order line {orderLineId}.");
        }

        public async Task<GetBasketOutput> GetBasket(string basketId, CancellationToken ct)
        {
            var client = await AuthorizeClient(ct);
            var errorMessage = string.Empty;

            using (var request = new HttpRequestMessage(new HttpMethod("GET"), $"/api/v1/baskets/{basketId}"))
            {
                if (request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {Auth.AccessToken}"))
                {
                    var response = await client.SendAsync(request, ct);
                    if (response.IsSuccessStatusCode)
                        return await response.Content.ReadAsAsync<GetBasketOutput>(ct);
                    errorMessage = await response.Content.ReadAsStringAsync();
                }
            }

            throw new ServerException($"Couldn't get the basket with Id {basketId}. Message {errorMessage}");
        }

        public async Task EditBillingInformation(string basketId, string firstName, string lastName, string emailAddress, string phoneNumber,
            string mobilePhoneNumber, string company, string line1, string line2, string postalCode, string city,
            string attention, string state, string countryId, CancellationToken ct)
        {
            var client = await AuthorizeClient(ct);
            var errorMessage = string.Empty;

            using (var request = new HttpRequestMessage(new HttpMethod("POST"), $"/api/v1/baskets/{basketId}/billing-address"))
            {
                if (request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {Auth.AccessToken}"))
                {
                    var dict = new Dictionary<string, object>
                    {
                        {"attention", attention},
                        {"city", city},
                        {"companyName", company},
                        {"countryId", countryId},
                        {"email", emailAddress},
                        {"firstName", firstName},
                        {"lastName", lastName},
                        {"line1", line1},
                        {"line2", line2},
                        {"mobileNumber", mobilePhoneNumber},
                        {"phoneNumber", phoneNumber},
                        {"postalCode", postalCode},
                        {"state", state}
                    };

                    request.Content = new StringContent(JsonConvert.SerializeObject(dict));
                    request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

                    var response = await client.SendAsync(request, ct);
                    if (response.IsSuccessStatusCode)
                        return;
                    errorMessage = await response.Content.ReadAsStringAsync();
                }
            }

            throw new ServerException($"Couldn't update billing address. Message {errorMessage}");
        }

        public async Task EditShippingInformation(string basketId, string cultureCodeId, string priceGroupId, string shippingMethodId, string firstName, string lastName, string emailAddress, string phoneNumber,
            string mobilePhoneNumber, string company, string line1, string line2, string postalCode, string city,
            string attention, string state, string countryId, CancellationToken ct)
        {
            var client = await AuthorizeClient(ct);
            var errorMessage = string.Empty;

            using (var request = new HttpRequestMessage(new HttpMethod("POST"), $"/api/v1/baskets/{basketId}/shipping"))
            {
                if (request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {Auth.AccessToken}"))
                {
                    var dict = new Dictionary<string, object>
                    {
                        { "cultureCode", cultureCodeId },
                        { "priceGroupId", priceGroupId }, 
                        { "shippingMethodId", priceGroupId },
                        { 
                            "shippingAddress", new Dictionary<string, string>()
                            {
                                {"attention", attention},
                                {"city", city},
                                {"companyName", company},
                                {"countryId", countryId},
                                {"email", emailAddress},
                                {"firstName", firstName},
                                {"lastName", lastName},
                                {"line1", line1},
                                {"line2", line2},
                                {"mobileNumber", mobilePhoneNumber},
                                {"phoneNumber", phoneNumber},
                                {"postalCode", postalCode},
                                {"state", state}
                            }
                        }
                    };

                    request.Content = new StringContent(JsonConvert.SerializeObject(dict));
                    request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

                    var response = await client.SendAsync(request, ct);
                    if (response.IsSuccessStatusCode)
                        return;
                    errorMessage = await response.Content.ReadAsStringAsync();
                }
            }

            throw new ServerException($"Couldn't update shipping address. Message {errorMessage}");
        }

        public async Task<string> Checkout(string basketId, string cultureCode, string paymentMethodId, string priceGroupId, CancellationToken ct)
        {
            var client = await AuthorizeClient(ct);
            var errorMessage = string.Empty;

            using (var request = new HttpRequestMessage(new HttpMethod("POST"), $"/api/v1/payments"))
            {
                if (request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {Auth.AccessToken}"))
                {
                    var dict = new Dictionary<string, object>
                    {
                        {"basketId", basketId},
                        {"cultureCode", cultureCode},
                        {"paymentMethodId", paymentMethodId},
                        {"priceGroupId", priceGroupId},
                    };

                    request.Content = new StringContent(JsonConvert.SerializeObject(dict));
                    request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

                    var response = await client.SendAsync(request, ct);
                    if (response.IsSuccessStatusCode)
                        return await response.Content.ReadAsStringAsync();
                    errorMessage = await response.Content.ReadAsStringAsync();
                }
            }

            throw new ServerException($"Couldn't update billing address. Message {errorMessage}");
        }
    }
}