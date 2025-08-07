using Dsw2025Tpi.Application.Dtos;
using Dsw2025Tpi.Application.Exceptions;
using System;
using System.Text.RegularExpressions;
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
                    else if (!Regex.IsMatch(request.Sku, @"^SKU-[0-9]{3,10}$"))
                        errors.Add("The SKU must start with 'SKU-' followed by 3 to 10 digits.");

                    if (string.IsNullOrWhiteSpace(request.InternalCode))
                        errors.Add("Internal Code is mandatory.");
                    else if (!Regex.IsMatch(request.InternalCode, @"^INT-[0-9]{3,10}$"))
                        errors.Add("The internal code must start with 'INT-' followed by 3 to 10 digits.");

                    if (string.IsNullOrWhiteSpace(request.Name))
                        errors.Add("Name is mandatory.");
                    else if (request.Name.Length < 3 || request.Name.Length > 100)
                        errors.Add("Name must be between 3 and 100 characters long.");
                    else if (request.Name.Equals("string", StringComparison.OrdinalIgnoreCase))
                        errors.Add("Name cannot be 'string'.");
                    else if (request.Name.StartsWith(' ') || request.Name.EndsWith(' '))
                        errors.Add("Name cannot begin or end with a blank space.");

                    if (string.IsNullOrWhiteSpace(request.Description))
                        errors.Add("Description is mandatory.");
                    else if (request.Description.Length < 10 || request.Description.Length > 500)
                        errors.Add("Description must be between 10 and 500 characters long.");
                    else if (request.Name.StartsWith(' ') || request.Name.EndsWith(' '))
                        errors.Add("Description cannot begin or end with a blank space.");

                    if (request.CurrentUnitPrice <= 0)
                        errors.Add("CurrentUnitPrice cannot be negative or zero.");
                
                    if (request.StockQuantity <= 0)
                        errors.Add("Stock cannot be negative or zero.");

                    if (errors.Any())
                        throw new ValidationException("One or more validation errors occurred.", errors);
                }
                               
            }
        
    }

}
