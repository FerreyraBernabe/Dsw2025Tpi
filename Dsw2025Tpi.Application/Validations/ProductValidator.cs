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
            var errors = new List<ValidationError>();


            if (request == null)
                {
                    throw new InvalidOperationException("The product request body cannot be null.");
                }
                else
                {

                    if (string.IsNullOrWhiteSpace(request.Sku)|| !Regex.IsMatch(request.Sku, @"^SKU-[0-9]{3,10}$"))
                    errors.Add(new ValidationError("The SKU must start with 'SKU-' followed by 3 to 10 digits.",ValidationErrorCodes.ProductSku));

                if (string.IsNullOrWhiteSpace(request.InternalCode)|| !Regex.IsMatch(request.InternalCode, @"^INT-[0-9]{3,10}$"))
                    errors.Add(new ValidationError("The internal code must start with 'INT-' followed by 3 to 10 digits.",ValidationErrorCodes.ProductInternalCode));

                if (string.IsNullOrWhiteSpace(request.Name)|| request.Name.Length < 3 || request.Name.Length > 100)
                    errors.Add(new ValidationError("Name must be between 3 and 100 characters long.",ValidationErrorCodes.ProductNameEmpty));
                else if (request.Name.Equals("string", StringComparison.OrdinalIgnoreCase))
                    errors.Add(new ValidationError("Name cannot be 'string'.", ValidationErrorCodes.ProductNameFormat));
                else if (request.Name.StartsWith(' ') || request.Name.EndsWith(' '))
                    errors.Add(new ValidationError("Name cannot begin or end with a blank space.",ValidationErrorCodes.ProductNameFormat));

                if (string.IsNullOrWhiteSpace(request.Description)|| request.Description.Length < 10 || request.Description.Length > 500)
                    errors.Add(new ValidationError("Description must be between 10 and 500 characters long.",ValidationErrorCodes.ProductDescriptionLength));
                else if (request.Name.StartsWith(' ') || request.Name.EndsWith(' '))
                    errors.Add(new ValidationError("Description cannot begin or end with a blank space.",ValidationErrorCodes.ProductDescriptionFormat));

                if (request.CurrentUnitPrice <= 0)
                    errors.Add(new ValidationError("CurrentUnitPrice cannot be negative or zero.",ValidationErrorCodes.ProductPrice));

                if (request.StockQuantity <= 0)
                    errors.Add(new ValidationError("Stock cannot be negative or zero.",ValidationErrorCodes.ProductStock));

                if (errors.Any())
                        throw new ValidationException("One or more validation errors occurred.", errors);
                }
                               
            }
        
    }

}
