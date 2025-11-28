using Dsw2025Tpi.Application.Dtos;
using Dsw2025Tpi.Application.Exceptions;
using Dsw2025Tpi.Application.Interfaces;
using Dsw2025Tpi.Application.Validations;
using Dsw2025Tpi.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static Dsw2025Tpi.Application.Dtos.LoginModel;
using ApplicationException=Dsw2025Tpi.Application.Exceptions.ApplicationException;

namespace Dsw2025Tpi.Application.Services;

public class AuthenticateService : IAuthenticateService
{
    private readonly IConfiguration _config;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly ICustomerService _customerService;

    public AuthenticateService(
        IConfiguration config,
        SignInManager<IdentityUser> signInManager,
        UserManager<IdentityUser> userManager,
        ICustomerService customerService)
    {
        _config = config;
        _userManager = userManager;
        _signInManager = signInManager;
        _customerService = customerService;
    }

    public string GenerateToken(string username, string role)
    {
        var jwtConfig = _config.GetSection("Jwt");
        var keyText = jwtConfig["Key"] ?? throw new ArgumentNullException("Jwt Key");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyText));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Role, role)
        };

        var token = new JwtSecurityToken(
            issuer: jwtConfig["Issuer"],
            audience: jwtConfig["Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(double.Parse(jwtConfig["ExpireInMinutes"] ?? "60")),
            signingCredentials: creds
            );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<LoginModel.ResponseLogin> Login(LoginModel.RequestLogin request)
    {
        var user = await _userManager.FindByNameAsync(request.Username);
        if (user == null)
        {
            throw new UnauthorizedException("Incorrect username or password.");
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
        if (!result.Succeeded)
        {
            throw new UnauthorizedException("Incorrect username or password.");
        }

        var roles = await _userManager.GetRolesAsync(user);
        var role = roles.FirstOrDefault() ?? throw new ApplicationException("User has not assigned role");

        var token = GenerateToken(request.Username, role);

        // Obtener CustomerId si es Client
        Guid? customerId = null;

        if (role.Equals("Client", StringComparison.OrdinalIgnoreCase))
        {
            var customer = await _customerService.GetByEmailAsync(user.Email!);
            customerId = customer?.Id;
        }

        var userDto = new UserDto(
            Id: user.Id,
            Username: user.UserName!,
            Email: user.Email!,
            Role: role,
            CustomerId: customerId
        );

        return new LoginModel.ResponseLogin(token, userDto);
    }

    public async Task<RegisterModel.ResponseRegister> Register(RegisterModel.RequestRegister model)
    {
        RegisterValidator.Validate(model);

        var existingUser = await _userManager.FindByNameAsync(model.Username);
        if (existingUser != null)
        {
            throw new DuplicatedEntityException("A user with this username or email already exists.");
        }

        // Si el usuario no existe, verificamos el email.
        existingUser = await _userManager.FindByEmailAsync(model.Email);
        if (existingUser != null)
        {
            throw new DuplicatedEntityException("A user with this username or email already exists.");
        }

        // FIX: Usar GetChildren para obtener la lista de roles
        var allowedRoles = _config.GetSection("Roles").GetChildren().Select(r => r.Value).ToList();

        var requestedRole = model.Role.Trim();

        if (!allowedRoles.Contains(requestedRole))
        {
            var errors = new List<ValidationError>
            {
                new ValidationError(
                    "Role must be one of the predefined roles.",
                    ValidationErrorCodes.RoleInvalid)
            };

            throw new ValidationException("The specified role is not valid.", errors);
        }

        var user = new IdentityUser { UserName = model.Username, Email = model.Email };
        var result = await _userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
        {
            var identityErrors = result.Errors
                .Select(e => new ValidationError(e.Description, ValidationErrorCodes.IdentityError))
                .ToList();

            throw new ValidationException("One or more Identity validation errors occurred.", identityErrors);
        }

        var roleToAssign = requestedRole;

        var roleResult = await _userManager.AddToRoleAsync(user, roleToAssign!);

        if (!roleResult.Succeeded)
        {
            // En caso de que la asignación de rol falle.
            throw new ApplicationException("Could not assign user role.");
        }

        if (requestedRole.Equals("Client", StringComparison.OrdinalIgnoreCase))
        {
            var customerRequest = new CustomerModel.CustomerRequest(
            Name: user.UserName!,
            Email: user.Email!,
            PhoneNumber: "Sin Especificar" // requerido
        );

            await _customerService.CreateCustomerAsync(customerRequest);
        }

        return new RegisterModel.ResponseRegister();

    }

}

