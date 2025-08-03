using Dsw2025Tpi.Application.Dtos;
using Dsw2025Tpi.Application.Exceptions;
using System;
using ValidationException = Dsw2025Tpi.Application.Exceptions.ValidationException;

namespace Dsw2025Tpi.Application.Validation
{
    public static class ProductValidator
    {
            public static void Validate(ProductModel.Request request)
            {
                var errors = new List<string>();

                if (request == null)
                {
                    throw new InvalidOperationException("The product request body cannot be null.");
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(request.Sku))
                        errors.Add("SKU is mandatory.");

                    if (string.IsNullOrWhiteSpace(request.InternalCode))
                        errors.Add("Internal Code is mandatory.");

                    if (string.IsNullOrWhiteSpace(request.Name))
                        errors.Add("Name is mandatory.");

                    if (string.IsNullOrWhiteSpace(request.Description))
                        errors.Add("Description is mandatory.");

                    if (request.CurrentUnitPrice < 0)
                        errors.Add("CurrentUnitPrice must be above zero.");

                    if (request.StockQuantity < 0)
                        errors.Add("Stock must be above zero.");

                    if (errors.Any())
                        throw new ValidationException("One or more validation errors occurred.", errors);
                }
                               
            }
        
    }

}
