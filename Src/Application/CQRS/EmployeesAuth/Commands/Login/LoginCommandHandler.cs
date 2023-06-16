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
using WorkingGood.Log;

namespace Application.CQRS.EmployeesAuth.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, BaseMessageDto>
{
    private readonly IWgLog<LoginCommandHandler> _logger;
    private readonly IValidator<LoginCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenProvider _tokenProvider;
    public LoginCommandHandler(IWgLog<LoginCommandHandler> logger, IValidator<LoginCommand> validator, IUnitOfWork unitOfWork, ITokenProvider tokenProvider)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _tokenProvider = tokenProvider;
    }
    public async Task<BaseMessageDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {            
        _logger.Info("Handling AddCompanyCommand");
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {                
            _logger.Info(validationResult.Errors.GetErrorString());
            return new()
            {
                Errors = validationResult.Errors.GetErrorsStringList()
            };
        }
        Employee employee = await _unitOfWork
            .EmployeeRepository
            .GetByEmailAsync(request.CredentialsDto.Email!);
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