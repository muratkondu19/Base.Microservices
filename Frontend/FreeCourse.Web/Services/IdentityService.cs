using FreeCourse.Shared.Dtos;
using FreeCourse.Web.Models;
using FreeCourse.Web.Services.Interface;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Globalization;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace FreeCourse.Web.Services {
    public class IdentityService : IIdentityService {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor; //cookie erişim için kullanılıyor
        private readonly ClientSettings _clientSettings;
        private readonly ServiceApiSettings _serviceApiSettings;

        public IdentityService(HttpClient client, IHttpContextAccessor httpContextAccessor, IOptions<ClientSettings> clientSettings, IOptions<ServiceApiSettings> serviceApiSettings) {
            _httpClient = client;
            _httpContextAccessor = httpContextAccessor;
            _clientSettings = clientSettings.Value;
            _serviceApiSettings = serviceApiSettings.Value;
        }

        public async Task<TokenResponse> GetAccessTokenByRefreshToken() {

            //identity üzerinde yer alan url adreslerine erişeceğiz / token endpoint, userinfo endpoint buradaki response içerisinde mevcut 
            var disco = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest {
                Address = _serviceApiSettings.IdentityBaseUri,
                //https ile istek atmaması için işaretliyoruz
                Policy = new DiscoveryPolicy { RequireHttps = false }
            });

            if (disco.IsError) {
                throw disco.Exception;
            }

            var refreshToken = await _httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);

            RefreshTokenRequest refreshTokenRequest = new() {
                ClientId = _clientSettings.WebClientForUser.ClientId,
                ClientSecret = _clientSettings.WebClientForUser.ClientSecret,
                RefreshToken = refreshToken,
                Address = disco.TokenEndpoint
            };

            var token = await _httpClient.RequestRefreshTokenAsync(refreshTokenRequest);

            if (token.IsError) {
                return null;
            }

            var authenticationTokens = new List<AuthenticationToken>()
            {
                new AuthenticationToken{ Name=OpenIdConnectParameterNames.AccessToken,Value=token.AccessToken},
                   new AuthenticationToken{ Name=OpenIdConnectParameterNames.RefreshToken,Value=token.RefreshToken},

                      new AuthenticationToken{ Name=OpenIdConnectParameterNames.ExpiresIn,Value= DateTime.Now.AddSeconds(token.ExpiresIn).ToString("o",CultureInfo.InvariantCulture)}
            };

            var authenticationResult = await _httpContextAccessor.HttpContext.AuthenticateAsync();

            var properties = authenticationResult.Properties;
            properties.StoreTokens(authenticationTokens);

            await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, authenticationResult.Principal, properties);

            return token;
        }

        public async Task RevokeRefreshToken() {
            var disco = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest {
                Address = _serviceApiSettings.IdentityBaseUri,
                Policy = new DiscoveryPolicy { RequireHttps = false }
            });

            if (disco.IsError) {
                throw disco.Exception;
            }
            //cookieden refresh token alınır
            var refreshToken = await _httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);

            TokenRevocationRequest tokenRevocationRequest = new() {
                ClientId = _clientSettings.WebClientForUser.ClientId,
                ClientSecret = _clientSettings.WebClientForUser.ClientSecret,
                Address = disco.RevocationEndpoint,
                Token = refreshToken,
                TokenTypeHint = "refresh_token"
            };

            //refresh token revoke edilir access token refresh edilmesi uygun değildir 
            await _httpClient.RevokeTokenAsync(tokenRevocationRequest);
        }

        public async Task<Response<bool>> SignIn(SigninInput signinInput) {

            //identity üzerinde yer alan url adreslerine erişeceğiz / token endpoint, userinfo endpoint buradaki response içerisinde mevcut 
            var disco = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest {
                Address = _serviceApiSettings.IdentityBaseUri,
                Policy = new DiscoveryPolicy { RequireHttps = false }
            });

            if (disco.IsError) {
                throw disco.Exception;
            }

            //password grand tpye prop değerlerni vermek için ayarlama yapıyoruz 
            var passwordTokenRequest = new PasswordTokenRequest {
                ClientId = _clientSettings.WebClientForUser.ClientId,
                ClientSecret = _clientSettings.WebClientForUser.ClientSecret,
                UserName = signinInput.Email,
                Password = signinInput.Password,
                Address = disco.TokenEndpoint
            };

            var token = await _httpClient.RequestPasswordTokenAsync(passwordTokenRequest);

            if (token.IsError) {
                //token içerisinde gelen http response okunur 
                var responseContent = await token.HttpResponse.Content.ReadAsStringAsync();

                var errorDto = JsonSerializer.Deserialize<ErrorDto>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                //gelen hata yakalanarak response olarak verilir 
                return Response<bool>.Fail(errorDto.Errors, 400);
            }


            //tokenı başarılı şekilde aldıktan sonra user info endpoint istek atılarak user bilgileri alınır 
            var userInfoRequest = new UserInfoRequest {
                Token = token.AccessToken,
                Address = disco.UserInfoEndpoint
            };

            var userInfo = await _httpClient.GetUserInfoAsync(userInfoRequest);

            if (userInfo.IsError) {
                throw userInfo.Exception;
            }

            //user bilgilerini ve tokenı cookie ekleme 
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(userInfo.Claims, CookieAuthenticationDefaults.AuthenticationScheme, "name", "role");
            //username name üzerinden role ler ise claim üzerinde role değeri üzerinden alınacağını belirtiyoruz 


            //cookie verilen claim nesneleri üzerinden okunacak 
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            //access token ve refresh token tutma işlemi 
            var authenticationProperties = new AuthenticationProperties();

            authenticationProperties.StoreTokens(new List<AuthenticationToken>()
            {
                new AuthenticationToken{ Name=OpenIdConnectParameterNames.AccessToken,Value=token.AccessToken},
                   new AuthenticationToken{ Name=OpenIdConnectParameterNames.RefreshToken,Value=token.RefreshToken},
                   //culture bilgisine bağlı kalmadan expire tarihini  yazması için ayarlıyoruz 
                      new AuthenticationToken{ Name=OpenIdConnectParameterNames.ExpiresIn,Value= DateTime.Now.AddSeconds(token.ExpiresIn).ToString("o",CultureInfo.InvariantCulture)}
            });

            //is rememmber true ise cookie kalıcı olması ayarı
            authenticationProperties.IsPersistent = signinInput.IsRemember;

            await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, authenticationProperties);

            return Response<bool>.Success(200);
        }
    }
}
