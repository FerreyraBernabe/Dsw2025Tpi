using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsw2025Tpi.Application.Exceptions
{

    public class ValidationError
    {
        public string Message { get; set; } = string.Empty;
        public int Code { get; set; }

        public ValidationError(string message, int code)
        {
            Message = message;
            Code = code;
        }
    }

    public class ValidationException : Exception
    {
        public List<ValidationError> Errors { get; }

        public ValidationException(string message, List<ValidationError> errors)
            : base(message)
        {
            Errors = errors ?? new List<ValidationError>();
        }
    }
}

//namespace Dsw2025Tpi.Application.Exceptions
//{



//    public class ValidationException : Exception
//    {
//        public List<string> Errors { get; }

//        public ValidationException(string message, List<string> errors)
//            : base(message)
//        {
//            Errors = errors ?? new List<string>();
//        }
//    }
//}
