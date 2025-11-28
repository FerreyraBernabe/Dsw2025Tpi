using Dsw2025Tpi.Application.Dtos;
using Dsw2025Tpi.Application.Exceptions;
using Dsw2025Tpi.Application.Interfaces;
using Dsw2025Tpi.Application.Services;
using Dsw2025Tpi.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using ApplicationException = Dsw2025Tpi.Application.Exceptions.ApplicationException;

namespace Dsw2025Tpi.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/customers")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _service;

        public CustomersController(ICustomerService service)
        {
            _service = service;
        }

        [Authorize]
        [HttpPost("me-by-email")]
        public async Task<IActionResult> GetMyCustomerByEmail([FromBody] string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new BadRequestException("Email is required");

            var customer = await _service.GetByEmailAsync(email);

            if (customer is null)
                return NotFound("Customer not found");

            return Ok(customer);
        }

    }
}