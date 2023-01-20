using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Application.Abstractions.Services.Configurations;
using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Domain.Entities;
using ETicaretAPI.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ETicaretAPI.Persistence.Services
{
    public class AuthorizationEndpointService : IAuthorizationEndpointService
    {
        readonly IApplicationService applicationService;
        readonly IEndpointReadRepository endpointReadRepository;
        readonly IEndpointWriteRepository endpointWriteRepository;
        readonly IMenuReadRepository menuReadRepository;
        readonly IMenuWriteRepository menuWriteRepository;
        readonly RoleManager<AppRole> roleManager;
        public AuthorizationEndpointService(IApplicationService applicationService, IEndpointReadRepository endpointReadRepository, IEndpointWriteRepository endpointWriteRepository, IMenuReadRepository menuReadRepository, IMenuWriteRepository menuWriteRepository, RoleManager<AppRole> roleManager)
        {
            this.applicationService = applicationService;
            this.endpointReadRepository = endpointReadRepository;
            this.endpointWriteRepository = endpointWriteRepository;
            this.menuReadRepository = menuReadRepository;
            this.menuWriteRepository = menuWriteRepository;
            this.roleManager = roleManager;
        }

        public async Task AssignRoleEndpointAsync(string[] roles, string code, string menu, Type type)
        {
            Menu? _menu = await menuReadRepository.GetSingleAsync(x => x.Name == menu);
            if (_menu is null)
            {
                _menu = new()
                {

                    ID = Guid.NewGuid(),
                    Name = menu
                };
                await menuWriteRepository.AddAsync(_menu);
                await menuWriteRepository.SaveAsync();
            }
            var endpoint = await endpointReadRepository.Table.Include(x => x.Menu).Include(x => x.AppRoles).FirstOrDefaultAsync(x => x.Code == code && x.Menu.Name == menu);
            if (endpoint == null)
            {
                var action = applicationService.GetAuthorizeDefinitionEndpoints(type).FirstOrDefault(x => x.Name == menu)?.Actions.FirstOrDefault(x => x.Code == code);
                endpoint = new()
                {
                    Code = action.Code,
                    ActionType = action.ActionType,
                    HttpType = action.HttpType,
                    Definition = action.Definition,
                    ID = Guid.NewGuid(),
                    Menu = _menu
                };
                await endpointWriteRepository.AddAsync(endpoint);
                await endpointWriteRepository.SaveAsync();
            }
            foreach (var item in endpoint.AppRoles)
            {
                endpoint.AppRoles.Remove(item);
            }

            var approles = await roleManager.Roles.Where(x => roles.Contains(x.Name)).ToListAsync();
            foreach (var role in approles)
                endpoint.AppRoles.Add(role);

            await endpointWriteRepository.SaveAsync();
        }

        public async Task<List<string>> GetRolesToEndpoint(string cod,string menu)
        {
            var endpoint = await endpointReadRepository.Table.Include(x => x.AppRoles).Include(x=>x.Menu).FirstOrDefaultAsync(x => x.Code == cod&&x.Menu.Name==menu);
            if (endpoint is not null)
            {
                return endpoint.AppRoles.Select(x => x.Name).ToList();
            }
            return null;
        }
    }
}
