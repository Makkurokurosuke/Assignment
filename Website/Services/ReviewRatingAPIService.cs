using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Common.Models;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Website.Services
{
    public class ReviewRatingApiService : IReviewRatingApiService
    {
        private readonly HttpClient httpClient;

        public ReviewRatingApiService(HttpClient client)
        {
            httpClient = client;
        }

        public async Task<IEnumerable<ReviewRatingTypeModel>> GetAll()
        {
            List<ReviewRatingTypeModel> result = null;
            httpClient.AddTokenToHeader();

            using (System.Threading.CancellationTokenSource cancelAfterDelay = new System.Threading.CancellationTokenSource(WebsiteUtility.GetWebAPITimeoutmillisecond()))
            {
                var response = await httpClient.GetAsync("api/v1/ReviewRating/GetAll");
                if (response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadAsAsync<List<ReviewRatingTypeModel>>();
                }
                else if (response.StatusCode != System.Net.HttpStatusCode.Unauthorized)
                {
                    //unexpected error
                    await WebsiteUtility.HandleWebAPIError(response);

                }
            }

            return result;
        }


    }
}
