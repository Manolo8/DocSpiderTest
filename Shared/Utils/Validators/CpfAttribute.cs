using System;
using System.ComponentModel.DataAnnotations;

namespace Shared.Utils.Validators {
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public class CpfAttribute : ValidationAttribute {
        public override string FormatErrorMessage(string name) {
            return ErrorMessage ?? "O cpf é inválido!";
        }

        public override bool IsValid(object value) {
            var cpf = value?.ToString();

            return cpf == null || ValidateCpf(cpf);
        }

        public static bool ValidateCpf(string cpf) {
            var multiply1 = new[] {10, 9, 8, 7, 6, 5, 4, 3, 2};
            var multiply2 = new[] {11, 10, 9, 8, 7, 6, 5, 4, 3, 2};

            // cpf = cpf.Trim().Replace(".", "").Replace("-", ""); o servidor já vai fornecer isso corretamente

            if (cpf.Length != 11)
                return false;

            for (var j = 0; j < 10; j++)
                if (j.ToString().PadLeft(11, char.Parse(j.ToString())) == cpf)
                    return false;

            var tempCpf = cpf.Substring(0, 9);
            var sum     = 0;

            for (var i = 0; i < 9; i++)
                sum += int.Parse(tempCpf[i].ToString()) * multiply1[i];

            var rest = sum % 11;
            if (rest < 2)
                rest = 0;
            else
                rest = 11 - rest;

            var digit = rest.ToString();
            tempCpf += digit;
            sum     =  0;

            for (var i = 0; i < 10; i++)
                sum += int.Parse(tempCpf[i].ToString()) * multiply2[i];

            rest = sum % 11;
            if (rest < 2)
                rest = 0;
            else
                rest = 11 - rest;

            digit += rest.ToString();

            return cpf.EndsWith(digit);
        }
    }
}