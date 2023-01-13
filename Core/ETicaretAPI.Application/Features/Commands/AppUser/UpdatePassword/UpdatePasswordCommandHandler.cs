using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Application.Exceptions;
using MediatR;

namespace ETicaretAPI.Application.Features.Commands.AppUser.UpdatePassword
{
    public class UpdatePasswordCommandHandler : IRequestHandler<UpdatePasswordCommandRequest, UpdatePasswordCommandResponse>
    {
        readonly IUserService userService;

        public UpdatePasswordCommandHandler(IUserService userService)
        {
            this.userService = userService;
        }

        public async Task<UpdatePasswordCommandResponse> Handle(UpdatePasswordCommandRequest request, CancellationToken cancellationToken)
        {
            if (!request.NewPassword.Equals(request.NewPasswordConfirm))
            {
                throw new PasswordChangeFailedException();
            }
            await userService.UpdatePasswordAsync(request.UserId, request.ResetToken, request.NewPassword);
            return new();
        }
    }
}
