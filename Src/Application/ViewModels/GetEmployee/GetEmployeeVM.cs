namespace Application.ViewModels.GetEmployee;

public class GetEmployeeVM
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public string Initials { get; set; } = string.Empty;
}