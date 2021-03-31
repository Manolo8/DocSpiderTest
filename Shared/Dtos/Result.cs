using System.Collections.Generic;

namespace Shared.Dtos {
    public class Result<T> {
        public bool                    Fail    { get; set; }
        public IEnumerable<FieldError> Errors  { get; set; }
        public string                  Message { get; set; }
        public T                       Data    { get; set; }
    }
}