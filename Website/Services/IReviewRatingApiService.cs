using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Models;

namespace Website.Services
{
    public interface IReviewRatingApiService
    {
        Task<IEnumerable<ReviewRatingTypeModel>> GetAll();
    }
}