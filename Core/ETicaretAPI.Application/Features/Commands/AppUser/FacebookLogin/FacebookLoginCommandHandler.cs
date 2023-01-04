using ETicaretAPI.Application.Abstractions.Token;
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
            return new() { };
        }
    }
}
