using System;

namespace Shared.Exceptions {
    public class ValidationFieldException : Exception {
        public readonly string Field;
        public readonly string Error;

        public ValidationFieldException(string field, string error) {
            Field = field;
            Error = error;
        }
    }
}