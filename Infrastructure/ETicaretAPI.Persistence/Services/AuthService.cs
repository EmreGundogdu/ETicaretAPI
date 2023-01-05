﻿using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Application.Abstractions.Token;
using ETicaretAPI.Application.DTOs;
using ETicaretAPI.Application.DTOs.Facebook;
using ETicaretAPI.Domain.Entities.Identity;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace ETicaretAPI.Persistence.Services
{
    public class AuthService : IAuthService
    {
        readonly HttpClient httpClient;
        readonly IConfiguration configuration;
        readonly UserManager<AppUser> userManager;
        readonly ITokenHandler tokenHandler;
        public AuthService(IHttpClientFactory httpClientFactory, IConfiguration configuration, UserManager<AppUser> userManager, ITokenHandler tokenHandler)
        {
            httpClient = httpClientFactory.CreateClient();
            this.configuration = configuration;
            this.userManager = userManager;
            this.tokenHandler = tokenHandler;
        }
        public async Task<Token> CreateUserExternalAsync(AppUser user, string email, string name, UserLoginInfo info, int accessTokenLifeTime)
        {
            bool result = user != null;
            if (user == null)
            {
                user = await userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    user = new()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Email = email,
                        UserName = email,
                        NameSurname = name
                    };
                    var identityResult = await userManager.CreateAsync(user);
                    result = identityResult.Succeeded;
                }
            }
            if (result)
            {
                await userManager.AddLoginAsync(user, info);
                Token token = tokenHandler.CreateAccessToken(accessTokenLifeTime);
                return token;
            }
            throw new Exception("Invalid External Authentication");
        }

        public async Task<Token> FacebookLoginAsync(string authToken, int accessTokenLifeTime)
        {
            string accessTokenResponse = await httpClient.GetStringAsync($"https://graph.facebook.com/oauth/access_token?client_id={configuration["ExternalLoginSettings:Facebook:ClientId"]}&client_secret={configuration["ExternalLoginSettings:Facebook:ClientSecret"]}&grant_type=client_credentials");
            FacebookAccessTokenResponse? facebookAccessTokenResponseDto = JsonSerializer.Deserialize<FacebookAccessTokenResponse>(accessTokenResponse);
            string userAccessTokenValidation = await httpClient.GetStringAsync($"https://graph.facebook.com/debug_token?input_token={authToken}&access_token={facebookAccessTokenResponseDto?.AccessToken}");
            FacebookUserAccessTokenValidation? facebookUserAccessTokenValidation = JsonSerializer.Deserialize<FacebookUserAccessTokenValidation>(userAccessTokenValidation);
            if (facebookUserAccessTokenValidation?.Data.IsValid != null)
            {
                string userInfoResponse = await httpClient.GetStringAsync($"https://graph.facebook.com/me?fields=email,name&access_token={authToken}");
                FacebookUserInfoResponse? facebookUserInfoResponse = JsonSerializer.Deserialize<FacebookUserInfoResponse>(userInfoResponse);
                var info = new UserLoginInfo("FACEBOOK", facebookUserAccessTokenValidation.Data.UserId, "FACEBOOK");
                AppUser user = await userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
                return await CreateUserExternalAsync(user, facebookUserInfoResponse.Email, facebookUserInfoResponse.Name, info, accessTokenLifeTime);
            }
            throw new Exception("Invalid External Authentication");
        }

        public async Task<Token> GoogleLoginAsync(string idToken, int accessTokenLifeTime)
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string> { $"{configuration["ExternalLoginSettings:Google:ClientId"]}" }
            };
            var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);
            var info = new UserLoginInfo("GOOGLE", payload.Subject, "GOOGLE");
            AppUser user = await userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
            return await CreateUserExternalAsync(user, payload.Email, payload.Name, info, accessTokenLifeTime);
        }

        public Task LoginAsync()
        {
            throw new NotImplementedException();
        }
    }
}
