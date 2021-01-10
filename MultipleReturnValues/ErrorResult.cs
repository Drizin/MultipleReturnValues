using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace MultipleReturnValues
{
    /// <summary>
    /// This can be used as return by methods which do not return anything else 
    /// but may return some expected errors 
    /// (instead of throwing Exceptions which should be reserved for unexpected/unhandleable exceptions where you want to bubble up the error).
    /// Those methods should return null if no error happen.
    /// This is also used in classes like SuccessOrError (for methods that may return an ErrorResult{TError} OR ELSE (if success) will return some other result)
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("{ToString()}")]
    public class ErrorResult<TError> : IValidationErrorResult
        where TError : struct, Enum
    {
        #region Members
        /// <summary>
        /// This MAY or may not be defined (even if an error happened!).
        /// If this is null, you should check the <see cref="ValidationErrors"/> to see why the command failed.
        /// </summary>
        public TError? ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        protected IList<ValidationError> _validationErrors;

        /// <summary>
        /// List of Validation Errors that happened in this command.
        /// If anything is returned here you probably will want to bind the error back to the UI in the appropriate control which caused the error.
        /// </summary>
        public IReadOnlyCollection<ValidationError> ValidationErrors => _validationErrors == null ? null : new ReadOnlyCollection<ValidationError>(_validationErrors);
        #endregion

        #region IValidatableCommandResult
        /// <summary>
        /// Adds the specified validationError to the list of validation errors.
        /// </summary>
        public void AddValidationError(string validationError)
        {
            if (_validationErrors == null)
                _validationErrors = new List<ValidationError>();
            _validationErrors.Add(new ValidationError() { ErrorMessage = validationError });
        }

        /// <summary>
        /// Adds the specified validationError to the list of validation errors.
        /// </summary>
        public void AddValidationError(ValidationError validationError)
        {
            if (_validationErrors == null)
                _validationErrors = new List<ValidationError>();
            _validationErrors.Add(validationError);
        }
        #endregion

        #region ctor
        /// <summary>
        /// ErrorResult where error code is a well-defined enum
        /// </summary>
        public ErrorResult(TError errorCode, string message = null)
        {
            if (message == null)
                message = GetEnumDescription(errorCode) ?? "Error";
            ErrorCode = errorCode;
            ErrorMessage = message;
        }

        /// <summary>
        /// ErrorResult where error code is a well-defined enum
        /// </summary>
        public ErrorResult(IList<ValidationError> validationErrors, string errorMessage = "Validation Error")
        {
            _validationErrors = validationErrors;
            ErrorMessage = errorMessage;
        }


        #endregion

        #region Equals / GetHashCode (including Conversions to TError enum)
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            ErrorResult<TError> other = obj as ErrorResult<TError>;
            if (other != null)
                return Equals(other);
            if (obj.GetType() == typeof(TError))
                return Equals((TError)obj);
            return false;
        }
        public bool Equals(ErrorResult<TError> other)
        {
            if (!ErrorCode.Equals(other.ErrorCode))
                return false;
            if (ErrorMessage != other.ErrorMessage)
                return false;
            if (_validationErrors != null && other._validationErrors == null)
                return false;
            if (_validationErrors == null && other._validationErrors != null)
                return false;
            if (_validationErrors != null && other._validationErrors != null && _validationErrors.Equals(other._validationErrors))
                return false;
            return true;
        }
        public bool Equals(TError other)
        {
            return ErrorCode != null && ErrorCode.Value.Equals(other);
        }
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + (ErrorCode == null ? 0 : ErrorCode.GetHashCode());
                hash = hash * 23 + (ErrorMessage == null ? 0 : ErrorMessage.GetHashCode());
                hash = hash * 23 + (_validationErrors == null ? 0 : _validationErrors.GetHashCode());
                return hash;
            }
        }

        public static bool operator ==(ErrorResult<TError> left, ErrorResult<TError> right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ErrorResult<TError> left, ErrorResult<TError> right)
        {
            return !Equals(left, right);
        }
        public static bool operator ==(TError left, ErrorResult<TError> right)
        {
            return Equals(left, right);
        }
        public static bool operator ==(ErrorResult<TError> left, TError right)
        {
            //return left.ErrorCode != null && left.ErrorCode.Value.Equals(right);
            return Equals(left, right);
        }

        public static bool operator !=(TError left, ErrorResult<TError> right)
        {
            return !Equals(left, right);
        }
        public static bool operator !=(ErrorResult<TError> left, TError right)
        {
            //return !(left == right);
            return !Equals(left, right);
        }
        // 
        //public static implicit operator TError?(ErrorResult<TError> error)
        //{
        //    return error?.ErrorCode;
        //}
        #endregion Equals / GetHashCode (including Conversions to TError enum)

        #region ThrowException()
        /// <summary>
        /// Use this if you received an ErrorResult{TError} which you can't handle and want to bubble-up as an exception 
        /// until some upper layer catches it, usually to abort the operation or the whole program
        /// </summary>
        public void ThrowException()
        {
            throw new ErrorResultException<TError>(this);
        }
        #endregion

        #region Implicit conversions from TError
        public static implicit operator ErrorResult<TError>(TError? error)
        {
            //if you do "return (null)", it will be converted to a Success(). "return TError.SomeValue" will be converted to a Fail()
            if (error == null)
                return null;
            else
                return new ErrorResult<TError>(error.Value);
        }
        #endregion

        #region GetEnumDescription
        private string GetEnumDescription(Enum value)
        {
            // Get the Description attribute value for the enum value
            FieldInfo fi = value.GetType().GetField(value.ToString());
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes.Length > 0)
                return attributes[0].Description;
            //else
            //    return value.ToString();
            return null;
        }
        #endregion

        #region ToString()
        public override string ToString()
        {
            if (ErrorCode == null)
                return "Error" 
                    + (ErrorMessage != null ? $@" (""{ErrorMessage}"")" : "");

            string description = GetEnumDescription(ErrorCode);
            return "Error: " + ErrorCode.ToString() 
                + (description == null ? "" : $@" (""{description}"")")
                + ((ErrorMessage != null && ErrorMessage != description && ErrorMessage != "Error") ? $@" (""{ErrorMessage}"")" : "");
        }
        #endregion

    }
}
