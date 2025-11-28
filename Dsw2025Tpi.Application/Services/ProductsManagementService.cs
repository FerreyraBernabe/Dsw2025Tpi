using Dsw2025Tpi.Application.Dtos;
using Dsw2025Tpi.Application.Exceptions;
using Dsw2025Tpi.Application.Interfaces;
using Dsw2025Tpi.Application.Validation;
using Dsw2025Tpi.Domain.Entities;
using Dsw2025Tpi.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsw2025Tpi.Application.Services;

public class ProductsManagementService : IProductsManagementService
{
    private readonly IRepository _repository;

    public ProductsManagementService(IRepository repository)
    {
        _repository = repository;
    }
    public async Task<ProductModel.Response?> GetProductById(Guid id)
    {
       
        var product = await _repository.GetById<Product>(id)
                      ?? throw new EntityNotFoundException("Product not found.", ExceptionErrorCodes.ProductNotFound);


        return new ProductModel.Response(
            product.Id,
            product.Sku,
            product.InternalCode,
            product.Name,
            product.Description,
            product.CurrentUnitPrice,
            product.StockQuantity,
            product.IsActive
        );
    }

    public async Task<ProductModel.ResponsePagination> GetAllProducts(ProductModel.FilterProduct request)
    {
        var isActive = request.Status == "enabled"
                ? (bool?)true
                : request.Status == "disabled"
                ? (bool?)false
                : null;
        var activeProducts = await _repository.GetFiltered<Product>(p => ((isActive == null || p.IsActive == isActive)
        && string.IsNullOrEmpty(request.Search) || p.Name.Contains(request.Search)));

        if (activeProducts is null || !activeProducts.Any())
                    throw new NoContentException("No products were found", ExceptionErrorCodes.NoProducts);

        var products = activeProducts.Select(p => new ProductModel.Response(
        p.Id,
        p.Sku,
        p.InternalCode,
        p.Name,
        p.Description,
        p.CurrentUnitPrice,
        p.StockQuantity,
        p.IsActive))
        .OrderBy(p => p.Sku)
        .Skip((request.PageNumber - 1) * request.PageSize ?? 0)
        .Take(request.PageSize ?? activeProducts.Count());

        return new ProductModel.ResponsePagination(products.ToList(), activeProducts.Count());
    }

    public async Task<ProductModel.Response> AddProduct(ProductModel.Request request)
    {
        ProductValidator.Validate(request);

        var existSku = await _repository.First<Product>(p => p.Sku == request.Sku);
        var existInternalCode = await _repository.First<Product>(p => p.InternalCode == request.InternalCode);

        if (existSku != null) {
            throw new DuplicatedEntityException($"A product with the same SKU already exists: {request.Sku}", ExceptionErrorCodes.DuplicateUSKU);
        }

        if (existInternalCode != null)  {
            throw new DuplicatedEntityException($"A product with the same Internal Code already exists: {request.InternalCode}", ExceptionErrorCodes.DuplicateInternalCode);
        }

        var product = new Product(
            request.Sku, 
            request.InternalCode, 
            request.Name, 
            request.Description, 
            request.CurrentUnitPrice, 
            request.StockQuantity
        );
        
        await _repository.Add(product);

        return new ProductModel.Response(
            product.Id, 
            product.Sku, 
            product.InternalCode, 
            product.Name, 
            product.Description, 
            product.CurrentUnitPrice, 
            product.StockQuantity, 
            product.IsActive
        );
    }

   public async Task<ProductModel.Response> UpdateProduct(Guid id, ProductModel.Request request)
    {
        ProductValidator.Validate(request);

        var product = await _repository.GetById<Product>(id) 
            ?? throw new EntityNotFoundException("Product not found.", ExceptionErrorCodes.ProductNotFound);

        var existSku = await _repository.First<Product>(p => p.Sku == request.Sku);
        var existInternalCode = await _repository.First<Product>(p => p.InternalCode == request.InternalCode);

        if (existSku != null && !(existSku.Id == id))
        {
            throw new DuplicatedEntityException($"A product with the same SKU already exists: {request.Sku}", ExceptionErrorCodes.DuplicateUSKU);
        }

        if (existInternalCode != null && !(existInternalCode.Id == id))
        {
            throw new DuplicatedEntityException($"A product with the same Internal Code already exists: {request.InternalCode}", ExceptionErrorCodes.DuplicateInternalCode);
        }

        product.Sku = request.Sku;
        product.InternalCode = request.InternalCode;
        product.Name = request.Name;
        product.Description = request.Description;
        product.CurrentUnitPrice = request.CurrentUnitPrice;
        product.StockQuantity = request.StockQuantity;

        var updated = await _repository.Update(product);

        return new ProductModel.Response(
            updated.Id,
            updated.Sku,
            updated.InternalCode,
            updated.Name,
            updated.Description,
            updated.CurrentUnitPrice,
            updated.StockQuantity,
            updated.IsActive
        );
    }

   public async Task<ProductModel.Response?> DeactivateProduct(Guid id)
    {

       var product = await _repository.GetById<Product>(id)
                      ?? throw new EntityNotFoundException("Product not found.", ExceptionErrorCodes.ProductNotFound);

       product.IsActive = false;
       var updated = await _repository.Update(product);

        return new ProductModel.Response(
            updated.Id,
            updated.Sku,
            updated.InternalCode,
            updated.Name,
            updated.Description,
            updated.CurrentUnitPrice,
            updated.StockQuantity,
            updated.IsActive
        );
    }

}

