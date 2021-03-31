using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Shared.Utils.Other {
    public static class ValidationUtils {
        public static void Error(this List<ValidationResult> results, string error, params string[] members) {
            results.Add(new ValidationResult(error, members));
        }
    }
}