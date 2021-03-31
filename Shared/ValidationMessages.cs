using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Shared.Utils.Validators;

namespace Shared {
    public class ValidationMessages {
        private static Dictionary<string, string> Messages;

        static ValidationMessages() {
            Messages = new Dictionary<string, string>();

            Add(typeof(StringLengthAttribute), "O valor deve ter entre {2} e {1} caracteres.");
            Add(typeof(RequiredAttribute), "O campo é obrigatório.");
            Add(typeof(RangeAttribute), "O valor deve ser entre {2} e {1}.");
            Add(typeof(PhoneAttribute), "O telefone é inválido.");
            Add(typeof(CompareAttribute), "O valor atual não bate com o valor de {1}");
            Add(typeof(RequiredNonZeroAttribute), "O campo é obrigatório.");
            Add(typeof(RegularExpressionAttribute), "(Mensagem customizad)");
            Add(typeof(EmailAddressAttribute), "Email inválido");
        }

        private static void Add(MemberInfo type, string message) {
            Messages.Add(type.Name, message);
        }

        public static string Get(MemberInfo type) {
            Messages.TryGetValue(type.Name, out var temp);
            return temp;
        }

        public static string Get<TType>() {
            return Get(typeof(TType));
        }
    }
}