using System;
using Application.DTOs;
using Application.Extensions.Validation;
using Domain.Interfaces;
using Domain.Models.Company;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Application.CQRS.Companies.Commands
{
    public class AddCompanyCommandHandler : IRequestHandler<AddCompanyCommand, BaseMessageDto>
    {
        private readonly ILogger<AddCompanyCommandHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<AddCompanyCommand> _validator;
        public AddCompanyCommandHandler(ILogger<AddCompanyCommandHandler> logger, IUnitOfWork unitOfWork, IValidator<AddCompanyCommand> validator)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _validator = validator;
        }
        public async Task<BaseMessageDto> Handle(AddCompanyCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning(validationResult.Errors.GetErrorString());
                return new ()
                {
                    Errors = validationResult.Errors
                };
            }
            Company company = new Company(request.CompanyDto!.Name!);
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

