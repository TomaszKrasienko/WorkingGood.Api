namespace Application.DTOs.EmployeesAuth;

public class RefreshResponseDto : BaseMessageDto
{
    public bool IsAuthorized { get; set; }
}