using System;

namespace Shared.Exceptions {
    public class NotAllowedException : Exception {
        public NotAllowedException(string message) : base(message) { }
    }
}