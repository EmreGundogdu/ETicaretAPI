using ETicaretAPI.Application.Abstractions.Services.Authentication;
using MediatR;

namespace ETicaretAPI.Application.Features.Commands.AppUser.FacebookLogin
{
    public class FacebookLoginCommandHandler : IRequestHandler<FacebookLoginCommandRequest, FacebookLoginCommandResponse>
    {
        readonly IExternalAuthentication externalAuthentication;

        public FacebookLoginCommandHandler(IExternalAuthentication externalAuthentication
            )
        {
            this.externalAuthentication = externalAuthentication;
        }

        public async Task<FacebookLoginCommandResponse> Handle(FacebookLoginCommandRequest request, CancellationToken cancellationToken)
        {

            var token = await externalAuthentication.FacebookLoginAsync(request.AuthToken, 900);
            return new()
            {
                Token = token
            };
        }
    }
}