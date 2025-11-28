using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsw2025Tpi.Application.Exceptions;


    public class NoContentException : ApplicationException
    {
        public int Code { get; }

        public NoContentException(string message, int code) : base(message)
        {
            Code = code;
        }
    }

