using Application.Common.Extensions.Validation;
using Application.DTOs;
using Application.ViewModels.Login;
using Domain.Interfaces;
using Domain.Models.Employee;
using Domain.Services;
using Domain.ValueObjects;
using FluentValidation;
using Infrastructure.Common.ConfigModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Application.CQRS.EmployeesAuth.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, BaseMessageDto>
{
    private readonly ILogger<LoginCommandHandler> _logger;
    private readonly IValidator<LoginCommand> _validator;
    private readonly JwtConfig _jwtConfig;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenProvider _tokenProvider;
    public LoginCommandHandler(ILogger<LoginCommandHandler> logger, IValidator<LoginCommand> validator, JwtConfig jwtConfig, IUnitOfWork unitOfWork, ITokenProvider tokenProvider)
    {
        _logger = logger;
        _jwtConfig = jwtConfig;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _tokenProvider = tokenProvider;
    }
    public async Task<BaseMessageDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {            
        _logger.LogInformation("Handling AddCompanyCommand");
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {                
            _logger.LogWarning(validationResult.Errors.GetErrorString());
            return new()
            {
                Errors = validationResult.Errors.GetErrorsStringList()
            };
        }
        Employee employee = await _unitOfWork
            .EmployeeRepository
            .GetByEmailAsync(request.CredentialsDto.Email!);
        // LoginToken loginToken = employee.Login(
        //     request.CredentialsDto.Password!,
        //     _jwtConfig.TokenKey,
        //     _jwtConfig.Audience,
        //     _jwtConfig.Issuer
        // );
        LoginToken loginToken = employee.Login(request.CredentialsDto.Password!, _tokenProvider);
        await _unitOfWork.CompleteAsync();
        return new ()
        {
            Message = "Login successfully",
            Object = new LoginVM
            {
                Token = loginToken.Token,
                TokenExpiration = loginToken.Expiration,
                RefreshToken = employee.RefreshToken.Token!,
                RefreshTokenExpiration = (DateTime)employee.RefreshToken.Expiration!
            }
        };
    }
}