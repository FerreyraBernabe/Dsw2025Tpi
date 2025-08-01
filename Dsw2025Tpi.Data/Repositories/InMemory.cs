using Dsw2025Tpi.Domain.Entities;
using Dsw2025Tpi.Domain.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Dsw2025Tpi.Data.Repositories;

public class InMemory : IRepository
{
    private List<Product>? _products;
    private List<Customer>? _customers;
    private List<Order>? _orders;
    private List<OrderItem>? _orderItems;

    public InMemory()
    {
        LoadProducts();
        LoadCustomers();
        LoadOrders();
        LoadOrderItems();
    }
    #region Loads
    private void LoadProducts()
    {
        var json = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Sources\\products.json"));
        _products = JsonSerializer.Deserialize<List<Product>>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        });
    }
    private void LoadCustomers()
    {
        var json = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Sources\\customers.json"));
        _customers = JsonSerializer.Deserialize<List<Customer>>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        });
    }

    private void LoadOrders()
    {
        var json = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Sources\\orders.json"));
        _orders = JsonSerializer.Deserialize<List<Order>>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        });
    }
    private void LoadOrderItems()
    {
        var json = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Sources\\orderitems.json"));
        _orderItems = JsonSerializer.Deserialize<List<OrderItem>>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        });
    }

    #endregion

    private List<T>? GetList<T>() where T : EntityBase
    {
        return typeof(T).Name switch
        {
            nameof(Product) => _products as List<T>,
            nameof(Customer) => _customers as List<T>,
            nameof(Order) => _orders as List<T>,
            nameof(OrderItem) => _orderItems as List<T>,
            _ => throw new NotSupportedException(),
        };
    }

    public async Task<T?> GetById<T>(Guid id, params string[] include) where T : EntityBase
    {
       
        var entity = GetList<T>()?.FirstOrDefault(e => e.Id == id);

        // Si es una orden, cargar los ítems y productos asociados
        if (entity is Order order)
        {
            // Cargar los ítems de la orden
            order.OrderItems = _orderItems?
                .Where(oi => oi.OrderId == order.Id)
                .ToList() ?? new List<OrderItem>();

            // Cargar los productos de cada ítem
            foreach (var item in order.OrderItems)
            {
                item.Product = _products?.FirstOrDefault(p => p.Id == item.ProductId);
            }
        }

        return await Task.FromResult(entity);
    }
        
       // return await Task.FromResult(GetList<T>()?.FirstOrDefault(e => e.Id == id));

        

         
    

    public async Task<IEnumerable<T>?> GetAll<T>(params string[] include) where T : EntityBase
    {
        return await Task.FromResult(GetList<T>());
    }

    public async Task<T> Add<T>(T entity) where T : EntityBase
    {
        GetList<T>()?.Add(entity);
        return await Task.FromResult(entity);
    }

    public Task<T> Update<T>(T entity) where T : EntityBase
    {
        var list = GetList<T>();
        if (list == null)
            throw new InvalidOperationException($"No se encontró la lista para el tipo {typeof(T).Name}.");

        var index = list.FindIndex(e => e.Id == entity.Id);
        if (index == -1)
            throw new KeyNotFoundException($"No se encontró la entidad con Id {entity.Id} para actualizar.");

        list[index] = entity;
        return Task.FromResult(entity);
    }

    public Task<T> Delete<T>(T entity) where T : EntityBase
    {
        var list = GetList<T>();
        if (list == null)
            throw new InvalidOperationException($"No se encontró la lista para el tipo {typeof(T).Name}.");

        var removed = list.RemoveAll(e => e.Id == entity.Id);
        if (removed == 0)
            throw new KeyNotFoundException($"No se encontró la entidad con Id {entity.Id} para eliminar.");

        return Task.FromResult(entity);
    }

    public async Task<T?> First<T>(Expression<Func<T, bool>> predicate, params string[] include) where T : EntityBase
    {
        var product = GetList<T>()?.FirstOrDefault(predicate.Compile());
        return await Task.FromResult(product);
    }

    public async Task<IEnumerable<T>?> GetFiltered<T>(Expression<Func<T, bool>> predicate, params string[] include) where T : EntityBase
    {
        var products = GetList<T>()?.Where(predicate.Compile());
        return await Task.FromResult(products);
    }
}



