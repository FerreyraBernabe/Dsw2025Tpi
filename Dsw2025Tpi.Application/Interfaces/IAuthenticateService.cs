﻿using Dsw2025Tpi.Application.Dtos;

namespace Dsw2025Tpi.Application.Interfaces
{
    public interface IAuthenticateService
    {
        string GenerateToken(string username, string role);
        Task<LoginModel.ResponseLogin> Login(LoginModel.RequestLogin request);
        Task<RegisterModel.ResponseRegister> Register(RegisterModel.RequestRegister model);
    }
}