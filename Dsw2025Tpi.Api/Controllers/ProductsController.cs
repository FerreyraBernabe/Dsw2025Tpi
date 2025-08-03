using Dsw2025Tpi.Application.Dtos;
using Dsw2025Tpi.Application.Exceptions;
using Dsw2025Tpi.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ApplicationException = Dsw2025Tpi.Application.Exceptions.ApplicationException;


namespace Dsw2025Tpi.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly ProductsManagementService _service;

    public ProductsController(ProductsManagementService service)
    {
        _service = service;
    }


    // punto 1
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateProductAsync([FromBody] ProductModel.Request request)
    {
        var createdProduct = await _service.AddProduct(request);
        return CreatedAtRoute("GetProductById", new { id = createdProduct.Id }, createdProduct);
    }


    //punto 2
    [HttpGet()]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllProductsAsync()
    {
        var products = await _service.GetAllProducts();
        if (products == null || !products.Any()) return NoContent();
        return Ok(products);
    }

   
    // punto 3
    [HttpGet("{id:guid}", Name = "GetProductById")]
    [AllowAnonymous]
    public async Task<IActionResult> GetProductByIdAsync(Guid id)
    {
        var product = await _service.GetProductById(id);        
        return Ok(product);
    }


    // punto 4
    [HttpPut()]
    [Route("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateProductAsync(Guid id, [FromBody] ProductModel.Request request)
    {
        var updatedProduct = await _service.UpdateProduct(id, request);        
        return Ok(updatedProduct);
    }


    // punto 5
    [HttpPatch()]
    [Route("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> PatchProduct(Guid id)
    {        
        var product = await _service.DeactivateProduct(id);
        Response.Headers.Append("X-Message", "Deactivated product.");
        return NoContent();
    }
}