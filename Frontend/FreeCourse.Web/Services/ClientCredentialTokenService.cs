﻿using FreeCourse.Web.Models;
using FreeCourse.Web.Services.Interface;
using IdentityModel.AspNetCore.AccessTokenManagement;
using IdentityModel.Client;
using Microsoft.Extensions.Options;

namespace FreeCourse.Web.Services {
    public class ClientCredentialTokenService : IClientCredentialTokenService {
        private readonly ServiceApiSettings _serviceApiSettings;
        private readonly ClientSettings _clientSettings;

        //tokenı memory üzerinde tutmaya yarar
        private readonly IClientAccessTokenCache _clientAccessTokenCache;
        private readonly HttpClient _httpClient;

        public ClientCredentialTokenService(IOptions<ServiceApiSettings> serviceApiSettings, IOptions<ClientSettings> clientSettings, IClientAccessTokenCache clientAccessTokenCache, HttpClient httpClient) {
            _serviceApiSettings = serviceApiSettings.Value;
            _clientSettings = clientSettings.Value;
            _clientAccessTokenCache = clientAccessTokenCache;
            _httpClient = httpClient;
        }

        public async Task<string> GetToken() {

            //cache üzerinde tkoen var mı kontrol et 
            var currentToken = await _clientAccessTokenCache.GetAsync("WebClientToken",default);

            if (currentToken != null) {
                return currentToken.AccessToken;
            }

            var disco = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest {
                Address = _serviceApiSettings.IdentityBaseUri,
                Policy = new DiscoveryPolicy { RequireHttps = false }
            });

            if (disco.IsError) {
                throw disco.Exception;
            }

            //client için token alma amacıyla ıoptions pattern ile appsettings üzerindeki verileri oku
            var clientCredentialTokenRequest = new ClientCredentialsTokenRequest {
                ClientId = _clientSettings.WebClient.ClientId,
                ClientSecret = _clientSettings.WebClient.ClientSecret,
                Address = disco.TokenEndpoint
            };

            //client için yeni token al 
            var newToken = await _httpClient.RequestClientCredentialsTokenAsync(clientCredentialTokenRequest);

            if (newToken.IsError) {
                throw newToken.Exception;
            }

            //yeni token access  token ömrü kadae cachede tutulacak
            await _clientAccessTokenCache.SetAsync("WebClientToken", newToken.AccessToken, newToken.ExpiresIn,default);

            return newToken.AccessToken;
        }
    }
}