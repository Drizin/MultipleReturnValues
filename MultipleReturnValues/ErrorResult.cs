using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;

namespace MultipleReturnValues
{
    [DebuggerDisplay("{ErrorCode,nq} ({ErrorMessage})")]
    public class ErrorResult<TError>
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

        public void AddValidationError(string validationError)
        {
            if (_validationErrors == null)
                _validationErrors = new List<ValidationError>();
            _validationErrors.Add(new ValidationError() { ErrorMessage = validationError });
        }

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
        public ErrorResult(TError errorCode, string message = "Error")
        {
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

    }
}
