﻿using ETicaretAPI.Application.Abstractions.Services.Authentication;
using MediatR;

namespace ETicaretAPI.Application.Features.Commands.AppUser.LoginUser
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommandRequest, LoginUserCommandResponse>
    {
        readonly IInternalAuthentication internalAuthentication;

        public LoginUserCommandHandler(IInternalAuthentication internalAuthentication)
        {
            this.internalAuthentication = internalAuthentication;
        }

        public async Task<LoginUserCommandResponse> Handle(LoginUserCommandRequest request, CancellationToken cancellationToken)
        {
            var token = await internalAuthentication.LoginAsync(request.UsernameOrEmail, request.Password, 900);
            return new LoginUserSuccessCommandResponse()
            {
                Token = token
            };
        }
    }
}
