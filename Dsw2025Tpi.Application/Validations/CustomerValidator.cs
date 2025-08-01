using Dsw2025Tpi.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsw2025Tpi.Application.Validation
{
    public static class CustomerValidator
    {
        public static void Validate(CustomerModel.Request request)
        {
            if (request == null)
                throw new InvalidOperationException("TThe customer request body can not be null.");

            if (string.IsNullOrWhiteSpace(request.Name))
                throw new InvalidOperationException("Name is mandatory.");

            if (string.IsNullOrWhiteSpace(request.Email))
                throw new InvalidOperationException("Email is mandatory.");

            if (string.IsNullOrWhiteSpace(request.PhoneNumber))
                throw new InvalidOperationException("PhoneNumber is mandatory.");
        }
    }
}

