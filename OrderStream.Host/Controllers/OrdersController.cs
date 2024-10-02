using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderStream.Application.Commands;
using OrderStream.Application.Queries;

namespace OrderStream.Host.Controllers
{
    [ApiController]
    [Route("")]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Tüm sipariþleri getirir.
        /// GET /orders
        /// </summary>
        [HttpGet("orders")]
        public async Task<IActionResult> GetAllOrders()
        {
            var query = new GetAllOrdersQuery();
            var orders = await _mediator.Send(query);
            return Ok(orders);
        }

        /// <summary>
        /// Belirli bir sipariþi ID'ye göre getirir.
        /// GET /orders/{id}
        /// </summary>
        [HttpGet("orders/{id}")]
        public async Task<IActionResult> GetOrderById(string id)
        {
            var query = new GetOrderByIdQuery { OrderId = id };
            var order = await _mediator.Send(query);

            if (order == null)
            {
                return NotFound("Sipariþ bulunamadý.");
            }

            return Ok(order);
        }

        /// <summary>
        /// Yeni bir sipariþ oluþturur.
        /// POST /orders
        /// </summary>
        [HttpPost("orders")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderCommand command)
        {
            var result = await _mediator.Send(command);
            if (result)
            {
                return CreatedAtAction(nameof(GetOrderById), new { command = command.Id }, command);
            }

            return BadRequest("Sipariþ oluþturulamadý.");
        }

        /// <summary>
        /// Sipariþi iptal eder.
        /// PUT /orders/{id}/cancel
        /// </summary>
        [HttpPut("orders/{id}/cancel")]
        public async Task<IActionResult> CancelOrder(string id)
        {
            var command = new CancelOrderCommand { OrderId = id };
            var result = await _mediator.Send(command);

            if (result)
            {
                return NoContent();
            }

            return BadRequest("Sipariþ iptal edilemedi.");
        }

        /// <summary>
        /// Sipariþi tamamlar.
        /// PUT /orders/{id}/complete
        /// </summary>
        [HttpPut("orders/{id}/complete")]
        public async Task<IActionResult> CompleteOrder(string id)
        {
            var command = new CompleteOrderCommand { OrderId = id };
            var result = await _mediator.Send(command);

            if (result)
            {
                return NoContent();
            }

            return BadRequest("Sipariþ tamamlanamadý.");
        }

        /// <summary>
        /// Sipariþin ürünlerini günceller.
        /// PUT /orders/{id}/items
        /// </summary>
        [HttpPut("orders/{id}/items")]
        public async Task<IActionResult> UpdateOrderItems(string id, [FromBody] UpdateOrderItemsCommand command)
        {
            command.OrderId = id;
            var result = await _mediator.Send(command);

            if (result)
            {
                return NoContent();
            }

            return BadRequest("Sipariþ ürünleri güncellenemedi.");
        }

        /// <summary>
        /// Sipariþin durumunu deðiþtirir.
        /// PUT /orders/{id}/status
        /// </summary>
        [HttpPut("orders/{id}/status")]
        public async Task<IActionResult> ChangeOrderStatus(string id, [FromBody] ChangeOrderStatusCommand command)
        {
            command.OrderId = id;
            var result = await _mediator.Send(command);

            if (result)
            {
                return NoContent();
            }

            return BadRequest("Sipariþ durumu güncellenemedi.");
        }

        /// <summary>
        /// Sipariþin iadesini gerçekleþtirir.
        /// PUT /orders/{id}/refund
        /// </summary>
        [HttpPut("orders/{id}/refund")]
        public async Task<IActionResult> RefundOrder(string id)
        {
            var command = new RefundOrderCommand { OrderId = id };
            var result = await _mediator.Send(command);

            if (result)
            {
                return NoContent();
            }

            return BadRequest("Sipariþ iadesi gerçekleþtirilemedi.");
        }

        /// <summary>
        /// Ýptal edilen sipariþi yeniden açar.
        /// PUT /orders/{id}/reopen
        /// </summary>
        [HttpPut("orders/{id}/reopen")]
        public async Task<IActionResult> ReopenOrder(string id)
        {
            var command = new ReopenOrderCommand { OrderId = id };
            var result = await _mediator.Send(command);

            if (result)
            {
                return NoContent();
            }

            return BadRequest("Sipariþ yeniden açýlamadý.");
        }

        /// <summary>
        /// Sipariþi siler.
        /// DELETE /orders/{id}
        /// </summary>
        [HttpDelete("orders/{id}")]
        public async Task<IActionResult> DeleteOrder(string id)
        {
            var command = new DeleteOrderCommand { OrderId = id };
            var result = await _mediator.Send(command);

            if (result)
            {
                return NoContent();
            }

            return BadRequest("Sipariþ silinemedi.");
        }

        /// <summary>
        /// Sipariþin farklý aþamalardan geçmesini saðlar.
        /// PUT /orders/{id}/process
        /// </summary>
        [HttpPut("orders/{id}/process")]
        public async Task<IActionResult> ProcessOrderInStages(string id, [FromBody] ProcessOrderInStagesCommand command)
        {
            command.OrderId = id;
            var result = await _mediator.Send(command);

            if (result)
            {
                return NoContent();
            }

            return BadRequest("Sipariþ aþamalarý iþlenemedi.");
        }

        /// <summary>
        /// Sipariþin bir kýsmýný sevk eder.
        /// PUT /orders/{id}/partialship
        /// </summary>
        [HttpPut("orders/{id}/partialship")]
        public async Task<IActionResult> PartialShipOrder(string id, [FromBody] PartialShipOrderCommand command)
        {
            command.OrderId = id;
            var result = await _mediator.Send(command);

            if (result)
            {
                return NoContent();
            }

            return BadRequest("Sipariþ kýsmen sevk edilemedi.");
        }

        /// <summary>
        /// Ýade edilen ürünleri iþle ve stoða geri ekle.
        /// PUT /orders/{id}/return
        /// </summary>
        [HttpPut("orders/{id}/return")]
        public async Task<IActionResult> ReturnOrder(string id, [FromBody] ReturnOrderCommand command)
        {
            command.OrderId = id;
            var result = await _mediator.Send(command);

            if (result)
            {
                return NoContent();
            }

            return BadRequest("Ýade iþlemi gerçekleþtirilemedi.");
        }

        /// <summary>
        /// Satýþý düþük ürünleri satýþtan kaldýrýr.
        /// PUT /orders/discontinue-low-selling
        /// </summary>
        [HttpPut("orders/discontinue-low-selling")]
        public async Task<IActionResult> DiscontinueLowSellingProducts([FromBody] DiscontinueLowSellingProductsCommand command)
        {
            var result = await _mediator.Send(command);

            if (result)
            {
                return NoContent();
            }

            return BadRequest("Satýþý düþük ürünler satýþtan kaldýrýlamadý.");
        }
    }
}