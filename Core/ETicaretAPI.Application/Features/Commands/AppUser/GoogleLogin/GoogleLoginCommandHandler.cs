using ETicaretAPI.Application.Abstractions.Token;
using ETicaretAPI.Application.DTOs;
using Google.Apis.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;
using P = ETicaretAPI.Domain.Entities.Identity;


namespace ETicaretAPI.Application.Features.Commands.AppUser.GoogleLogin
{
    public class GoogleLoginCommandHandler : IRequestHandler<GoogleLoginCommandRequest, GoogleLoginCommandResponse>
    {
        readonly UserManager<P.AppUser> userManager;
        readonly ITokenHandler tokenHandler;
        public GoogleLoginCommandHandler(UserManager<P.AppUser> userManager, ITokenHandler tokenHandler)
        {
            this.userManager = userManager;
            this.tokenHandler = tokenHandler;
        }

        public async Task<GoogleLoginCommandResponse> Handle(GoogleLoginCommandRequest request, CancellationToken cancellationToken)
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string> { "957600947173-p70lrqqhr09h965tdg3590122k70q04c.apps.googleusercontent.com" }
            };
            var payload = await GoogleJsonWebSignature.ValidateAsync(request.IdToken, settings);
            var info = new UserLoginInfo(request.Provider, payload.Subject, request.Provider);
            P.AppUser user = await userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
            bool result = user != null;
            if (user == null)
            {
                user = await userManager.FindByEmailAsync(payload.Email);
                if (user == null)
                {
                    user = new()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Email = payload.Email,
                        UserName = payload.Email,
                        NameSurname = payload.Name
                    };
                    var identityResult = await userManager.CreateAsync(user);
                    result = identityResult.Succeeded;
                }
            }
            if (result)
                await userManager.AddLoginAsync(user, info);
            else
                throw new Exception("Invalid External Authentication");
            Token token = tokenHandler.CreateAccessToken(5);
            return new()
            {
                Token = token
            };
        }
    }
}
