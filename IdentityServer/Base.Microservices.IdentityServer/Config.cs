﻿// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;

namespace Base.Microservices.IdentityServer {
    public static class Config {
        public static IEnumerable<ApiResource> ApiResources => new ApiResource[]
        {
            //Resource tanımlaması ve hangi scope ile erişimi olacağının belirtilmesi
            new ApiResource("resource_catalog"){Scopes={"catalog_fullpermission"}},
               new ApiResource("photo_stock_catalog"){Scopes={"photo_stock_fullpermission"}},
                  new ApiResource("resource_basket"){Scopes={"basket_fullpermission"}},
                     new ApiResource("resource_discount"){Scopes={"discount_fullpermission"}},
               new ApiResource(IdentityServerConstants.LocalApi.ScopeName)
        };

        public static IEnumerable<IdentityResource> IdentityResources =>
                   //cleintlerin kullanıcının hangi bilgilerine erişebileceği bilgisi tanımlanır 
                   new IdentityResource[]
                   {
                       new IdentityResources.Email(),
                       new IdentityResources.OpenId(),
                       new IdentityResources.Profile(),
                       new IdentityResource(){ Name="roles", DisplayName="Roles", Description="Kullanıcı rolleri", UserClaims=new []{ "role"} }
                   };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                //Scope tanımlaması 
                new ApiScope("catalog_fullpermission","Catalog API için full erişim"),
                new ApiScope("photo_stock_fullpermission","Photo Stock API için full erişim"),
                    new ApiScope("basket_fullpermission","Basket API için full erişim"),
                      new ApiScope("discount_fullpermission","Discount API için full erişim"),
                 //Identity server kendine ait scope tanımı
                new ApiScope(IdentityServerConstants.LocalApi.ScopeName)
            };

        public static IEnumerable<Client> Clients =>
            //Client tanımlama işlemleri 
            new Client[]
            {
                new Client
                {
                   ClientName="Asp.Net Core MVC",
                    ClientId="WebMvcClient",
                    ClientSecrets= {new Secret("secret".Sha256())},
                    AllowedGrantTypes= GrantTypes.ClientCredentials,
                    //izin verilen scope bilgileri
                    //client id ve secret ile istek atılınca hangi api'lere erişim yapılabileceği bilgisi 
                    AllowedScopes={ "catalog_fullpermission","photo_stock_fullpermission",IdentityServerConstants.LocalApi.ScopeName }
                },
                   new Client
                {
                   ClientName="Asp.Net Core MVC",
                    ClientId="WebMvcClientForUser",
                    AllowOfflineAccess=true,
                    ClientSecrets= {new Secret("secret".Sha256())},
                    AllowedGrantTypes= GrantTypes.ResourceOwnerPassword,
                    AllowedScopes={ "basket_fullpermission", "discount_fullpermission", IdentityServerConstants.StandardScopes.Email, IdentityServerConstants.StandardScopes.OpenId,IdentityServerConstants.StandardScopes.Profile, IdentityServerConstants.StandardScopes.OfflineAccess, IdentityServerConstants.LocalApi.ScopeName,"roles" },
                    //OfflineAccess refresh token işlemini sağlar 
                    //token ömrü 1 saat
                    //refresh token ömrü 60 gün vw 61. gün absolute olacak yani yeniden refresh token alıunacak ömrü uzatılmayacak 
                    //refresh token yeniden kullanılabilir reuse 
                    AccessTokenLifetime=1*60*60,
                    RefreshTokenExpiration=TokenExpiration.Absolute,
                    AbsoluteRefreshTokenLifetime= (int) (DateTime.Now.AddDays(60)- DateTime.Now).TotalSeconds,
                    RefreshTokenUsage= TokenUsage.ReUse
                }
            };
    }
}