using Dsw2025Tpi.Application.Dtos;
using Dsw2025Tpi.Application.Exceptions;
using Dsw2025Tpi.Application.Interfaces;
using Dsw2025Tpi.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ApplicationException = Dsw2025Tpi.Application.Exceptions.ApplicationException;

namespace Dsw2025Tpi.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/orders")]
    public class OrdersController : ControllerBase
    {
        private readonly OrdersManagementService _service;

        public OrdersController(OrdersManagementService service)
        {
            _service = service;
        }


        //punto 6
        [HttpPost]
        public async Task<IActionResult> CreateOrderAsync([FromBody] OrderModel.OrderRequest request)
        {
            if (request == null || request.OrderItems == null || request.OrderItems.Count == 0)
                throw new BadRequestException("Datos de la orden inválidos o incompletos.");
                var order = await _service.CreateOrderAsync(request);
                return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, order);
           
        }


        //punto 7
        [HttpGet(Name = "GetAllOrders")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllOrders([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? status = null, [FromQuery] Guid? customerId = null)
        {
            var result = await _service.GetAllOrders(page, pageSize, status, customerId);
            return Ok(result);
            
        }


        //punto 8
        [HttpGet("{id:guid}", Name = "GetOrderById")]
        [AllowAnonymous]
        public async Task<IActionResult> GetOrderById(Guid id)
        {
            var orden = await _service.GetOrderById(id);
            if (orden == null)
            {
                throw new EntityNotFoundException("Orden no encontrada");
            }
            return Ok(orden);
        }

        //punto 9 
        [HttpPut("{id:guid}/status")]
        public async Task<IActionResult> UpdateOrderStatus(Guid id, [FromBody] string status)
        {
            if (string.IsNullOrWhiteSpace(status))
                throw new BadRequestException("Debe especificar un estado válido.");

                var updatedOrder = await _service.UpdateOrderStatus(id, status);
                if (updatedOrder == null)
                    throw new EntityNotFoundException("Orden no encontrada.");
                return Ok(updatedOrder);
            
        }
    }
}


