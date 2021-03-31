using System;

namespace Shared.Exceptions {
    public class ValidationGenericException : Exception {
        public int Answer { get; }

        public ValidationGenericException(string message, int answer = 0) : base(message) {
            Answer = answer;
        }
    }
}