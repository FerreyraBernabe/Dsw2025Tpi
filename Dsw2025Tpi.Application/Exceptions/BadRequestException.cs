using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Dsw2025Tpi.Application.Exceptions
{
    public class BadRequestException : ApplicationException
    {
        public int Code { get; }

        public BadRequestException(string message, int code) : base(message)
        {
            Code = code;
        }
    }
}

