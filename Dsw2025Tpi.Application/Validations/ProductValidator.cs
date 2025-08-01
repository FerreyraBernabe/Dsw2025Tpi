using Dsw2025Tpi.Application.Dtos;
using Dsw2025Tpi.Application.Exceptions;
using System;

namespace Dsw2025Tpi.Application.Validation
{
    public static class ProductValidator
    {
        public static void Validate(ProductModel.Request request)
        {

            if (request == null)
                throw new BadRequestException("The product request body can not be null.");

            if (string.IsNullOrWhiteSpace(request.Sku))
                throw new BadRequestException("SKU is mandatory.");

            if (string.IsNullOrWhiteSpace(request.InternalCode))
                throw new BadRequestException("Internal Code is mandatory.");

            if (string.IsNullOrWhiteSpace(request.Name))
                throw new BadRequestException("Name is mandatory.");

            if (string.IsNullOrWhiteSpace(request.Description))
                throw new BadRequestException("Description is mandatory.");

            if (request.CurrentUnitPrice < 0)
                throw new BadRequestException("CurrentUnitPrice must be above zero.");

            if (request.StockQuantity < 0)
                throw new BadRequestException("Stock must be above zero.");
        }
    }

}
