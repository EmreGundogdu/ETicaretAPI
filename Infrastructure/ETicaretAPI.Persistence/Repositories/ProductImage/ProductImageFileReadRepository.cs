using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Domain.Entities;
using ETicaretAPI.Persistence.Contexts;
using ETicaretAPI.Persistence.Repositories;

namespace ETicaretAPI.Persistence.Repositories
{
    public class ProductImageFileReadRepository : ReadRepository<ProductImageFile>, IProductImageFileReadRepository
    {
        public ProductImageFileReadRepository(ETicaretAPIDbContext context) : base(context)
        {
        }
    }
}
