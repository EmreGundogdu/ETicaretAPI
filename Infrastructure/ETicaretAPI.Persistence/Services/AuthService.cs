﻿using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Application.Abstractions.Token;
using ETicaretAPI.Application.DTOs;
using ETicaretAPI.Application.DTOs.Facebook;
using ETicaretAPI.Application.Exceptions;
using ETicaretAPI.Application.Helpers;
using ETicaretAPI.Domain.Entities.Identity;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Text.Json;

namespace ETicaretAPI.Persistence.Services
{
    public class AuthService : IAuthService
    {
        readonly HttpClient httpClient;
        readonly IConfiguration configuration;
        readonly UserManager<AppUser> userManager;
        readonly SignInManager<AppUser> signInManager;
        readonly ITokenHandler tokenHandler;
        readonly IUserService userService;
        readonly IMailService mailService;
        public AuthService(IHttpClientFactory httpClientFactory, IConfiguration configuration, UserManager<AppUser> userManager, ITokenHandler tokenHandler, SignInManager<AppUser> signInManager, IUserService userService, IMailService mailService)
        {
            httpClient = httpClientFactory.CreateClient();
            this.configuration = configuration;
            this.userManager = userManager;
            this.tokenHandler = tokenHandler;
            this.signInManager = signInManager;
            this.userService = userService;
            this.mailService = mailService;
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
                Token token = tokenHandler.CreateAccessToken(accessTokenLifeTime, user);
                await userService.UpdateRefreshTokenAsync(token.RefreshToken, user, token.Expiration, 10);
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
            try
            {
                var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);
                var info = new UserLoginInfo("GOOGLE", payload.Subject, "GOOGLE");
                AppUser user = await userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
                return await CreateUserExternalAsync(user, payload.Email, payload.Name, info, accessTokenLifeTime);
            }
            catch
            {
                throw;
            }

        }

        public async Task<Token> LoginAsync(string userNameOrEmail, string password, int accessTokenLifeTime)
        {
            AppUser user = await userManager.FindByNameAsync(userNameOrEmail);
            if (user == null)
                user = await userManager.FindByEmailAsync(userNameOrEmail);
            if (user == null)
                throw new NotFoundUserException();

            SignInResult signInResult = await signInManager.CheckPasswordSignInAsync(user, password, false);
            if (signInResult.Succeeded)
            {
                Token token = tokenHandler.CreateAccessToken(accessTokenLifeTime, user);
                await userService.UpdateRefreshTokenAsync(token.RefreshToken, user, token.Expiration, 15);
                return token;
            }
            else
            {
                throw new AuthenticationErrorException();
            }
        }

        public async Task PasswordResetAsync(string email)
        {
            AppUser appUser = await userManager.FindByEmailAsync(email);
            if (appUser != null)
            {
                string resetToken = await userManager.GeneratePasswordResetTokenAsync(appUser);
                resetToken = resetToken.UrlEncode();
                await mailService.SendPasswordResetMailAsync(email, appUser.Id, resetToken);
            }
        }

        public async Task<Token> RefreshTokenLoginAsync(string refreshToken)
        {
            AppUser? appUser = await userManager.Users.FirstOrDefaultAsync(x => x.RefreshToken == refreshToken);
            if (appUser != null && appUser?.RefreshTokenEndDate > DateTime.UtcNow)
            {
                Token token = tokenHandler.CreateAccessToken(15, appUser);
                await userService.UpdateRefreshTokenAsync(token.RefreshToken, appUser, token.Expiration, 300);
                return token;
            }
            else
            {
                throw new NotFoundUserException();
            }
        }

        public async Task<bool> VerifyResetTokenAsync(string resetToken, string userId)
        {
            AppUser appUser = await userManager.FindByIdAsync(userId);
            if (appUser != null)
            {
                resetToken = resetToken.UrlDecode();
                return await userManager.VerifyUserTokenAsync(appUser, userManager.Options.Tokens.PasswordResetTokenProvider, "ResetPassword", resetToken);
            }
            return false;
        }
    }
}
