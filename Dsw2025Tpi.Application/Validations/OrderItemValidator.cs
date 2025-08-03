using Dsw2025Tpi.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsw2025Tpi.Application.Validation
{
    public static class OrderItemValidator
    {
        public static void Validate(OrderItemModel.OrderItemRequest item)
        {
            if (item == null)
                throw new InvalidOperationException("The Order Item can not be null.");

           if (item.ProductId == Guid.Empty)
                throw new InvalidOperationException("Product is mandatory.");

            if (item.Quantity <= 0)
                throw new InvalidOperationException("The quantity must be above zero.");
        }
    }
}

