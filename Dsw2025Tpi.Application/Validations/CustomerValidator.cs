using Dsw2025Tpi.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValidationException=Dsw2025Tpi.Application.Exceptions.ValidationException;

namespace Dsw2025Tpi.Application.Validation
{
    public static class CustomerValidator
    {
        public static void Validate(CustomerModel.Request request)
        {
            var errors = new List<string>();

            if (request == null)
            {
                throw new InvalidOperationException("The customer request body cannot be null.");
            }
            else
            {
                if (string.IsNullOrWhiteSpace(request.Name))
                    errors.Add("Name is mandatory.");

                if (request.Name.Length < 3 || request.Name.Length > 100)
                    errors.Add("Name must be between 3 and 100 characters long.");

                if (string.IsNullOrWhiteSpace(request.Email))
                    errors.Add("Email is mandatory.");

                if (string.IsNullOrWhiteSpace(request.PhoneNumber))
                    errors.Add("PhoneNumber is mandatory.");

                if (request.PhoneNumber.Length < 10 || request.PhoneNumber.Length > 15)
                    errors.Add("PhoneNumber must be between 10 and 15 characters long.");

                if (errors.Any())
                    throw new ValidationException("One or more validation errors occurred.", errors);
            }

        }
    }
}

