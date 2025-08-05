using Dsw2025Tpi.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ValidationException = Dsw2025Tpi.Application.Exceptions.ValidationException;

namespace Dsw2025Tpi.Application.Validations
{
    public static class RegisterValidator
    {
        public static void Validate(RegisterModel.RequestRegister model)
        {
            var errors = new List<string>();

            if (model == null)
            {
                throw new InvalidOperationException("The register request body cannot be null.");
            }
            else
            {
                if (string.IsNullOrWhiteSpace(model.Username))
                    errors.Add("Username is mandatory.");

                if (model.Username.Length < 4)
                    errors.Add("Username must be at least 4 characters long.");

                if (string.IsNullOrWhiteSpace(model.Email))
                    errors.Add("Email is mandatory.");

                if (!model.Email.Contains('@'))
                {
                    errors.Add("Email format is not valid: '@' symbol is missing.");
                }
                else if (!model.Email.Split('@')[1].Contains('.'))
                {
                    errors.Add("Email format is not valid: domain must contain a '.' (e.g., '.com').");
                }
                else
                {
                    var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+(\.[^@\s]+)*$", RegexOptions.IgnoreCase);
                    if (!emailRegex.IsMatch(model.Email))
                    {
                        errors.Add("Email format is not valid.");
                    }
                }

                //if (!IsValidEmail(model.Email))
                //    errors.Add("Email format is not valid.");

                if (string.IsNullOrWhiteSpace(model.Password))
                {
                    errors.Add("Password is mandatory.");
                }
                else if (model.Password.Length < 8)
                {
                    errors.Add("Password must be at least 8 characters long.");
                }

                var hasNumber = new Regex(@"[0-9]+");
                var hasUpperChar = new Regex(@"[A-Z]+");
                var hasLowerChar = new Regex(@"[a-z]+");
                var hasMiniSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");

                if (!hasNumber.IsMatch(model.Password))
                    errors.Add("Password must contain at least one number.");

                if (!hasUpperChar.IsMatch(model.Password))
                    errors.Add("Password must contain at least one uppercase letter.");

                if (!hasLowerChar.IsMatch(model.Password))
                    errors.Add("Password must contain at least one lowercase letter.");

                if (!hasMiniSymbols.IsMatch(model.Password))
                    errors.Add("Password must contain at least one special character.");

                if (errors.Any())
                    throw new ValidationException("One or more validation errors occurred.", errors);
            }

        }
    }
}

    //    private static bool IsValidEmail(string email)
    //    {
    //        try
    //        {
    //            var addr = new System.Net.Mail.MailAddress(email);
    //            return addr.Address == email;
    //        }
    //        catch
    //        {
    //            return false;
    //        }
    //    }