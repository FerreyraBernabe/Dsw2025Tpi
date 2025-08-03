using Dsw2025Tpi.Application.Dtos;
using Dsw2025Tpi.Application.Exceptions;
using Dsw2025Tpi.Application.Interfaces;
using Dsw2025Tpi.Application.Validations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ApplicationException = Dsw2025Tpi.Application.Exceptions.ApplicationException;

namespace Dsw2025Tpi.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthenticateController : ControllerBase
{
  
    private readonly IAuthenticateService _service;

    public AuthenticateController(IAuthenticateService service,
        SignInManager<IdentityUser> signInManager)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }


    //Inicio de Sesión   
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel.RequestLogin request)
    {

        var token = await _service.Login(request);
        return Ok(token);
    }

    //Registro de usuario
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel.RequestRegister model)
    {
        await _service.Register(model);
        return Ok("New user successfully created.");
    }
}    
