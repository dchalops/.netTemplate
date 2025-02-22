using Microsoft.AspNetCore.Mvc;
using Orders.Application.Services;
using Orders.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;

namespace Orders.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly OrderService _orderService;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(OrderService orderService, ILogger<OrdersController> logger)
        {
            _orderService = orderService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<List<Order>>> GetOrders()
        {
            try
            {
                var orders = await _orderService.GetOrdersAsync();
                return Ok(orders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los pedidos.");
                return StatusCode(500, new { message = "Ocurrió un error al obtener los pedidos." });
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateOrder([FromBody] Order order)
        {
            try
            {
                if (order == null)
                {
                    return BadRequest(new { message = "El pedido no puede estar vacío." });
                }

                await _orderService.AddOrderAsync(order);
                return CreatedAtAction(nameof(GetOrders), new { id = order.ID }, order);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear un pedido.");
                return StatusCode(500, new { message = "Ocurrió un error al crear el pedido." + ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrderById(Guid id)
        {
            try
            {
                var order = await _orderService.GetOrderByIdAsync(id);
                if (order == null)
                {
                    return NotFound(new { message = "El pedido no existe." });
                }

                return Ok(order);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el pedido.");
                return StatusCode(500, new { message = "Ocurrió un error al obtener el pedido." });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateOrder(Guid id, [FromBody] Order order)
        {
            try
            {
                if (order == null)
                {
                    return BadRequest(new { message = "El pedido no puede estar vacío." });
                }

                if (id != order.ID)
                {
                    return BadRequest(new { message = "El ID del pedido no coincide." });
                }

                var existingOrder = await _orderService.GetOrderByIdAsync(id);
                if (existingOrder == null)
                {
                    return NotFound(new { message = "El pedido no existe." });
                }

                await _orderService.UpdateOrderAsync(order);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el pedido.");
                return StatusCode(500, new { message = "Ocurrió un error al actualizar el pedido." });
            }
        }

        [HttpPut("delete/{id}")]
        public async Task<ActionResult> DeleteOrder(Guid id)
        {
            try
            {
                await _orderService.DeleteOrderAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el pedido.");
                return StatusCode(500, new { message = "Ocurrió un error al eliminar el pedido." });
            }
        }   

    }
}
