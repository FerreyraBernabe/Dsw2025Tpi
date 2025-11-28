using Dsw2025Tpi.Application.Dtos;
using Dsw2025Tpi.Application.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InvalidOperationException = Dsw2025Tpi.Application.Exceptions.InvalidOperationException;
using ValidationException=Dsw2025Tpi.Application.Exceptions.ValidationException;

namespace Dsw2025Tpi.Application.Validation
{
    public static class OrderItemValidator
    {
        public static void Validate(OrderItemModel.OrderItemRequest item)
        {
            var errors = new List<ValidationError>();

            if (item == null)
            {
                throw new InvalidOperationException("The Order Item cannot be null.",ExceptionErrorCodes.OrderItemsCannotBeNull);
            }
            else
            {
                if (item.ProductId == Guid.Empty)
                    errors.Add(new ValidationError("Product is mandatory.", ValidationErrorCodes.OrderItemProduct));

                if (item.Quantity <= 0)
                    errors.Add(new ValidationError("The quantity must be above zero.", ValidationErrorCodes.OrderItemQuantityPositive));

                if (item.Quantity > 100)
                    errors.Add(new ValidationError("The quantity cannot exceed 100 units.", ValidationErrorCodes.OrderItemQuantityMax));

                if (item.Quantity % 1 != 0)
                    errors.Add(new ValidationError("The quantity must be a whole number.", ValidationErrorCodes.OrderItemQuantityWhole));

                if (errors.Any())
                    throw new ValidationException("One or more validation errors occurred.", errors);
            }

        }
    }
}


//namespace Dsw2025Tpi.Application.Validation
//{
//    public static class OrderItemValidator
//    {
//        public static void Validate(OrderItemModel.OrderItemRequest item)
//        {
//            var errors = new List<string>();

//            if (item == null)
//            {
//                throw new InvalidOperationException("The Order Item cannot be null.");
//            }
//            else
//            {
//                if (item.ProductId == Guid.Empty)
//                    errors.Add("Product is mandatory.");

//                if (item.Quantity <= 0)
//                    errors.Add("The quantity must be above zero.");

//                if (item.Quantity > 100)
//                    errors.Add("The quantity cannot exceed 100 units.");

//                if (item.Quantity % 1 != 0)
//                    errors.Add("The quantity must be a whole number.");

//                if (errors.Any())
//                    throw new ValidationException("One or more validation errors occurred.", errors);
//            }

//        }
//    }
//}

