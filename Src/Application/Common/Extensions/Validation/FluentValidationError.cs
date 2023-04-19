using System.Text;
using FluentValidation.Results;
namespace Application.Common.Extensions.Validation;
public static class FluentValidationError
{
    public static List<string> GetErrorsStringList(this List<ValidationFailure> errors)
    {
        List<string> errorsList = new(); 
        errors.ForEach(x => errorsList.Add(x.ErrorMessage));
        return errorsList;
    }
    public static string GetErrorString(this List<ValidationFailure> errors)
    {
        StringBuilder stringBuilder = new StringBuilder();
        errors.ForEach(x => stringBuilder.Append(x));
        return stringBuilder.ToString();
    }
}