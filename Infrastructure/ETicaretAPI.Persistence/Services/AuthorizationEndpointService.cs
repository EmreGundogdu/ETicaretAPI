using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Application.Abstractions.Services.Configurations;
using ETicaretAPI.Application.Repositories;

namespace ETicaretAPI.Persistence.Services
{
    public class AuthorizationEndpointService : IAuthorizationEndpointService
    {
        readonly IApplicationService applicationService;
        readonly IEndpointReadRepository endpointReadRepository;
        readonly IEndpointWriteRepository endpointWriteRepository;

        public AuthorizationEndpointService(IApplicationService applicationService, IEndpointReadRepository endpointReadRepository, IEndpointWriteRepository endpointWriteRepository)
        {
            this.applicationService = applicationService;
            this.endpointReadRepository = endpointReadRepository;
            this.endpointWriteRepository = endpointWriteRepository;
        }

        public async Task AssignRoleEndpointAsync(string[] roles, string code,Type type)
        {
            var endpoint = await endpointReadRepository.GetSingleAsync(x => x.Code == code);
            if (endpoint == null)
            {
                applicationService.GetAuthorizeDefinitionEndpoints(type);
            }
        }
    }
}
