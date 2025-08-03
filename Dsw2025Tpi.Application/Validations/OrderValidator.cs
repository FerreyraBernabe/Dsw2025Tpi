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
            var errors = new List<string>();

            if (request == null)
            {
               throw new InvalidOperationException("The order request body can not be null.");
            }
            else
            {
                if (request.CustomerId == Guid.Empty)
                {
                    errors.Add("CustomerID is mandatory.");
                }

                if (string.IsNullOrWhiteSpace(request.ShippingAddress) || request.ShippingAddress.Length > 256)
                {
                    errors.Add("The shipping address is required and cannot exceed 256 characters.");
                }

                if (string.IsNullOrWhiteSpace(request.BillingAddress) || request.BillingAddress.Length > 256)
                {
                    errors.Add("The billing address is required and cannot exceed 256 characters.");
                }

                if (request.OrderItems == null || request.OrderItems.Count == 0)
                {
                    errors.Add("Must include at least one item in the order.");
                }

                if (errors.Any())
                {
                    throw new ValidationException("One or more validation errors occurred.", errors);
                }

            }

        }
    }
    //public static class OrderValidator
    //{
    //    public static void Validate(OrderModel.OrderRequest request)
    //    {
    //        if (request == null)
    //            throw new InvalidOperationException("The order request body can not be null.");

    //        if (request.CustomerId == Guid.Empty)
    //            throw new InvalidOperationException("CustomerID is mandatory.");

    //        if (string.IsNullOrWhiteSpace(request.ShippingAddress) || request.ShippingAddress.Length > 256)
    //            throw new InvalidOperationException("The shipping address is required and cannot exceed 256 characters.");

    //        if (string.IsNullOrWhiteSpace(request.BillingAddress) || request.BillingAddress.Length > 256)
    //            throw new InvalidOperationException("The billing address is required and cannot exceed 256 characters.");

    //        if (request.OrderItems == null || request.OrderItems.Count == 0)
    //            throw new InvalidOperationException("Must include at least one item in the order.");
    //    }
    //}
}
