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
        /// T�m sipari�leri getirir.
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
        /// Belirli bir sipari�i ID'ye g�re getirir.
        /// GET /orders/{id}
        /// </summary>
        [HttpGet("orders/{id}")]
        public async Task<IActionResult> GetOrderById(string id)
        {
            var query = new GetOrderByIdQuery { OrderId = id };
            var order = await _mediator.Send(query);

            if (order == null)
            {
                return NotFound("Sipari� bulunamad�.");
            }

            return Ok(order);
        }

        /// <summary>
        /// Yeni bir sipari� olu�turur.
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

            return BadRequest("Sipari� olu�turulamad�.");
        }

        /// <summary>
        /// Sipari�i iptal eder.
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

            return BadRequest("Sipari� iptal edilemedi.");
        }

        /// <summary>
        /// Sipari�i tamamlar.
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

            return BadRequest("Sipari� tamamlanamad�.");
        }

        /// <summary>
        /// Sipari�in �r�nlerini g�nceller.
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

            return BadRequest("Sipari� �r�nleri g�ncellenemedi.");
        }

        /// <summary>
        /// Sipari�in durumunu de�i�tirir.
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

            return BadRequest("Sipari� durumu g�ncellenemedi.");
        }

        /// <summary>
        /// Sipari�in iadesini ger�ekle�tirir.
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

            return BadRequest("Sipari� iadesi ger�ekle�tirilemedi.");
        }

        /// <summary>
        /// �ptal edilen sipari�i yeniden a�ar.
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

            return BadRequest("Sipari� yeniden a��lamad�.");
        }

        /// <summary>
        /// Sipari�i siler.
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

            return BadRequest("Sipari� silinemedi.");
        }

        /// <summary>
        /// Sipari�in farkl� a�amalardan ge�mesini sa�lar.
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

            return BadRequest("Sipari� a�amalar� i�lenemedi.");
        }

        /// <summary>
        /// Sipari�in bir k�sm�n� sevk eder.
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

            return BadRequest("Sipari� k�smen sevk edilemedi.");
        }

        /// <summary>
        /// �ade edilen �r�nleri i�le ve sto�a geri ekle.
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

            return BadRequest("�ade i�lemi ger�ekle�tirilemedi.");
        }

        /// <summary>
        /// Sat��� d���k �r�nleri sat��tan kald�r�r.
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

            return BadRequest("Sat��� d���k �r�nler sat��tan kald�r�lamad�.");
        }
    }
}