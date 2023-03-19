using Application.DTOs;
using Application.Extensions.Validation;
using Application.ViewModels.Login;
using Domain.Interfaces;
using Domain.Models.Employee;
using Domain.ValueObjects;
using FluentValidation;
using Infrastructure.Common.ConfigModels;
using Infrastructure.Persistance.Repositories;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Application.CQRS.EmployeesAuth.Commands.Refresh;

public class RefreshCommandHandler : IRequestHandler<RefreshCommand, IActionResult>
{
    private readonly ILogger<RefreshCommandHandler> _logger;
    private readonly JwtConfig _jwtConfig;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<RefreshCommand> _validator;
    public RefreshCommandHandler(ILogger<RefreshCommandHandler> logger, JwtConfig jwtConfig, IUnitOfWork unitOfWork, IValidator<RefreshCommand> validator)
    {
        _logger = logger;
        _jwtConfig = jwtConfig;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }
    public async Task<IActionResult> Handle(RefreshCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return new BadRequestObjectResult(new BaseMessage
            {
                Errors = validationResult.Errors.GetErrorsStringList()
            });
        Employee employee = await _unitOfWork
            .EmployeeRepository
            .GetByRefreshToken(request.RefreshDto.RefreshToken);
        if (employee == null)
            return new BadRequestObjectResult(new BaseMessage
            {
                Message = "Refresh token is invalid"
            });
        LoginToken loginToken = employee.Refresh(_jwtConfig.TokenKey, _jwtConfig.Audience, _jwtConfig.Issuer);
        await _unitOfWork.CompleteAsync();
        return new OkObjectResult(new BaseMessage
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