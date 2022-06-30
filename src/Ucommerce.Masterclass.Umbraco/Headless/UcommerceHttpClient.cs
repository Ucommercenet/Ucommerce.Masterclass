using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ucommerce.Masterclass.Umbraco.Headless.Models;

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
        private static readonly string _clientId = "clientId";
        private static readonly string _clientSecret = "secret";
        private static readonly string _redirectUrl = "http://localhost";
        private static readonly string _apiUrl = "https://localhost:PORT";

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
                            var handler = new HttpClientHandler();
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
            throw new NotImplementedException();
        }

        private static async Task AuthorizeAsync(CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        private static string GenerateBasicAuthorizationHeaderValue(string clientId, string clientSecret)
        {
            var credentials = $"{clientId}:{clientSecret}";
            var credentialsByteData = Encoding.GetEncoding("iso-8859-1").GetBytes(credentials);
            var base64Credentials = Convert.ToBase64String(credentialsByteData);

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