using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomHelper.Authentication.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;

namespace CustomHelper.Authentication.NewFolder
{
    public class SignInKeys : ISignInKeys
    {
        public async Task<IEnumerable<SecurityKey>> GetSigninKeys(string authority)
        {
            HttpClient httpClient = new HttpClient();
            var metaDataRequest = new HttpRequestMessage(HttpMethod.Get, $"{authority}/.well-known/openid-configuration");
            var metaDataResponse = await httpClient.SendAsync(metaDataRequest);

            string content = await metaDataResponse.Content.ReadAsStringAsync();
            var payload = JObject.Parse(content);
            string jwksUri = payload.Value<string>("jwks_uri");

            var keysRequest = new HttpRequestMessage(HttpMethod.Get, jwksUri);
            var keysResponse = await httpClient.SendAsync(keysRequest);
            var keysPayload = await keysResponse.Content.ReadAsStringAsync();
            var signInKeys = new JsonWebKeySet(keysPayload).Keys;

            return signInKeys;
        }
    }
}
