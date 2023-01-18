using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Domain.Entities;
using ETicaretAPI.Persistence.Contexts;

namespace ETicaretAPI.Persistence.Repositories
{
    public class EndpointReadRepository : ReadRepository<Menu>, IMenuReadRepository
    {
        public EndpointReadRepository(ETicaretAPIDbContext context) : base(context)
        {
        }
    }
}
