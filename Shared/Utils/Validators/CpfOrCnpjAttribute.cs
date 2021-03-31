using System;
using System.ComponentModel.DataAnnotations;

namespace Shared.Utils.Validators {
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public class CpfOrCnpjAttribute : ValidationAttribute {
        public override string FormatErrorMessage(string name) {
            return ErrorMessage ?? "CPF ou CNPJ inválido!";
        }

        public override bool IsValid(object value) {
            var document = value?.ToString();

            return document == null
                || CpfAttribute.ValidateCpf(document)
                || CnpjAttribute.ValidateCnpj(document);
        }
    }
}