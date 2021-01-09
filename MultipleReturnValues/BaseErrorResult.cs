using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace MultipleReturnValues
{
    /// <summary>
    /// BaseErrorResult is the result of any command/query which may or may not return errors.
    /// </summary>
    public abstract class BaseErrorResult<TError>
        where TError : struct, Enum
    {
        #region Members

        /// <summary>
        /// Optional return message (if success!)
        /// </summary>
        public string SuccessMessage { get; set; }

        /// <summary>
        /// Returns true if the command was executed without any errors. Basically this tests if <see cref="Error"/> object is not null.
        /// </summary>
        public virtual bool IsSuccess => Error != null;

        /// <summary>
        /// If any error happens, this will be non-null.
        /// This MAY or may not have a code in <see cref="ErrorResult{TError}.ErrorCode"/>.
        /// If <see cref="ErrorResult{TError}.ErrorCode"/> is null, you should check the <see cref="ErrorResult{TError}.ValidationErrors"/> to see why the command failed.
        /// </summary>
        public ErrorResult<TError> Error { get; set; }
        #endregion

        #region AddValidationError proxy methods
        /// <summary>
        /// Adds the specified validationError to the list of validation errors.
        /// If an <see cref="Error"/> (<see cref="ErrorResult{TError}"/>) is not defined (because there's no specific enum code for the error), a new one will be created.
        /// </summary>
        public void AddValidationError(string validationError)
        {
            if (Error == null)
                Error = new ErrorResult<TError>(new List<ValidationError>() { new ValidationError() { ErrorMessage = validationError } });
            else
                Error.AddValidationError(validationError);
        }

        /// <summary>
        /// Adds the specified validationError to the list of validation errors.
        /// If an <see cref="Error"/> (<see cref="ErrorResult{TError}"/>) is not defined (because there's no specific enum code for the error), a new one will be created.
        /// </summary>
        public void AddValidationError(ValidationError validationError)
        {
            if (Error == null)
                Error = new ErrorResult<TError>(new List<ValidationError>() { validationError });
            else
                Error.AddValidationError(validationError);
        }
        #endregion

        #region ToString
        public override string ToString()
        {
            if (this.IsSuccess)
                return $"Success: {this.SuccessMessage}";
            else
                return $"Error: {this.Error?.ErrorCode?.ToString()} - {this.Error?.ErrorMessage}";
        }
        #endregion
    }

}
