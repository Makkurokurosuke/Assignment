using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Models;

namespace Website.Services
{
    public interface IUserReviewApiService
    {
        Task<IEnumerable<UserReviewModel>> GetAll();

        Task Add(SubmitUserReviewModel reviewModel);

        Task<UserReviewModel> GetById(int reviewId);

    }
}