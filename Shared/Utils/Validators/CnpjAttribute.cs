using System;
using System.ComponentModel.DataAnnotations;

namespace Shared.Utils.Validators {
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public class CnpjAttribute : ValidationAttribute {
        public override string FormatErrorMessage(string name) {
            return ErrorMessage ?? "O cnpj é inválido!";
        }

        public override bool IsValid(object value) {
            var cnpj = value?.ToString();

            return cnpj == null || ValidateCnpj(cnpj);
        }

        public static bool ValidateCnpj(string cnpj) {
            var multiply1 = new[] {5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2};
            var multiply2 = new[] {6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2};

            //cnpj = cnpj.Trim().Replace(".", "").Replace("-", "").Replace("/", ""); fornecido...

            if (cnpj.Length != 14)
                return false;

            var tempCnpj = cnpj.Substring(0, 12);
            var sum      = 0;

            for (var i = 0; i < 12; i++)
                sum += int.Parse(tempCnpj[i].ToString()) * multiply1[i];

            var rest = (sum % 11);
            if (rest < 2)
                rest = 0;
            else
                rest = 11 - rest;

            var digit = rest.ToString();
            tempCnpj += digit;
            sum      =  0;
            for (var i = 0; i < 13; i++)
                sum += int.Parse(tempCnpj[i].ToString()) * multiply2[i];

            rest = (sum % 11);
            if (rest < 2)
                rest = 0;
            else
                rest = 11 - rest;

            digit += rest.ToString();

            return cnpj.EndsWith(digit);
        }
    }
}