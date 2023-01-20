using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Application.Consts;
using ETicaretAPI.Application.CustomAttributes;
using ETicaretAPI.Application.Enums;
using ETicaretAPI.Application.Features.Commands.Product.CreateProduct;
using ETicaretAPI.Application.Features.Commands.Product.RemoveProduct;
using ETicaretAPI.Application.Features.Commands.Product.UpdateProduct;
using ETicaretAPI.Application.Features.Commands.ProductImage.ChangeShowcaseImage;
using ETicaretAPI.Application.Features.Commands.ProductImage.RemoveProductImage;
using ETicaretAPI.Application.Features.Commands.ProductImage.UploadProductImage;
using ETicaretAPI.Application.Features.Queries.Product.GetAllProducts;
using ETicaretAPI.Application.Features.Queries.Product.GetProductById;
using ETicaretAPI.Application.Features.Queries.ProductImage.GetProductImages;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ETicaretAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        readonly IMediator mediator;
        readonly IProductService productService;
        public ProductsController(IMediator mediator, IProductService productService)
        {
            this.mediator = mediator;
            this.productService = productService;
        }
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetAllProductsQueryRequest getAllProductsQueryRequest)
        {
            GetAllProductsQueryResponse data = await mediator.Send(getAllProductsQueryRequest);
            return Ok(data);
        }

        [HttpGet("qrcode/{productId}")]
        public async Task<IActionResult> GetQrCodeToProduct([FromQuery] string productId)
        {
            var data = await productService.QrCodeToProductAsync(productId);
            return File(data,"image/png");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] GetProductByIdQueryRequest getProductByIdQueryRequest)
        {
            var response = await mediator.Send(getProductByIdQueryRequest);
            return Ok(response);
        }
        [HttpPost]
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Products, ActionType = ActionType.Writing, Definition = "Create Product")]
        public async Task<IActionResult> Post(CreateProductCommandRequest createProductCommandRequest)
        {
            CreateProductCommandResponse result = await mediator.Send(createProductCommandRequest);
            return Ok(result);
        }
        [HttpPut]
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Products, ActionType = ActionType.Updating, Definition = "Update Product")]
        public async Task<IActionResult> Put([FromBody] UpdateProductCommandRequest updateProductCommandRequest)
        {
            var result = await mediator.Send(updateProductCommandRequest);
            return Ok();
        }
        [HttpDelete("{Id}")]
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Products, ActionType = ActionType.Deleting, Definition = "Delete Product")]
        public async Task<IActionResult> Delete([FromRoute] RemoveProductCommandRequest removeProductCommandRequest)
        {
            var result = await mediator.Send(removeProductCommandRequest);
            return NoContent();
        }
        [HttpPost("[action]")]
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Products, ActionType = ActionType.Writing, Definition = "Upload Product File")]
        public async Task<IActionResult> Upload([FromQuery] UploadProductImageCommandRequest uploadProductImageCommandRequest)
        {
            uploadProductImageCommandRequest.Files = Request.Form.Files;
            var result = await mediator.Send(uploadProductImageCommandRequest);
            return Ok();
        }
        [HttpGet("[action]/{id}")]
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Products, ActionType = ActionType.Reading, Definition = "Get Product Images")]
        public async Task<IActionResult> GetProductImages([FromRoute] GetProductImageQueryRequest getProductImageQueryRequest)
        {
            var result = await mediator.Send(getProductImageQueryRequest);
            return Ok(result);
        }
        [HttpDelete("[action]/{Id}")]
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Products, ActionType = ActionType.Deleting, Definition = "Delete Product Images")]
        public async Task<IActionResult> DeleteProductImage([FromRoute] RemoveProductImageCommandRequest removeProductImageCommandRequest, [FromQuery] string imageId)
        {
            removeProductImageCommandRequest.ImageId = imageId;
            var result = await mediator.Send(removeProductImageCommandRequest);
            return Ok();
        }
        [HttpGet("[action]")]
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Products, ActionType = ActionType.Updating, Definition = "Change Showcase Image")]
        public async Task<IActionResult> ChangeShowcaseImage([FromQuery] ChangeShowcaseImageCommandRequest changeShowcaseImageCommandRequest)
        {
            var response = await mediator.Send(changeShowcaseImageCommandRequest);
            return Ok(response);
        }
    }
}
