using Application.Common.Extensions.Validation;
using Application.DTOs;
using Application.ViewModels.Login;
using Domain.Interfaces;
using Domain.Models.Employee;
using Domain.Services;
using Domain.ValueObjects;
using FluentValidation;
using Infrastructure.Common.ConfigModels;
using Infrastructure.Persistance.Repositories;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Application.CQRS.EmployeesAuth.Commands.Refresh;

public class RefreshCommandHandler : IRequestHandler<RefreshCommand, BaseMessageDto>
{
    private readonly ILogger<RefreshCommandHandler> _logger;
    private readonly JwtConfig _jwtConfig;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<RefreshCommand> _validator;
    private readonly ITokenProvider _tokenProvider;
    public RefreshCommandHandler(ILogger<RefreshCommandHandler> logger, IValidator<RefreshCommand> validator, JwtConfig jwtConfig, IUnitOfWork unitOfWork, ITokenProvider tokenProvider)
    {
        _logger = logger;
        _jwtConfig = jwtConfig;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _tokenProvider = tokenProvider;
    }
    public async Task<BaseMessageDto> Handle(RefreshCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling RefreshCommand");
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
            .GetByRefreshTokenAsync(request.RefreshDto.RefreshToken!);
        if (employee == null)
        {
            _logger.LogWarning("Employee is null");
            return new()
            {
                Errors = "Refresh token is invalid"
            };
        }
        LoginToken loginToken = employee.Refresh(_tokenProvider);
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