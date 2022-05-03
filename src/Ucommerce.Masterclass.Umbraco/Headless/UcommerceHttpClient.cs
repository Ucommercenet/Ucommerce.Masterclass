using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using MC_Headless.Headless.Models;

namespace Ucommerce.Masterclass.Umbraco.Headless
{
    /// <summary>
    /// Authorize a HttpClient for Ucommerce Headless Api's
    /// </summary>
    /// <remarks>
    /// HttpClient is intended to be instantiated once per application, rather than per-use.
    /// https://docs.microsoft.com/en-us/dotnet/api/system.net.http.httpclient?view=netframework-4.8
    /// </remarks>
    public abstract class UcommerceHttpClient : IDisposable
    {
        private static readonly string _clientId = "eb446cf5-9f7d-4eaf-a4ba-26c5b64e88b6";
        private static readonly string _clientSecret = "EAAE6EE4-A90D-4AE6-85BC-C9BBAA1E3FCBpkPdRblQH1i389c7wJG4gsLcpTDKouTLPZ6630T8hXDa7uTBvuxUYC1QEQn6cLGfdNCKTEj9Gu7SSe2XKRFsvL9Es6INsCO7OTUCuI8c55uKhtNz58KhFzG0DpjW2C2BkN";
        private static readonly string _redirectUrl = "http://localhost";
        private static readonly string _apiUrl = "https://localhost:44340";

        protected static AuthenticationModel Auth = null;

        private static object _locker = new object();

        //Declared as volatile to ensure that assignment to the static variable is complete before releasing the lock.
        private static volatile HttpClient _client;

        private static HttpClient Client
        {
            get
            {
                if (_client == null)
                {
                    lock (_locker)
                    {
                        if (_client == null)
                        {
                            HttpClientHandler handler = new HttpClientHandler();
                            handler.AllowAutoRedirect = false;
                            _client = new HttpClient(handler);
                            _client.BaseAddress = new Uri(_apiUrl);
                        }
                    }
                }

                return _client;
            }
        }

        protected static async Task<HttpClient> AuthorizeClient(CancellationToken ct)
        {
            if (Auth == null)
            {
                await AuthorizeAsync(ct);
            }

            if (Auth != null && Auth.ExpiresAt < DateTime.UtcNow.AddMinutes(1))
            {
                try
                {
                    await RefreshToken(ct);
                }
                catch (Exception e) //RefreshTokenExpiredException
                {
                    await AuthorizeAsync(ct);
                }
            }

            return Client;
        }

        private static async Task RefreshToken(CancellationToken ct)
        {
            if (Auth == null) return;

            using (var request = new HttpRequestMessage(new HttpMethod("POST"), "/api/v1/oauth/token"))
            {
                if (request.Headers.TryAddWithoutValidation("Authorization", GenerateBasicAuthorizationHeaderValue(_clientId, _clientSecret)))
                {
                    var dict = new Dictionary<string, string>
                     {
                         { "refresh_token", Auth.RefreshToken },
                         { "grant_type", "refresh_token" }
                     };
                    request.Content = new FormUrlEncodedContent(dict);
                    request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

                    var response = await Client.SendAsync(request, ct);

                    if (response.IsSuccessStatusCode)
                    {
                        Auth = await response.Content.ReadAsAsync<AuthenticationModel>(ct);
                        Auth.ExpiresAt = DateTime.UtcNow.AddSeconds(Auth.ExpiresIn);
                        return;
                    }
                }
                throw new SecurityException($"Unable to refresh the token.");
            }
        }

        private static async Task AuthorizeAsync(CancellationToken ct)
        {
            if (Auth != null) return;

            string authorizationCode = null;

            var connectResponse = await Client.GetAsync(
                $"/api/v1/oauth/connect?client_id={_clientId}&redirect_uri={_redirectUrl}&response_type=code", ct);
            if (connectResponse.StatusCode.Equals(HttpStatusCode.Found))
            {
                var targetUrlUri = new Uri(connectResponse.Headers.Location.OriginalString);
                authorizationCode = HttpUtility.ParseQueryString(targetUrlUri.Query).Get("code");
            }

            if (authorizationCode != null)
            {
                using (var request = new HttpRequestMessage(new HttpMethod("POST"), "/api/v1/oauth/token"))
                {
                    if (request.Headers.TryAddWithoutValidation("Authorization", GenerateBasicAuthorizationHeaderValue(_clientId, _clientSecret)))
                    {

                        var dict = new Dictionary<string, string>
                         {
                             { "code", authorizationCode },
                             { "grant_type", "authorization_code" },
                             { "redirect_uri", _redirectUrl }
                         };
                        request.Content = new FormUrlEncodedContent(dict);
                        request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded");

                        var response = await Client.SendAsync(request, ct);
                        if (response.IsSuccessStatusCode)
                        {
                            Auth = await response.Content.ReadAsAsync<AuthenticationModel>(ct);
                            Auth.ExpiresAt = DateTime.UtcNow.AddSeconds(Auth.ExpiresIn);
                            return;
                        }
                    }
                }
            }

            throw new SecurityException($"Unable to authorize headless APIs.");
        }

        private static string GenerateBasicAuthorizationHeaderValue(string clientId, string clientSecret)
        {
            string credentials = $"{clientId}:{clientSecret}";
            byte[] credentialsByteData = Encoding.GetEncoding("iso-8859-1").GetBytes(credentials);
            string base64Credentials = Convert.ToBase64String(credentialsByteData);

            return $"Basic {base64Credentials}";
        }

        public void Dispose()
        {
            if (_client != null)
            {
                _client.Dispose();
                _client = null;
            }
        }
    }
}