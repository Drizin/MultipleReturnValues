using System;
using System.Collections.Generic;
using System.Text;

namespace MultipleReturnValues
{
    public interface IValidationErrorResult
    {
        void AddValidationError(string error);
        void AddValidationError(ValidationError error);
    }
}
