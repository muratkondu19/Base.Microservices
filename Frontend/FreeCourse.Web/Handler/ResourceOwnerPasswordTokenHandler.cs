using FreeCourse.Web.Exceptions;
using FreeCourse.Web.Services.Interface;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace FreeCourse.Web.Handler {
    public class ResourceOwnerPasswordTokenHandler : DelegatingHandler {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IIdentityService _identityService;
        private readonly ILogger<ResourceOwnerPasswordTokenHandler> _logger;

        public ResourceOwnerPasswordTokenHandler(IHttpContextAccessor httpContextAccessor, IIdentityService identityService, ILogger<ResourceOwnerPasswordTokenHandler> logger) {
            _httpContextAccessor = httpContextAccessor;
            _identityService = identityService;
            _logger = logger;
        }

        //her istek başladığında bu metod araya girecek ve çalışacak
        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) {
            var accessToken = await _httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);

            //access token req headera eklenir
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            //req gönderimi
            var response = await base.SendAsync(request, cancellationToken);

            //access token ömrü kontrolü
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized) {
                //yeni access token al
                var tokenResponse = await _identityService.GetAccessTokenByRefreshToken();

                if (tokenResponse != null) {
                    //yeni alınan access tokenı headera ekle
                    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenResponse.AccessToken);

                    //bu response nesnesi de 401 alıyorsa refresh token da geçersiz olmuş oluyor 
                    response = await base.SendAsync(request, cancellationToken);
                }
            }

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized) {
                //kullanıcıyı login ekranına döndürme işlemi için hata fırlatılır buradan yönlendirme yapılamıyor
                //custom exp sınıfı oluşturduk ve ekledik
                throw new UnAuthorizeException();
            }

            return response;
        }
    }
}
