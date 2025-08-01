using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsw2025Tpi.Application.Dtos
{
    public record OrderItemModel
    {
        public record OrderItemRequest(Guid ProductId,int Quantity);
        public record Response(Guid Id, Guid ProductId, string ProductName, string? ProductDescription, int Quantity, decimal Price, decimal Subtotal);


    }
}
