using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsw2025Tpi.Application.Exceptions
{
    public static class ValidationErrorCodes
    {
        // RegisterValidator
        public const int RegisterUsername = 2001;
        public const int RegisterEmailEmpty = 2002;
        public const int RegisterEmailFormat = 2003;
        public const int RegisterPasswordLength = 2004;
        public const int RegisterPasswordNumber = 2005;
        public const int RegisterPasswordUpper = 2006;
        public const int RegisterPasswordLower = 2007;
        public const int RegisterPasswordSpecial = 2008;

        // ProductValidator
        public const int ProductSku = 3001;
        public const int ProductInternalCode = 3002;
        public const int ProductNameEmpty = 3003;
        public const int ProductNameFormat = 3004;
        public const int ProductDescriptionLength = 3005;
        public const int ProductDescriptionFormat = 3006;
        public const int ProductPrice = 3007;
        public const int ProductStock = 3008;

        // OrderValidator
        public const int OrderCustomerId = 4001;
        public const int OrderShippingAddressEmpty = 4002;
        public const int OrderShippingAddressFormat = 4003;
        public const int OrderShippingAddressLength = 4004;
        public const int OrderBillingAddressEmpty = 4005;
        public const int OrderBillingAddressFormat = 4006;
        public const int OrderBillingAddressLength = 4007;
        public const int OrderNotes= 4008;
        public const int OrderItems = 4009;

        // OrderItemValidator
        public const int OrderItemProduct = 5001;
        public const int OrderItemQuantityPositive = 5002;
        public const int OrderItemQuantityMax = 5003;
        public const int OrderItemQuantityWhole = 5004;

        // CustomerValidator
        public const int CustomerName = 6001;
        public const int CustomerEmail = 6002;
        public const int CustomerPhone = 6003;

        // Roles / Authorization
        public const int RoleInvalid = 7001;

        // Identity / UserManager
        public const int IdentityError = 8001;
    }
}
