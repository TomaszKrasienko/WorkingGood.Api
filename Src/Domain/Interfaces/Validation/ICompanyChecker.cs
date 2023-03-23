namespace Domain.Interfaces.Validation;

public interface ICompanyChecker
{
    bool IsCompanyExists(Guid companyId);
    bool IsCompanyExists(string name);
}