using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace MultipleReturnValues
{
    /// <summary>
    /// Returns the result of a command/query that outputs an entity
    /// </summary>
    [DebuggerDisplay("{Error == null ? Entity : Error,nq}")]
    public class CommandResult<TEntity, TError> : BaseErrorResult<TError>
        where TEntity : class
        where TError : struct, Enum
    {
        #region Members
        public TEntity Entity { get; set; }
        #endregion

        #region ctor
        public static CommandResult<TEntity, TError> ValidationError(IList<ValidationError> validationErrors, string errorMessage = "Validation Error")
        {
            return new CommandResult<TEntity, TError>() { Error = new ErrorResult<TError>(validationErrors, errorMessage) };
        }
        public static CommandResult<TEntity, TError> Fail(TError errorCode, string errorMessage = "Error")
        {
            return new CommandResult<TEntity, TError>() { Error = new ErrorResult<TError>(errorCode, errorMessage) };
        }
        public static CommandResult<TEntity, TError> Success(TEntity entity, string successMessage = "Success")
        {
            return new CommandResult<TEntity, TError>() { Entity = entity, SuccessMessage = successMessage };
        }
        #endregion

        #region IsSuccess
        public override bool IsSuccess
        {

            get
            {
                if (Error == null && Entity != null)
                    return true;
                if (Error != null && Entity == null)
                    return false;
                throw new NotImplementedException("When error is returned you cannot return any other value together");
            }
        }
        #endregion

        #region ToString
        public override string ToString()
        {
            if (this.IsSuccess)
                return $"Success: {this.SuccessMessage}: {this.Entity.ToString()}";
            else
                return base.ToString();
        }
        #endregion

        #region Implicit conversions for ValueTuple syntax
        public static implicit operator CommandResult<TEntity, TError>(ValueTuple<TEntity, TError?> tuple)
        {
            if (tuple.Item1 != null && tuple.Item2 == null)
                return Success(tuple.Item1);
            if (tuple.Item1 == null && tuple.Item2 != null)
                return Fail(tuple.Item2.Value);
            throw new NotImplementedException("When error is returned you cannot return any other value together");
        }
        #endregion

        #region Descontruct to ValueTuple
        public void Deconstruct(out TEntity entity, out ErrorResult<TError> error) => (entity, error) = (this.Entity, this.Error);
        #endregion


    }

}
