using System;
using Domain.Interfaces;
using Domain.Models.Company;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.CQRS.Companies.Commands
{
    public class AddCompanyCommandHandler : IRequestHandler<AddCompanyCommand, IActionResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<AddCompanyCommand> _validator;
        public AddCompanyCommandHandler(IUnitOfWork unitOfWork, IValidator<AddCompanyCommand> validator)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
        }
        public async Task<IActionResult> Handle(AddCompanyCommand request, CancellationToken cancellationToken)
        {
            var validationResult = _validator.Validate(request);
            if (!validationResult.IsValid)
                return new BadRequestObjectResult(validationResult.Errors);
            Company company = new Company(request.CompanyDto!.Name!);
            await _unitOfWork.CompanyRepository.AddAsync(company);
            await _unitOfWork.CompleteAsync();
            return new OkObjectResult(company);
        }
    }
}

