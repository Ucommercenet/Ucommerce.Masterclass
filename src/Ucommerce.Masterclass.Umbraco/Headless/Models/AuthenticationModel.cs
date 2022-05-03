using System;
using Newtonsoft.Json;

namespace MC_Headless.Headless.Models
{
    public class AuthenticationModel
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonProperty("redirect_uri")]
        public string RedirectUri { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        public DateTime ExpiresAt { get; set; }

    }
}