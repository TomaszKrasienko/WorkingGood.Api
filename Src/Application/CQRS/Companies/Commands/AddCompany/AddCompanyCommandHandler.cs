using Application.Common.Extensions.Validation;
using Application.DTOs;
using Domain.Interfaces;
using Domain.Models.Company;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using WorkingGood.Log;

namespace Application.CQRS.Companies.Commands.AddCompany
{
    public class AddCompanyCommandHandler : IRequestHandler<AddCompanyCommand, BaseMessageDto>
    {
        private readonly IWgLog<AddCompanyCommandHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<AddCompanyCommand> _validator;
        public AddCompanyCommandHandler(IWgLog<AddCompanyCommandHandler> logger, IUnitOfWork unitOfWork, IValidator<AddCompanyCommand> validator)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _validator = validator;
        }
        public async Task<BaseMessageDto> Handle(AddCompanyCommand request, CancellationToken cancellationToken)
        {
            _logger.Info("Handling AddCompanyCommand");
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                _logger.Warn(validationResult.Errors.GetErrorString());
                return new ()
                {
                    Message = "Bad Request",
                    Errors = validationResult.Errors.GetErrorsStringList()
                };
            }
            Company company = new Company(request.CompanyDto!.Name!, request.CompanyDto.Logo!);
            await _unitOfWork.CompanyRepository.AddAsync(company);
            await _unitOfWork.CompleteAsync();
            return new()
            {
                Message = "Added company successfully",
                Object = company
            };
        }
    }
}

