using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace MultipleReturnValues
{
    /// <summary>
    /// Base class for classes that may hold an ErrorResult{TError} if an error occurs.
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("{ToString()}")]
    public abstract class SuccessOrErrorBase<TError> : IValidationErrorResult
        where TError : struct, Enum
    {
        #region Members

        /// <summary>
        /// Optional return message (if success!)
        /// </summary>
        public string SuccessMessage { get; set; }

        public string ErrorMessage => ErrorResult?.ErrorMessage;

        /// <summary>
        /// Returns true if the command was executed without any errors. Basically this tests if <see cref="ErrorResult"/> object is not null.
        /// </summary>
        public virtual bool IsSuccess => ErrorResult != null;

        /// <summary>
        /// If any error happens, this will be non-null.
        /// This MAY or may not have a code in <see cref="ErrorResult{TError}.ErrorCode"/>.
        /// If <see cref="ErrorResult{TError}.ErrorCode"/> is null, you should check the <see cref="ErrorResult{TError}.ValidationErrors"/> to see why the command failed.
        /// </summary>
        public ErrorResult<TError> ErrorResult { get; set; }
        #endregion

        #region IValidatableCommandResult - AddValidationError proxy methods
        /// <summary>
        /// Adds the specified validationError to the list of validation errors.
        /// If an <see cref="ErrorResult"/> (<see cref="ErrorResult{TError}"/>) is not defined (because there's no specific enum code for the error), a new one will be created.
        /// </summary>
        public void AddValidationError(string validationError)
        {
            if (ErrorResult == null)
                ErrorResult = new ErrorResult<TError>(new List<ValidationError>() { new ValidationError() { ErrorMessage = validationError } });
            else
                ErrorResult.AddValidationError(validationError);
        }

        /// <summary>
        /// Adds the specified validationError to the list of validation errors.
        /// If an <see cref="ErrorResult"/> (<see cref="ErrorResult{TError}"/>) is not defined (because there's no specific enum code for the error), a new one will be created.
        /// </summary>
        public void AddValidationError(ValidationError validationError)
        {
            if (ErrorResult == null)
                ErrorResult = new ErrorResult<TError>(new List<ValidationError>() { validationError });
            else
                ErrorResult.AddValidationError(validationError);
        }
        #endregion

        #region ToString
        public override string ToString()
        {
            if (this.IsSuccess)
                return "Success" + ((SuccessMessage == null || SuccessMessage == "Success") ? "" : $@" (""{this.SuccessMessage}"")");
            else
                return this.ErrorResult.ToString();
        }
        #endregion
    }

}
