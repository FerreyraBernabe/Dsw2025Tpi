using Dsw2025Tpi.Application.Dtos;
using System.Linq.Dynamic.Core;

namespace Dsw2025Tpi.Application.Interfaces
{
    public interface IOrdersManagementService
    {
        Task<OrderModel.Response> CreateOrderAsync(OrderModel.OrderRequest request);
        Task<OrderModel.Response?> GetOrderById(Guid id);
        Task<OrderModel.GetOrderResponse> GetAllOrders(OrderModel.GetOrder request);
        Task<OrderModel.Response> UpdateOrderStatus(Guid id, string status);
    }
}
