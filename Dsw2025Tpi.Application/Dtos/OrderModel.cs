using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsw2025Tpi.Application.Dtos
{
    public record OrderModel
    {
        public record OrderRequest(Guid CustomerId, string ShippingAddress, string BillingAddress, 
            string? Notes, List<OrderItemModel.OrderItemRequest> OrderItems);
        public record Response(Guid Id, Guid CustomerId, string ShippingAddress, string BillingAddress, 
            DateTime Date, string? Notes, decimal TotalAmount, string Status,
            List<OrderItemModel.Response> OrderItems);

    }
}
