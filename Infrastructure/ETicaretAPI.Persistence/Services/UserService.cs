using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Application.DTOs.User;
using ETicaretAPI.Application.Exceptions;
using ETicaretAPI.Application.Helpers;
using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Domain.Entities;
using ETicaretAPI.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System.Security.Policy;
using System.Text;

namespace ETicaretAPI.Persistence.Services
{
    public class UserService : IUserService
    {
        readonly UserManager<AppUser> userManager;
        readonly IEndpointReadRepository endpointReadRepository;

        public UserService(UserManager<AppUser> userManager, IEndpointReadRepository endpointReadRepository)
        {
            this.userManager = userManager;
            this.endpointReadRepository = endpointReadRepository;
        }


        public async Task<CreateUserResponse> CreateAsync(CreateUser createUser)
        {
            IdentityResult result = await userManager.CreateAsync(new()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = createUser.Username,
                Email = createUser.Email,
                NameSurname = createUser.NameSurname
            }, createUser.Password);
            CreateUserResponse response = new() { Succeeded = result.Succeeded };
            if (result.Succeeded)
                response.Message = "Kullanıcı Kaydı Başarıyla Tamamlandı";
            else
                foreach (var item in result.Errors)
                    response.Message += $"{item.Code} - {item.Description}<br>";
            return response;
        }

        public async Task<List<ListUser>> GetAllUsersAsync(int page, int pageSize)
        {
            var users = await userManager.Users.Skip(page * pageSize).Take(pageSize).ToListAsync();
            return users.Select(user => new ListUser
            {
                Id = user.Id,
                Email = user.Email,
                NameSurname = user.NameSurname,
                TwoFactorEnabled = user.TwoFactorEnabled,
                UserName = user.UserName
            }).ToList();
        }
        public int TotalUsersCount => userManager.Users.Count();


        public async Task UpdatePasswordAsync(string userId, string resetToken, string newPassword)
        {
            AppUser appUser = await userManager.FindByIdAsync(userId);
            if (appUser != null)
            {
                resetToken = resetToken.UrlDecode();
                IdentityResult identityResult = await userManager.ResetPasswordAsync(appUser, resetToken, newPassword);
                if (identityResult.Succeeded)
                    await userManager.UpdateSecurityStampAsync(appUser);
                else
                    throw new PasswordChangeFailedException();
            }
        }

        public async Task UpdateRefreshTokenAsync(string refreshToken, AppUser user, DateTime accessTokenDate, int addOnAccessTokenDate)
        {
            if (user != null)
            {
                user.RefreshToken = refreshToken;
                user.RefreshTokenEndDate = accessTokenDate.AddSeconds(addOnAccessTokenDate);
                await userManager.UpdateAsync(user);
            }
            else
            {
                throw new NotFoundUserException();

            }
        }

        public async Task AssignRoleToUserAsync(string userId, string[] roles)
        {
            var appUser = await userManager.FindByIdAsync(userId);
            if (appUser is not null)
            {
                var userRoles = await userManager.GetRolesAsync(appUser);
                await userManager.RemoveFromRolesAsync(appUser, userRoles);
                await userManager.AddToRolesAsync(appUser, roles);
            }
        }

        public async Task<string[]> GetRolesToUserAsync(string userIdOrName)
        {
            var appUser = await userManager.FindByIdAsync(userIdOrName);
            if (appUser is null)
            {
                appUser = await userManager.FindByNameAsync(userIdOrName);
            }
            if (appUser is not null)
            {
                var userRoles = await userManager.GetRolesAsync(appUser);
                return userRoles.ToArray();
            }
            return new string[] { };
        }

        public async Task<bool> HasRolePermissionToEndpointAsync(string name, string code)
        {
            var userRoles = await GetRolesToUserAsync(name);
            if (!userRoles.Any())
                return false;
            Endpoint endpoint = await endpointReadRepository.Table.Include(x => x.AppRoles).FirstOrDefaultAsync(x => x.Code == code);
            if (endpoint is null)
                return false;
            var hasRole = false;
            var endpointRoles = endpoint.AppRoles.Select(x => x.Name);
            #region 1. Alternatif
            //foreach (var userRole in userRoles)
            //{
            //    if (!hasRole)
            //    {
            //        foreach (var endpointRole in endpointRoles)
            //            if (userRole == endpointRole)
            //            {
            //                hasRole = true;
            //                break;
            //            }
            //    }
            //    else
            //        break;
            //}
            //return hasRole;
            #endregion
            #region 2. Alternatif
            foreach (var userRole in userRoles)
            {
                foreach (var endpointRole in endpointRoles)
                    if (userRole == endpointRole)
                        return true;
            }
            return false;
            #endregion
        }
    }
}
