using Dsw2025Tpi.Application.Dtos;
using Dsw2025Tpi.Application.Exceptions;
using Dsw2025Tpi.Application.Services;
using Dsw2025Tpi.Application.Validations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ApplicationException = Dsw2025Tpi.Application.Exceptions.ApplicationException;

namespace Dsw2025Tpi.Api.Controllers
{
        [ApiController]
        [Route("api/auth")]
        [Authorize]
        public class AuthenticateController : ControllerBase
        {
            private readonly UserManager<IdentityUser> _userManager;
            private readonly SignInManager<IdentityUser> _signInManager;
            private readonly JwtTokenService _jwtTokenService;

            public AuthenticateController(UserManager<IdentityUser> userManager,
                SignInManager<IdentityUser> signInManager,
                JwtTokenService jwtTokenService)
            {
                _userManager = userManager;
                _signInManager = signInManager;
                _jwtTokenService = jwtTokenService;
            }

            [HttpPost("login")]
            [AllowAnonymous]
            public async Task<IActionResult> Login([FromBody] LoginModel request)
            {
                var user = await _userManager.FindByNameAsync(request.Username);
                if (user == null)
                {
                    throw new UnauthorizedException("Usuario o contraseña incorrectos");
                }

                var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
                if (!result.Succeeded)
                {
                    throw new UnauthorizedException("Usuario o contraseña incorrectos");
                }

            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault() ?? throw new ApplicationException("User has not assigned role");


            var token = _jwtTokenService.GenerateToken(request.Username,role);
                return Ok(new { token });
            }

            [HttpPost("register")]
            [AllowAnonymous]
            public async Task<IActionResult> Register([FromBody] RegisterModel model)
            {
                RegisterValidator.Validate(model);

                var user = new IdentityUser { UserName = model.Username, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                var role = await _userManager.AddToRoleAsync(user, "Admin");

                 if (!result.Succeeded)
                    return BadRequest(result.Errors);

                // Opcional: enviar email de confirmación, etc.
                return Ok("Usuario registrado correctamente.");
            }
        }
    
}
