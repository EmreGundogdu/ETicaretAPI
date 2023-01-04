using ETicaretAPI.Application.Abstractions.Token;
using ETicaretAPI.Application.DTOs;
using ETicaretAPI.Application.DTOs.Facebook;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Text.Json;
using P = ETicaretAPI.Domain.Entities.Identity;

namespace ETicaretAPI.Application.Features.Commands.AppUser.FacebookLogin
{
    public class FacebookLoginCommandHandler : IRequestHandler<FacebookLoginCommandRequest, FacebookLoginCommandResponse>
    {
        readonly UserManager<P.AppUser> userManager;
        readonly ITokenHandler tokenHandler;
        readonly HttpClient httpClient;

        public FacebookLoginCommandHandler(UserManager<P.AppUser> userManager, IHttpClientFactory httpClientFactory, ITokenHandler tokenHandler)
        {
            this.userManager = userManager;
            httpClient = httpClientFactory.CreateClient();
            this.tokenHandler = tokenHandler;
        }

        public async Task<FacebookLoginCommandResponse> Handle(FacebookLoginCommandRequest request, CancellationToken cancellationToken)
        {
            string accessTokenResponse = await httpClient.GetStringAsync($"https://graph.facebook.com/oauth/access_token?client_id=705703167811594&client_secret=2d0f8c3dfd2b61960d7f0dc413299f91&grant_type=client_credentials");
            FacebookAccessTokenResponse facebookAccessTokenResponseDto = JsonSerializer.Deserialize<FacebookAccessTokenResponse>(accessTokenResponse);
            string userAccessTokenValidation = await httpClient.GetStringAsync($"https://graph.facebook.com/debug_token?input_token={request.AuthToken}&access_token={facebookAccessTokenResponseDto.AccessToken}");
            FacebookUserAccessTokenValidation facebookUserAccessTokenValidation = JsonSerializer.Deserialize<FacebookUserAccessTokenValidation>(userAccessTokenValidation);
            if (facebookUserAccessTokenValidation.Data.IsValid)
            {
                string userInfoResponse = await httpClient.GetStringAsync($"https://graph.facebook.com/me?fields=email,name&access_token={request.AuthToken}");
                FacebookUserInfoResponse facebookUserInfoResponse = JsonSerializer.Deserialize<FacebookUserInfoResponse>(userInfoResponse);
                var info = new UserLoginInfo("FACEBOOK", facebookUserAccessTokenValidation.Data.UserId, "FACEBOOK");
                P.AppUser user = await userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
                bool result = user != null;
                if (user == null)
                {
                    user = await userManager.FindByEmailAsync(facebookUserInfoResponse.Email);
                    if (user == null)
                    {
                        user = new()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Email = facebookUserInfoResponse.Email,
                            UserName = facebookUserInfoResponse.Email,
                            NameSurname = facebookUserInfoResponse.Name
                        };
                        var identityResult = await userManager.CreateAsync(user);
                        result = identityResult.Succeeded;
                    }
                }
                if (result)
                {
                    await userManager.AddLoginAsync(user, info);
                    Token token = tokenHandler.CreateAccessToken(5);
                    return new() { Token = token };
                }
            }
            throw new Exception("Invalid External Authentication");
        }
    }
}
