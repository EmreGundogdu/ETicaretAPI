using ETicaretAPI.Application.Repositories;
using MediatR;

namespace ETicaretAPI.Application.Features.Commands.Product.UpdateProduct
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommandRequest, UpdateProductCommandResponse>
    {
        readonly IProductReadRepository productReadRepository;
        readonly IProductWriteRepository productWriteRepository;
        public UpdateProductCommandHandler(IProductReadRepository productReadRepository, IProductWriteRepository productWriteRepository)
        {
            this.productReadRepository = productReadRepository;
            this.productWriteRepository = productWriteRepository;
        }

        public async Task<UpdateProductCommandResponse> Handle(UpdateProductCommandRequest request, CancellationToken cancellationToken)
        {
            var product = await productReadRepository.GetByIdAsync(request.Id);
            product.Name = request.Name;
            product.Stock = request.Stock;
            product.Price = request.Price;
            await productWriteRepository.SaveAsync();
            return new()
            {

            };
        }
    }
}
