using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Shared.Utils.Other {
    public class ValidationMessages {
        // WTF!!!
        public static  string StringLength  = "O valor deve ter entre {1} e {0} caracteres";
        private static string _StringLength = "O valor deve ter entre {2} e {1} caracteres";
        public static  string Required      = "O campo é obrigatório";
        public static  string Range         = "O valor deve ser entre {1} e {0}";
        private static string _Range        = "O valor deve ser entre {2} e {1}";
        public static  string Phone         = "O telefone é inválido";
        public static  string Compare       = "O valor atual não bate com o valor de {0}";
        private static string _Compare      = "O valor atual não bate com o valor de {1}";
        public static  string MaxLength     = "O campo deve ter no máximo {0} caracteres";
        private static string _MaxLength    = "O campo deve ter no máximo {1} caracteres";
        public static  string MinLength     = "O campo deve ter no mínimo {0} caracteres";
        private static string _MinLength    = "O campo deve ter no mínimo {1} caracteres";
        public static  string EmailAddress  = "O endereço de e-mail não é válido";

        private Dictionary<string, string> Messages;

        public ValidationMessages() {
            Messages = new Dictionary<string, string>();

            Add(typeof(StringLengthAttribute), _StringLength);
            Add(typeof(RequiredAttribute), Required);
            Add(typeof(RangeAttribute), _Range);
            Add(typeof(PhoneAttribute), Phone);
            Add(typeof(CompareAttribute), _Compare);
            Add(typeof(MaxLengthAttribute), _MaxLength);
            Add(typeof(MinLengthAttribute), _MinLength);
            Add(typeof(EmailAddressAttribute), EmailAddress);
        }

        private void Add(MemberInfo type, string message) {
            Messages.Add(type.Name, message);
        }

        public string Get(MemberInfo type) {
            Messages.TryGetValue(type.Name, out var temp);
            return temp;
        }
    }
}