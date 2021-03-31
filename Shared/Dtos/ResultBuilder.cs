using System.Collections.Generic;

namespace Shared.Dtos {
    public static class ResultBuilder {
        public static Result<T> Success<T>(T data, string message = null) {
            return new Result<T> {
                Message = message,
                Data    = data
            };
        }

        public static Result<T> Error<T>(string message = null, IEnumerable<FieldError> errors = null) {
            return new Result<T> {
                Message = message,
                Fail    = true,
                Errors  = errors
            };
        }
    }
}