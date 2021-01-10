using System;
using System.Collections.Generic;
using System.Text;

namespace MultipleReturnValues
{
    /// <summary>
    /// Use this if after invoking a method and getting an ErrorResult{TError} you decide that this should be an exception 
    /// (an unhandleable situation where you want to bubble-up the error until some upper layer catches it, usually to abort the operation or the whole program)
    /// </summary>
    public class ErrorResultException<TError> : Exception
        where TError : struct, Enum
    {
        public ErrorResultException(ErrorResult<TError> errorResult)
        {
        }
    }
}
