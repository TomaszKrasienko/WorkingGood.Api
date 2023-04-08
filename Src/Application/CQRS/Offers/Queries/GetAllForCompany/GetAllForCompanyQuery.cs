using Application.DTOs;
using MediatR;

namespace Application.CQRS.Offers.Queries.GetAllForCompany;

public class GetAllForCompanyQuery : IRequest<BaseMessageDto>
{
    public Guid EmployeeId { get; set; }
}