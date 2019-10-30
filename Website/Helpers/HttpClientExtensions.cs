using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Website
{
    public static class HttpClientExtensions
    {
        public static HttpClient AddTokenToHeader(this HttpClient httpClient)
        {
            string JWToken = JWTHelper.GetJWTUserToken();

            httpClient.DefaultRequestHeaders.Authorization =
               new AuthenticationHeaderValue("Bearer", JWToken);

            return httpClient;
        }


    }


}
