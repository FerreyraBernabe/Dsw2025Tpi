using Dsw2025Tpi.Application.Dtos;
using Dsw2025Tpi.Application.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValidationException = Dsw2025Tpi.Application.Exceptions.ValidationException;

namespace Dsw2025Tpi.Application.Validation
{

    public static class OrderValidator
    {
        public static void Validate(OrderModel.OrderRequest request)
        {
            var errors = new List<ValidationError>();

            if (request == null)
            {
               throw new InvalidOperationException("The order request body can not be null.");
            }
            else
            {
                if (request.CustomerId == Guid.Empty)
                    errors.Add(new ValidationError("CustomerID is mandatory.", ValidationErrorCodes.OrderCustomerId));

                if (string.IsNullOrWhiteSpace(request.ShippingAddress))
                    errors.Add(new ValidationError("The Shipping Address is required.", ValidationErrorCodes.OrderShippingAddressEmpty));
                else if (request.ShippingAddress.Equals("string", StringComparison.OrdinalIgnoreCase))
                    errors.Add(new ValidationError("Shipping Address cannot be 'string'.", ValidationErrorCodes.OrderShippingAddressFormat));
                else if (request.ShippingAddress.StartsWith(' ') || request.ShippingAddress.EndsWith(' '))
                    errors.Add(new ValidationError("The Shipping Address cannot begin or end with a blank space.", ValidationErrorCodes.OrderShippingAddressFormat));

                if (request.ShippingAddress.Length > 256)
                    errors.Add(new ValidationError("The shipping address cannot exceed 256 characters.", ValidationErrorCodes.OrderShippingAddressLength));

                if (string.IsNullOrWhiteSpace(request.BillingAddress))
                    errors.Add(new ValidationError("The Billing Address is required.", ValidationErrorCodes.OrderBillingAddressEmpty));
                else if (request.BillingAddress.Equals("string", StringComparison.OrdinalIgnoreCase))
                    errors.Add(new ValidationError("Billing Address cannot be 'string'.", ValidationErrorCodes.OrderBillingAddressFormat));
                else if (request.BillingAddress.StartsWith(' ') || request.BillingAddress.EndsWith(' '))
                    errors.Add(new ValidationError("The Billing Address cannot begin or end with a blank space.", ValidationErrorCodes.OrderBillingAddressFormat));

                if (request.BillingAddress.Length > 256)
                    errors.Add(new ValidationError("The billing address cannot exceed 256 characters.", ValidationErrorCodes.OrderBillingAddressLength));

                if (!(string.IsNullOrWhiteSpace(request.Notes)) && (request.Notes.Equals("string", StringComparison.OrdinalIgnoreCase)))
                    errors.Add(new ValidationError("Notes cannot be 'string'.", ValidationErrorCodes.OrderNotes));

                if (request.OrderItems == null || request.OrderItems.Count == 0)
                    errors.Add(new ValidationError("Must include at least one item in the order.", ValidationErrorCodes.OrderItems));

                if (errors.Any())
                    throw new ValidationException("One or more validation errors occurred.", errors);

            }

        }
    }
}
