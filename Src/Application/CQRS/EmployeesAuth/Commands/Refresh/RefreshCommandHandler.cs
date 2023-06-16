using Application.Common.Extensions.Validation;
using Application.DTOs;
using Application.DTOs.EmployeesAuth;
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
using WorkingGood.Log;

namespace Application.CQRS.EmployeesAuth.Commands.Refresh;

public class RefreshCommandHandler : IRequestHandler<RefreshCommand, RefreshResponseDto>
{
    private readonly IWgLog<RefreshCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<RefreshCommand> _validator;
    private readonly ITokenProvider _tokenProvider;
    public RefreshCommandHandler(IWgLog<RefreshCommandHandler> logger, IValidator<RefreshCommand> validator, IUnitOfWork unitOfWork, ITokenProvider tokenProvider)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _tokenProvider = tokenProvider;
    }
    public async Task<RefreshResponseDto> Handle(RefreshCommand request, CancellationToken cancellationToken)
    {
        _logger.Info("Handling RefreshCommand");
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {                
            _logger.Warn(validationResult.Errors.GetErrorString());
            return new()
            {
                Errors = validationResult.Errors.GetErrorsStringList(),
                IsAuthorized = true
            };
        }
        Employee employee = await _unitOfWork
            .EmployeeRepository
            .GetByRefreshTokenAsync(request.RefreshDto.RefreshToken!);
        if (employee is null)
        {
            return new()
            {
                Errors = "Refresh token is invalid",
                IsAuthorized = false
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
            },
            IsAuthorized = true
        };
    }
}