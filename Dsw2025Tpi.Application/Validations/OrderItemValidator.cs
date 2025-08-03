using Dsw2025Tpi.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValidationException=Dsw2025Tpi.Application.Exceptions.ValidationException;

namespace Dsw2025Tpi.Application.Validation
{
    public static class OrderItemValidator
    {
        public static void Validate(OrderItemModel.OrderItemRequest item)
        {
            var errors = new List<string>();

            if (item == null)
            {
                throw new InvalidOperationException("The Order Item cannot be null.");
            }
            else
            {
                if (item.ProductId == Guid.Empty)
                    errors.Add("Product is mandatory.");

                if (item.Quantity <= 0)
                    errors.Add("The quantity must be above zero.");

                if (errors.Any())
                    throw new ValidationException("One or more validation errors occurred.", errors);
            }

        }
    }
}

