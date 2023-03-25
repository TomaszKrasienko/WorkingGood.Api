using Application.DTOs;
using Application.Extensions.Validation;
using Application.ViewModels.Login;
using Domain.Interfaces;
using Domain.Models.Employee;
using Domain.ValueObjects;
using FluentValidation;
using Infrastructure.Common.ConfigModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Application.CQRS.EmployeesAuth.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, IActionResult>
{
    private readonly ILogger<LoginCommandHandler> _logger;
    private readonly JwtConfig _jwtConfig;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<LoginCommand> _validator;
    public LoginCommandHandler(ILogger<LoginCommandHandler> logger, JwtConfig jwtConfig, IUnitOfWork unitOfWork, IValidator<LoginCommand> validator)
    {
        _logger = logger;
        _jwtConfig = jwtConfig;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }
    public async Task<IActionResult> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return new BadRequestObjectResult(new BaseMessageDto
            {
                Errors = validationResult.Errors.GetErrorsStringList()
            });
        Employee employee = await _unitOfWork
            .EmployeeRepository
            .GetByEmail(request.CredentialsDto.Email!);
        LoginToken loginToken = employee.Login(
            request.CredentialsDto.Password!,
            _jwtConfig.TokenKey,
            _jwtConfig.Audience,
            _jwtConfig.Issuer
        );
        await _unitOfWork.CompleteAsync();
        return new OkObjectResult(new BaseMessageDto
        {
            Message = "Login successfully",
            Object = new LoginVM
            {
                Token = loginToken.Token,
                TokenExpiration = loginToken.Expiration,
                RefreshToken = employee.RefreshToken.Token!,
                RefreshTokenExpiration = (DateTime)employee.RefreshToken.Expiration!
            }
        });
    }
}