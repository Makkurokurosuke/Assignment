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
    public class UserReviewApiService : IUserReviewApiService
    {
        private readonly HttpClient httpClient;

        public UserReviewApiService(HttpClient client)
        {
            httpClient = client;
        }

        public async Task<IEnumerable<UserReviewModel>> GetAll()
        {
            List<UserReviewModel> result = null;
            httpClient.AddTokenToHeader();
            using (System.Threading.CancellationTokenSource cancelAfterDelay = new System.Threading.CancellationTokenSource(WebsiteUtility.GetWebAPITimeoutmillisecond()))
            {
                var response = await httpClient.GetAsync("api/v1/UserReview/GetAll");

                if (response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadAsAsync<List<UserReviewModel>>();
                }
                else
                {
                    //unexpected error
                    await WebsiteUtility.HandleWebAPIError(response);
                }
            }


            return result;
        }

        public async Task Add(SubmitUserReviewModel reviewModel)
        {
            httpClient.AddTokenToHeader();
            using (System.Threading.CancellationTokenSource cancelAfterDelay = new System.Threading.CancellationTokenSource(WebsiteUtility.GetWebAPITimeoutmillisecond()))
            {
                var response = await httpClient.PostAsJsonAsync("api/v1/UserReview/Add", reviewModel);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<SubmitUserReviewModel>();
                }
                else
                {
                    //unexpected error
                    await WebsiteUtility.HandleWebAPIError(response);
                }

            }

        }


        public async Task<UserReviewModel> GetById(int reviewId)
        {
            httpClient.AddTokenToHeader();
            UserReviewModel result = null;
            using (System.Threading.CancellationTokenSource cancelAfterDelay = new System.Threading.CancellationTokenSource(WebsiteUtility.GetWebAPITimeoutmillisecond()))
            {
                var response = await httpClient.GetAsync($"api/v1/UserReview/GetById/{reviewId}");

                if (response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadAsAsync<UserReviewModel>();
                }
                else
                {
                    //unexpected error
                    await WebsiteUtility.HandleWebAPIError(response);
                }
            }

            return result;
        }
    }
}
