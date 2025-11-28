using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsw2025Tpi.Application.Exceptions
{
    public static class ExceptionErrorCodes
    {

        //Bad Request
        //Bad Request 9000
        public const int InvalidStatus = 9001;
        public const int EmailRequired = 9002;

        //Duplicate Entity 9100
        public const int DuplicateUserEmail = 9101;
        public const int DuplicateUSKU = 9102;
        public const int DuplicateInternalCode = 9103;

        //ApplicationException 9200
        public const int UserHasNotAssignedRole = 9201;
        public const int CouldntAssignedUser = 9202;

        //Unauthorized 9300
        public const int InvalidUserPassword = 9301;

        //Invalid Operation 9400
        public const int InsufficientStock = 9401;
        public const int CannotChangeOrderStatus = 9402;
        public const int OrderItemsCannotBeNull = 9403;
        public const int BodyNotNull = 9404;

        //Precondition Failed 9700
        public const int InvalidCustomer = 9701;

        //Not Found
        //Entity Not Found 9500
        public const int CustomerNotFound = 9501;
        public const int ProductNotFound = 9502;
        public const int OrderNotFound = 9503;
        public const int OrderItemNotFound = 9504;

        //No Content 9600
        public const int NoProducts = 9601;

    }
}
