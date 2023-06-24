using Application.Common.Extensions.Validation;
using Application.DTOs;
using Domain.Interfaces;
using Domain.Models.Company;
using FluentValidation;
using MediatR;
using WorkingGood.Log;

namespace Application.CQRS.Companies.Commands.EditCompany;

public class EditCompanyCommandHandler : IRequestHandler<EditCompanyCommand, BaseMessageDto>
{
    private readonly IWgLog<EditCompanyCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<EditCompanyCommand> _validator;
    
    
    public EditCompanyCommandHandler(IWgLog<EditCompanyCommandHandler> logger, IUnitOfWork unitOfWork, IValidator<EditCompanyCommand> validator)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }
    
    public async Task<BaseMessageDto> Handle(EditCompanyCommand request, CancellationToken cancellationToken)
    {
        _logger.Info($"Handling {nameof(EditCompanyCommandHandler)}");
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            _logger.Warn(validationResult.Errors.GetErrorString());
            return new()
            {
                Message = "Validation is not valid",
                Object = validationResult.Errors.GetErrorsStringList()
            };
        }
        Company company = await _unitOfWork.CompanyRepository.GetByIdAsync((Guid)request.CompanyId!);
        company.EditName(request.CompanyDto.Name!);
        company.EditLogo(request.CompanyDto.Logo!);
        await _unitOfWork.CompleteAsync();
        return new()
        {
            Message = "Edited company",
            Object = company
        };
    }
}