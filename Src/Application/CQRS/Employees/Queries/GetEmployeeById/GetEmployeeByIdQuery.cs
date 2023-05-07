using Application.DTOs;
using MediatR;

namespace Application.CQRS.Employees.Queries;

public record GetEmployeeByIdQuery : IRequest<BaseMessageDto>
{
    public Guid Id { get; init; }
}