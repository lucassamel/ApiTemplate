using ApiTemplate.Application.DTOs;
using ApiTemplate.Application.Interfaces;
using ApiTemplate.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiTemplate.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProductsController(IProductService productService) : ControllerBase
    {
        private readonly IProductService _productService = productService;

        /// <summary>
        ///     Obtém uma lista de Produtos
        /// </summary>
        /// <remarks>
        ///     Obtém uma lista de Produtos
        /// </remarks>        
        /// <returns>Uma lista com todos os produtos</returns>
        /// <response code="200">Retorna uma lista de produtos.</response>
        /// <response code="400">Querystring inválida.</response>
        /// <response code="404">Se nenhum produto for encontrado.</response>
        /// <response code="500">Erro interno.</response>
        [HttpGet]
        [ProducesResponseType(typeof(Response<IEnumerable<ProductDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<IEnumerable<string>>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.GetAllProductsAsync();

            if (products == null || !products.Any())
            {
                return NotFound(new Response<string>("Nenhum produto foi encontrado."));
            }

            return Ok(new Response<IEnumerable<ProductDto?>?>(products));
        }

        /// <summary>
        ///     Obtém um produto por ID
        /// </summary>
        /// <remarks>
        ///     Obtém um produto por ID
        /// </remarks>        
        /// <returns>Um produto</returns>
        /// <response code="200">Retorna um produto</response>
        /// <response code="400">Querystring inválida.</response>
        /// <response code="404">Se nenhum produto for encontrado.</response>
        /// <response code="500">Erro interno.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Response<ProductDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<IEnumerable<string>>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var product = await _productService.GetProductByIdAsync(id);

            if (product == null)
            {
                return NotFound(new Response<string>("Produto não encontrado."));
            }

            return Ok(new Response<ProductDto>(product));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductDto dto)
        {
            var product = await _productService.CreateProductAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CreateProductDto dto)
        {
            await _productService.UpdateProductAsync(id, dto);
            return NoContent();
        }

        /// <summary>
        ///     Deleta um produto por ID
        /// </summary>
        /// <remarks>
        ///     Deleta um produto por ID
        /// </remarks>        
        /// <returns>Um produto</returns>
        /// <response code="204">Produto deletado</response>        
        /// <response code="404">Se nenhum produto for encontrado.</response>
        /// <response code="500">Erro interno.</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _productService.DeleteProductAsync(id);
            return NoContent();
        }
    }
}
