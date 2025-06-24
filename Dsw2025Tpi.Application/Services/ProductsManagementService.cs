using Dsw2025Tpi.Application.Dtos;
using Dsw2025Tpi.Application.Exceptions;
using Dsw2025Tpi.Domain.Entities;
using Dsw2025Tpi.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsw2025Tpi.Application.Services;

public class ProductsManagementService
{
    private readonly IRepository _repository;

    public ProductsManagementService(IRepository repository)
    {
        _repository = repository;
    }
    public async Task<ProductModel.Response?> GetProductById(Guid id)
    {
        var product= await _repository.GetById<Product>(id);
        if (product == null)
            return null;

        return new ProductModel.Response(
            product.Id,
            product.Sku,
            product.Name,
            product.Price,
            product.Stock,
            product.InternalCode,
            product.IsActive
        );
    }
    public async Task<IEnumerable<ProductModel.Response>?> GetAllProducts()
    {
        var products = await _repository.GetFiltered<Product>(p => p.IsActive);
        return products?.Select(p => new ProductModel.Response(
            p.Id,
            p.Sku,
            p.Name,
            p.Price,
            p.Stock,
            p.InternalCode,
            p.IsActive
        ));
    }

    public async Task<ProductModel.Response> AddProduct(ProductModel.Request request)
    {
        if (string.IsNullOrWhiteSpace(request.Sku) ||
            string.IsNullOrWhiteSpace(request.InternalCode) ||
            string.IsNullOrWhiteSpace(request.Name) ||
            string.IsNullOrWhiteSpace(request.Price.ToString()) ||
            string.IsNullOrWhiteSpace(request.Stock.ToString()))
        {
            throw new ArgumentException("Invalid values for product");
        }

        var exist = await _repository.First<Product>(p => p.Sku == request.Sku);
        if (exist != null) throw new DuplicatedEntityException($"A product with that Sku already exists {request.Sku}");
        if (exist != null) throw new DuplicatedEntityException($"A product with that Internal Code already exists {request.InternalCode}");

        var product = new Product(request.Sku,request.Name, request.Price, request.Stock, request.InternalCode);
        await _repository.Add(product);
        return new ProductModel.Response(product.Id, product.Sku, product.Name, product.Price, product.Stock, product.InternalCode, product.IsActive);
    }

    public async Task<ProductModel.Response> UpdateProduct(Guid id, ProductModel.Request request)
    {
        var product = await _repository.GetById<Product>(id);
        if (product == null)
            throw new System.ApplicationException("Producto no encontrado.");
        if (string.IsNullOrWhiteSpace(request.Sku) || string.IsNullOrWhiteSpace(request.Name))
            throw new ArgumentException("SKU y nombre son obligatorios.");
        product.Sku = request.Sku;
        product.InternalCode = request.InternalCode;
        product.Name = request.Name;
        product.Price = request.Price;
        product.Stock = request.Stock;
        var updated = await _repository.Update(product);
        return new ProductModel.Response(
            updated.Id,
            updated.Sku,
            updated.Name,
            updated.Price,
            updated.Stock,
            updated.InternalCode,
            updated.IsActive
        );
    }

    public async Task<ProductModel.Response?> DeactivateProduct(Guid id)
    {
        var product = await _repository.GetById<Product>(id);
        if (product == null)
            return null;

        product.IsActive = false;
        var updated = await _repository.Update(product);

        return new ProductModel.Response(
            updated.Id,
            updated.Sku,
            updated.Name,
            updated.Price,
            updated.Stock,
            updated.InternalCode,
            updated.IsActive
        );
    }


    /*  public async Task<Product> CreateProductAsync(ProductDto productDto)
  {
      var product = new Product
      {
          Sku = productDto.Sku,
          Name = productDto.Name,
          Price = productDto.Price,
          Stock = productDto.Stock,
          InternalCode = productDto.InternalCode,
          IsActive = productDto.IsActive
      };
      await _productRepository.Add(product);
      return new Product
      {
          Id = product.Id,
          Sku = product.Sku,
          Name = product.Name,
          Price = product.Price,
          Stock = product.Stock,
          IsActive = product.IsActive
      };
  }

  public async Task<Product?> UpdateProductAsync(Product productDto)
  {
      var product = await _productRepository.GetByIdAsync(productDto.Id);
      if (product == null) return null;

      product.Sku = productDto.Sku;
      product.Name = productDto.Name;
      product.Price = productDto.Price;
      product.Stock = productDto.Stock;
      product.InternalCode = productDto.InternalCode;
      product.IsActive = productDto.IsActive;

      await _productRepository.UpdateAsync(product);

      return product;
  }

  public async Task<bool> DeleteProductAsync(Guid id)
  {
      var product = await _productRepository.GetById<Product>(id);
      if (product == null) return false;

      await _productRepository.Delete(product);
      return true;
  }*/

}

