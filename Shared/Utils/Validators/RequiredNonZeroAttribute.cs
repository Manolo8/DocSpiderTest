using System;
using System.ComponentModel.DataAnnotations;

namespace Shared.Utils.Validators {
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public class RequiredNonZeroAttribute : ValidationAttribute {
        public override bool IsValid(object value) {
            if (value == null)
                return false;

            return (long) value != 0;
        }
    }
}