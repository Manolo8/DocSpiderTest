using System;
using System.Collections.Generic;
using Shared.Dtos;

namespace Shared.Exceptions {
    public class ValidationException : Exception {
        public IEnumerable<FieldError> Errors;

        public ValidationException(IEnumerable<FieldError> errors) {
            Errors = errors;
        }
    }
}