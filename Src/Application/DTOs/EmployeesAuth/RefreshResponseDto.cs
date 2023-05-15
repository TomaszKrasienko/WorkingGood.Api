namespace Application.DTOs.EmployeesAuth;

public record RefreshResponseDto : BaseMessageDto
{
    public bool IsAuthorized { get; init; }
}