using Microsoft.AspNet.Identity;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace IdealHires.Web.TokenProvider
{
    public class JwtTokenProvider
    {
        private static string _tokenUri;

        public JwtTokenProvider() { }

        public static JwtTokenProvider Create(string tokenUri)
        {
            _tokenUri = tokenUri;
            return new JwtTokenProvider();
        }

        public async Task<string> GetTokenAsync(string username, string password) //, string clientId, string deviceId
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_tokenUri);
                client.DefaultRequestHeaders.Accept
                    .Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var content = new FormUrlEncodedContent(new[]
                {
                        new KeyValuePair<string, string>("username", username),
                        new KeyValuePair<string, string>("password", password),
                        new KeyValuePair<string, string>("grant_type", "password")                        
                    });
                var response = await client.PostAsync(string.Empty, content);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    return null;
                }
            }
        }

        private JObject DecodePayload(string token)
        {
            var parts = token.Split('.');
            var payload = parts[1];

            var payloadJson = Encoding.UTF8.GetString(Base64UrlDecode(payload));
            return JObject.Parse(payloadJson);
        }

        public ClaimsIdentity CreateIdentity(bool isAuthenticated, string userName, string token)
        {
            JObject obj = JObject.Parse(token);
            var accessToken = obj.SelectToken("access_token").ToString();
            var tokenType = obj.SelectToken("token_type").ToString();
            // decode payload
            dynamic payload = DecodePayload(token);
            string userId = payload.nameid;
            string firstName = payload.FirstName;
            string lastName = payload.LastName;
            string userType = payload.UserType;
            string[] roles = payload.role.ToObject(typeof(string[]));

            var jwtIdentity = new ClaimsIdentity(new JwtIdentity(
                isAuthenticated, userName, DefaultAuthenticationTypes.ApplicationCookie
                    ));

            jwtIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, userId));
            jwtIdentity.AddClaim(new Claim("AccessToken", accessToken, ClaimValueTypes.String));
            jwtIdentity.AddClaim(new Claim("TokenType", tokenType, ClaimValueTypes.String));
            jwtIdentity.AddClaim(new Claim("FirstName", firstName, ClaimValueTypes.String));
            jwtIdentity.AddClaim(new Claim("LastName", lastName, ClaimValueTypes.String));
            jwtIdentity.AddClaim(new Claim("UserType", userType, ClaimValueTypes.String));
            //jwtIdentity.AddClaim(ClaimsProviderExtension.GetClaims())

            foreach (var role in roles)
            {
                jwtIdentity.AddClaim(new Claim(ClaimTypes.Role, role));
            }

            return jwtIdentity;
        }

        private byte[] Base64UrlDecode(string input)
        {
            var output = input;
            output = output.Replace('-', '+'); // 62nd char of encoding
            output = output.Replace('_', '/'); // 63rd char of encoding
            switch (output.Length % 4) // Pad with trailing '='s
            {
                case 0: break; // No pad chars in this case
                case 2: output += "=="; break; // Two pad chars
                case 3: output += "="; break; // One pad char
                default: throw new System.Exception("Illegal base64url string!");
            }
            var converted = Convert.FromBase64String(output); // Standard base64 decoder
            return converted;
        }

        public class JwtIdentity : IIdentity
        {
            private bool _isAuthenticated;
            private string _name;
            private string _authenticationType;

            public JwtIdentity() { }
            public JwtIdentity(bool isAuthenticated, string name, string authenticationType)
            {
                _isAuthenticated = isAuthenticated;
                _name = name;
                _authenticationType = authenticationType;
            }
            public string AuthenticationType
            {
                get
                {
                    return _authenticationType;
                }
            }

            public bool IsAuthenticated
            {
                get
                {
                    return _isAuthenticated;
                }
            }

            public string Name
            {
                get
                {
                    return _name;
                }
            }
        }
    }
}