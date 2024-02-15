using FreeCourse.Shared.Dtos;
using FreeCourse.Web.Models;
using IdentityModel.Client;

namespace FreeCourse.Web.Services.Interface {
    public interface IIdentityService {
        Task<Response<bool>> SignIn(SigninInput signinInput);

        //token response identitymodel sınıfından gelmektedir ve token verilerini taşımaktadır
        Task<TokenResponse> GetAccessTokenByRefreshToken();

        //logout işlemlerinde kullanılacak
        Task RevokeRefreshToken();
    }
}
