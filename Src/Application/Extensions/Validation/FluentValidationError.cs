using FluentValidation.Results;

namespace Application.Extensions.Validation;

public static class FluentValidationError
{
    public static List<string> GetErrorsStringList(this List<ValidationFailure> errors)
    {
        List<string> errorsList = new(); 
        errors.ForEach(x => errorsList.Add(x.ErrorMessage));
        return errorsList;
    }
}