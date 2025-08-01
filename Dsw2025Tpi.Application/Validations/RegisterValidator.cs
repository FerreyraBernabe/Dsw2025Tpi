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
                throw new InvalidOperationException("El nombre de usuario es obligatorio.");

            if (string.IsNullOrWhiteSpace(model.Email))
                throw new InvalidOperationException("El email es obligatorio.");

            if (!IsValidEmail(model.Email))
                throw new InvalidOperationException("El formato del email no es válido.");

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
