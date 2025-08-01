using Dsw2025Tpi.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsw2025Tpi.Application.Validations
{
    public static class RegisterValidator
    {
       public static void Validate(RegisterModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Username))
                throw new InvalidOperationException("Username is mandatory.");

            if (string.IsNullOrWhiteSpace(model.Email))
                throw new InvalidOperationException("Email is mandatory.");

            if (!IsValidEmail(model.Email))
                throw new InvalidOperationException("Email format is not valid.");

        }

        private static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
