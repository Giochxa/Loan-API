using FluentValidation.Results;
using System.Collections.Generic;
using System.Linq;

namespace Loan_API.Helpers
{
    public class ValidationErrorParse 
    {
        public static List<string> GetErrors(ValidationResult result)
        {
            var fullErrorList = result.Errors.ToList<ValidationFailure>();
            var errorMessageList = new List<string>();
            foreach (var i in fullErrorList)
            {
                errorMessageList.Add(i.ErrorMessage);
            }
            return errorMessageList;
        }
    }
}
