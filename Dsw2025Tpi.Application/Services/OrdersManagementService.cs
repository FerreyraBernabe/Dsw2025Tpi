using Dsw2025Tpi.Application.Dtos;
using Dsw2025Tpi.Application.Exceptions;
using Dsw2025Tpi.Application.Interfaces;
using Dsw2025Tpi.Application.Validation;
using Dsw2025Tpi.Domain.Entities;
using Dsw2025Tpi.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
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

            var customer = await _repository.GetById<Customer>(request.CustomerId)
                    ?? throw new EntityNotFoundException($"Customer not found: {request.CustomerId}");

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
                    ?? throw new EntityNotFoundException($"Product not found: {item.ProductId}");
                
                if(!product.IsActive)
                    throw new EntityNotFoundException($"The following product is not available: {product.Id}");

                if (product.StockQuantity < item.Quantity)
                    throw new InvalidOperationException($"Insufficient stock for the product: {product.Id}");

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
                order.CustomerId,
                order.ShippingAddress,
                order.BillingAddress,
                order.Date,
                order.Notes ?? "",
                order.TotalAmount,
                order.Status.ToString(),
                responseItems
            );
        }

        public async Task<OrderModel.Response?> GetOrderById(Guid id)
        {

            var order = await _repository.GetById<Order>(id, nameof(Order.OrderItems), "OrderItems.Product");
            if (order == null) throw new EntityNotFoundException($"Order not found: {id}");

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
                order.CustomerId,
                order.ShippingAddress,
                order.BillingAddress,
                order.Date,
                order.Notes ?? "",
                order.TotalAmount,
                order.Status.ToString(),
                responseItems
            );

        }

        public async Task<OrderModel.GetOrderResponse> GetAllOrders(OrderModel.GetOrder request)
        {
            var orders = (await _repository.GetAll<Order>("OrderItems", "OrderItems.Product"))?.ToList()
                ?? new List<Order>();

            if (request.CustomerId.HasValue && request.CustomerId.Value != Guid.Empty)
            {
                // Verificar si el cliente existe
                var customer = await _repository.GetById<Customer>((Guid)request.CustomerId)
                   ?? throw new EntityNotFoundException($"Customer not found: {request.CustomerId}");

                orders = orders.Where(o => o.CustomerId == request.CustomerId.Value).ToList();
            }

            if (!string.IsNullOrEmpty(request.Status))  {
                if (!Enum.TryParse<OrderStatus>(request.Status, true, out var newStatus)) {
                    throw new InvalidOperationException("Invalid Status. Allowed status: PENDING, PROCESSING, SHIPPED, DELIVERED, CANCELLED");
                }
                orders = orders.Where(o => o.Status.ToString().Equals(request.Status, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            int page = request.Page ?? 1; // Usar valores por defecto
            int pageSize = request.PageSize ?? 10; 

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
                        order.CustomerId,
                        order.ShippingAddress,
                        order.BillingAddress,
                        order.Date,
                        order.Notes ?? "",
                        order.TotalAmount,
                        order.Status.ToString(),
                        responseItems
                    );
                })
                .ToList().OrderBy(o=>o.Date);

            return new OrderModel.GetOrderResponse(items, total, page, pageSize);
        }


        public async Task<OrderModel.Response> UpdateOrderStatus(Guid id, string status)
        {
            var order = await _repository.GetById<Order>(id, nameof(Order.OrderItems), "OrderItems.Product")
             ?? throw new EntityNotFoundException($"Order not found: {id}");

            // Validar y actualizar el estado
            if (string.IsNullOrWhiteSpace(status))
                throw new BadRequestException("You must specify a valid status. Allowed status: PENDING, PROCESSING, SHIPPED, DELIVERED, CANCELLED");

            if (!Enum.TryParse<OrderStatus>(status, true, out var newStatus))
                throw new InvalidOperationException("Invalid Status. Allowed status: PENDING, PROCESSING, SHIPPED, DELIVERED, CANCELLED");

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
                updated.CustomerId,
                updated.ShippingAddress,
                updated.BillingAddress,
                updated.Date,
                updated.Notes ?? "",
                updated.TotalAmount,
                updated.Status.ToString(),
                responseItems
            );
        }
    }
}


