using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderStream.Application.Commands;
using OrderStream.Application.Queries;

namespace OrderStream.Host.Controllers
{
    [ApiController]
    [Route("")]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Tüm ürünleri getirir.
        /// GET /products
        /// </summary>
        [HttpGet("products")]
        public async Task<IActionResult> GetAllProducts()
        {
            var query = new GetAllProductsQuery();
            var products = await _mediator.Send(query);
            return Ok(products);
        }

        /// <summary>
        /// Belirli bir ürünü ID'ye göre getirir.
        /// GET /products/{id}
        /// </summary>
        [HttpGet("products/{id}")]
        public async Task<IActionResult> GetProductById(string id)
        {
            var query = new GetProductByIdQuery { ProductId = id };
            var product = await _mediator.Send(query);

            if (product == null)
            {
                return NotFound("Ürün bulunamadı.");
            }

            return Ok(product);
        }

        /// <summary>
        /// Yeni bir ürün oluşturur.
        /// POST /products
        /// </summary>
        [HttpPost("products")]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductCommand command)
        {
            var result = await _mediator.Send(command);
            if (result)
            {
                return CreatedAtAction(nameof(GetProductById), new { id = command.Name }, command);
            }

            return BadRequest("Ürün oluşturulamadı.");
        }

        /// <summary>
        /// Belirli bir ürünü günceller.
        /// PUT /products/{id}
        /// </summary>
        [HttpPut("products/{id}")]
        public async Task<IActionResult> UpdateProduct(string id, [FromBody] UpdateProductCommand command)
        {
            command.Id = id;
            var result = await _mediator.Send(command);

            if (result)
            {
                return NoContent();
            }

            return BadRequest("Ürün güncellenemedi.");
        }

        /// <summary>
        /// Belirli bir ürünü siler.
        /// DELETE /products/{id}
        /// </summary>
        [HttpDelete("products/{id}")]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            var command = new DeleteProductCommand { ProductId = id };
            var result = await _mediator.Send(command);

            if (result)
            {
                return NoContent();
            }

            return NotFound("Ürün bulunamadı.");
        }

        /// <summary>
        /// Belirli bir ürüne stok ekler.
        /// PUT /products/{id}/restock
        /// </summary>
        [HttpPut("products/{id}/restock")]
        public async Task<IActionResult> RestockProduct(string id, [FromBody] RestockProductCommand command)
        {
            command.ProductId = id;
            var result = await _mediator.Send(command);

            if (result)
            {
                return NoContent();
            }

            return BadRequest("Stok güncellenemedi.");
        }

        /// <summary>
        /// Belirli bir ürüne indirim uygular.
        /// PUT /products/{id}/discount
        /// </summary>
        [HttpPut("products/{id}/discount")]
        public async Task<IActionResult> DiscountProduct(string id, [FromBody] DiscountProductCommand command)
        {
            command.ProductId = id;
            var result = await _mediator.Send(command);

            if (result)
            {
                return NoContent();
            }

            return BadRequest("İndirim uygulanamadı.");
        }

        /// <summary>
        /// Stokta olmayan ürünleri getirir.
        /// GET /products/outofstock
        /// </summary>
        [HttpGet("products/outofstock")]
        public async Task<IActionResult> GetOutOfStockProducts()
        {
            var query = new GetOutOfStockProductsQuery();
            var products = await _mediator.Send(query);
            return Ok(products);
        }

        /// <summary>
        /// Belirli bir ürünü arşivler.
        /// PUT /products/{id}/archive
        /// </summary>
        [HttpPut("products/{id}/archive")]
        public async Task<IActionResult> ArchiveProduct(string id)
        {
            var command = new ArchiveProductCommand { ProductId = id };
            var result = await _mediator.Send(command);

            if (result)
            {
                return NoContent();
            }

            return BadRequest("Ürün arşivlenemedi.");
        }

        /// <summary>
        /// Ürünleri toplu olarak günceller.
        /// PUT /products/bulkupdate
        /// </summary>
        [HttpPut("products/bulkupdate")]
        public async Task<IActionResult> BulkUpdateProducts([FromBody] BulkUpdateProductsCommand command)
        {
            var result = await _mediator.Send(command);

            if (result)
            {
                return NoContent();
            }

            return BadRequest("Ürünler toplu olarak güncellenemedi.");
        }
    }
}