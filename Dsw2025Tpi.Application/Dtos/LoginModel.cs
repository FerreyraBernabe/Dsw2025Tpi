using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsw2025Tpi.Application.Dtos
{
    public record LoginModel
    {
        public record RequestLogin(string Username, string Password);
        public record ResponseLogin(string Token, UserDto User);

        public record UserDto(
            string Id,
            string Username,
            string Email,
            string Role,
            Guid? CustomerId
        );


    }
}