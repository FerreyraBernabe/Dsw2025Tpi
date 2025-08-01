using Dsw2025Tpi.Application.Dtos;
using System.Linq.Dynamic.Core;

namespace Dsw2025Tpi.Application.Interfaces
{
    public interface IOrdersManagementService
    {
        Task<OrderModel.Response> CreateOrderAsync(OrderModel.OrderRequest request);
        Task<OrderModel.Response?> GetOrderById(Guid id);
        Task<Dtos.PagedResult<OrderModel.Response>> GetAllOrders(int page, int pageSize, string? status, Guid? customerId);
        Task<OrderModel.Response> UpdateOrderStatus(Guid id, string status);
    }
}
