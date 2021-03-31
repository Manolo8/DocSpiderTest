using System.Collections.Generic;

namespace Shared.Dtos
{
    public class FieldError
    {
        public string Field { get; set; }
        public IEnumerable<string> Errors { get; set; }

        public static IEnumerable<FieldError> Of(string field, string error)
        {
            return new[] {new FieldError {Field = field, Errors = new[] {error}}};
        }
    }
}