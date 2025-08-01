using Dsw2025Tpi.Application.Dtos;
using Dsw2025Tpi.Application.Exceptions;
using Dsw2025Tpi.Application.Interfaces;
using Dsw2025Tpi.Application.Validation;
using Dsw2025Tpi.Domain.Entities;
using Dsw2025Tpi.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Dsw2025Tpi.Application.Services
{
    public class OrdersManagementService : IOrdersManagementService
    {
        private readonly IRepository _repository;

        public OrdersManagementService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<OrderModel.Response> CreateOrderAsync(OrderModel.OrderRequest request)
        {
            OrderValidator.Validate(request);

            var orderItems = new List<OrderItem>();
            decimal totalAmount = 0;

            // Verificar stock y existencia de productos
            foreach (var item in request.OrderItems)
            {
                OrderItemValidator.Validate(item);
            }

            // Descontar stock y armar los ítems
            foreach (var item in request.OrderItems)
            {
                
                var product = await _repository.GetById<Product>(item.ProductId)
                    ?? throw new EntityNotFoundException($"Producto no encontrado: {item.ProductId}");

                if (product.StockQuantity < item.Quantity)
                    throw new InsufficientStockException($"Stock insuficiente para el producto: {product.Name}");

                product.StockQuantity -= item.Quantity;
                await _repository.Update(product);

                var orderItem = new OrderItem
                {
                    Id = Guid.NewGuid(),
                    ProductId = product.Id,
                    Quantity = item.Quantity,
                    Price = product.CurrentUnitPrice,
                    Description = product.Description,
                    Product = product
                };
                orderItems.Add(orderItem);
                totalAmount += product.CurrentUnitPrice * item.Quantity;
            }

            var order = new Order
            {
                
                Id = Guid.NewGuid(),
                CustomerId = request.CustomerId,
                ShippingAddress = request.ShippingAddress,
                BillingAddress = request.BillingAddress,
                Date = DateTime.UtcNow,
                Notes = request.Notes,
                TotalAmount = totalAmount,
                Status = OrderStatus.PENDING,
                OrderItems = orderItems
            };

            await _repository.Add(order);

            
            var responseItems = orderItems.Select(oi => new OrderItemModel.Response(
                oi.Id,
                oi.ProductId,
                oi.Product?.Name ?? "",
                oi.Product?.Description ?? "",
                oi.Quantity,
                oi.Price,
                oi.Price * oi.Quantity
            )).ToList();

            return new OrderModel.Response(
                order.Id,
                order.CustomerId ?? Guid.Empty,
                order.ShippingAddress,
                order.BillingAddress,
                order.Date,
                order.Notes,
                order.TotalAmount,
                order.Status.ToString(),
                responseItems
            );
        }

        public async Task<OrderModel.Response?> GetOrderById(Guid id)
        {
            
            var order = await _repository.GetById<Order>(id,nameof(Order.OrderItems), "OrderItems.Product")
             ?? throw new EntityNotFoundException("Orden no encontrada");

            var responseItems = order.OrderItems.Select(oi => new OrderItemModel.Response(
                oi.Id,
                oi.ProductId,
                oi.Product?.Name ?? "",
                oi.Product?.Description ?? "",
                oi.Quantity,
                oi.Price,
                oi.Price * oi.Quantity
            )).ToList();
            return new OrderModel.Response(
                order.Id,
                order.CustomerId ?? Guid.Empty,
                order.ShippingAddress,
                order.BillingAddress,
                order.Date,
                order.Notes,
                order.TotalAmount,
                order.Status.ToString(),
                responseItems
            );

        }

         public async Task<Dtos.PagedResult<OrderModel.Response>> GetAllOrders(int page, int pageSize, string? status, Guid? customerId = null)
        {
            var orders = (await _repository.GetAll<Order>("OrderItems", "OrderItems.Product"))?.ToList()
                ?? new List<Order>();

            if (customerId.HasValue && customerId.Value != Guid.Empty)
                orders = orders.Where(o => o.CustomerId == customerId.Value).ToList();

            if (!string.IsNullOrEmpty(status))
                orders = orders.Where(o => o.Status.ToString().Equals(status, StringComparison.OrdinalIgnoreCase)).ToList();

            var total = orders.Count;
            var items = orders
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(order =>
                {
                    var responseItems = order.OrderItems.Select(oi => new OrderItemModel.Response(
                        oi.Id,
                        oi.ProductId,
                        oi.Product?.Name ?? "",
                        oi.Product?.Description ?? "",
                        oi.Quantity,
                        oi.Price,
                        oi.Price * oi.Quantity
                    )).ToList();

                    return new OrderModel.Response(
                        order.Id,
                        order.CustomerId ?? Guid.Empty,
                        order.ShippingAddress,
                        order.BillingAddress,
                        order.Date,
                        order.Notes,
                        order.TotalAmount,
                        order.Status.ToString(),
                        responseItems
                    );
                })
                .ToList();

            return new Dtos.PagedResult<OrderModel.Response>(items, total, page, pageSize);
        }


        public async Task<OrderModel.Response> UpdateOrderStatus(Guid id, string status)
        {
            
            var order = await _repository.GetById<Order>(id, nameof(Order.OrderItems), "OrderItems.Product")
             ?? throw new EntityNotFoundException("Orden no encontrada");

            // Validar y actualizar el estado
            if (!Enum.TryParse<OrderStatus>(status, true, out var newStatus))
                throw new InvalidStatusException("Estado inválido.");

            order.Status = newStatus;
            var updated = await _repository.Update(order);

            var responseItems = updated.OrderItems.Select(oi => new OrderItemModel.Response(
                oi.Id,
                oi.ProductId,
                oi.Product?.Name ?? "",
                oi.Product?.Description ?? "",
                oi.Quantity,
                oi.Price,
                oi.Price * oi.Quantity
            )).ToList();

            return new OrderModel.Response(
                updated.Id,
                updated.CustomerId ?? Guid.Empty,
                updated.ShippingAddress,
                updated.BillingAddress,
                updated.Date,
                updated.Notes,
                updated.TotalAmount,
                updated.Status.ToString(),
                responseItems
            );
        }
    }
}


