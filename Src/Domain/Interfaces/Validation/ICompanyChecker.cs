namespace Domain.Interfaces.Validation;

public interface ICompanyChecker
{
    bool IsCompanyExists(Guid companyId);
}