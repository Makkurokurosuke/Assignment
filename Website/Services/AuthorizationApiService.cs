using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Common.Models;
using Common.Utility;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Website.Services
{
    public class AuthorizationApiService : IAuthorizationApiService
    {
        private readonly HttpClient httpClient;

        public AuthorizationApiService(HttpClient client)
        {
            httpClient = client;
        }

        public async Task<string> LogIn(UserModel userModel)
        {
            string token = "";
            using (System.Threading.CancellationTokenSource cancelAfterDelay = new System.Threading.CancellationTokenSource(WebsiteUtility.GetWebAPITimeoutmillisecond()))
            {
                HttpResponseMessage response = await httpClient.PostAsJsonAsync("api/v1/Account/Login", userModel, cancelAfterDelay.Token);

                if (response.IsSuccessStatusCode)
                {
                    JObject jsonResult = await response.Content.ReadAsAsync<JObject>();

                    JValue tokenJValue = (JValue)jsonResult["token"];
                    token = (string)tokenJValue.Value;
                    JValue tokenJValueUserId = (JValue)jsonResult["userId"];
                    int userId = (int)(long)tokenJValueUserId.Value;

                    if (!String.IsNullOrEmpty(token) && userId != 0)
                    {

                        JWTHelper.SetJWTUserToken(token);

                        JValue tokenJValueExpiry = (JValue)jsonResult["expiresIn"];
                        int expiresInSec = (int)(long)tokenJValueExpiry.Value;
                        DateTime expiryTime = DateTime.Now.AddSeconds(expiresInSec);

                        JWTHelper.SetTokenExpiryDateTime(expiryTime);
                        JWTHelper.SetJWTUserId(userId);

                    }

                }
                else if (response.StatusCode != System.Net.HttpStatusCode.Unauthorized)
                {

                    //unexpected error
                    await WebsiteUtility.HandleWebAPIError(response);


                }
            }


            return token;
        }

    }
}
