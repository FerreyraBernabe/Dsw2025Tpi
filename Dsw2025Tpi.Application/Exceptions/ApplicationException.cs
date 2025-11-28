using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsw2025Tpi.Application.Exceptions
{
    public class ApplicationException : Exception
    {
        public int Code { get; }

        public ApplicationException(string message, int code = 0) : base(message)
        {
            Code = code;
        }
    }
}
