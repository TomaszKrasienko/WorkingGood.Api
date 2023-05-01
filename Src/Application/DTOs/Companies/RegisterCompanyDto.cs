namespace Application.DTOs.Companies;

public class RegisterCompanyDto
{
    public string? CompanyName { get; set; }
    public string? EmployeeEmail { get; set; } 
    public string? EmployeeFirstName { get; set; }
    public string? EmployeeLastName { get; set; }
    public string? EmployeePassword { get; set; }
}