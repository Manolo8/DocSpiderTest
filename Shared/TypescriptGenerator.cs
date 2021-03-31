using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Shared {
    public class TypescriptGenerator {
        private string Target { get; set; }
        private string Src    { get; set; }

        public static void GenerateModels(Assembly assembly, string target) {
            var generator = new TypescriptGenerator {
                Src    = target.Substring(target.IndexOf("src", StringComparison.Ordinal) + 4),
                Target = target
            };

            generator.GenerateModels(
                assembly.GetTypes()
                        .Where(e => e.Namespace?.StartsWith("Core.Application.") ?? false)
                        .Where(e => (bool) e.Namespace?.Contains("Dtos"))
                        .ToList()
            );
        }

        private void GenerateModels(IEnumerable<Type> models) {
            foreach (var type in models)
                GenerateModel(type);
        }

        private void GenerateModel(Type type) {
            var name = ToTypescriptName(type);

            if (name.StartsWith("<>"))
                return;

            var content = type.IsEnum ? ToTypescriptEnum(type) : ToTypescriptInterface(type);

            var dir  = PathOf(type, false);
            var file = Path.Combine(dir, name);

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            if (File.Exists(file)) {
                var current = File.ReadAllText(file);

                if (current == content)
                    return;

                File.Delete(file);
            }

            var writer = File.CreateText(file);

            writer.Write(content);
            writer.Close();
        }

        private string PathOf(Type type, bool relative = true) {
            var nms           = type.Namespace ?? throw new Exception();
            var dataDtosIndex = nms.IndexOf(".Dtos", StringComparison.Ordinal);

            if (nms.EndsWith(".Dtos") || dataDtosIndex == -1)
                throw new Exception("Wrong organization at nms: " + nms);

            var lastIndex = nms.LastIndexOf('.', dataDtosIndex - 1) + 1;

            var model = nms.Substring(lastIndex, dataDtosIndex - lastIndex).ToLower();
            var place = nms.Substring(dataDtosIndex + 6).ToLower();

            var path = Path.Combine(relative ? Src : Target, model, place);

            if (relative)
                path = path.Replace('\\', '/');

            return path;
        }

        private string ToImport(Type type) {
            return $"import {{{type.Name}}} from '{PathOf(type)}/{ToTypescriptName(type, false)}';";
        }

        private static string ToTypescriptEnum(Type type) {
            var builder = new StringBuilder();

            var values = Enum.GetValues(type);

            builder.Append("export type ").Append(type.Name).Append(" =");

            foreach (var value in values)
                builder.Append(Environment.NewLine).Append("    |     '").Append(value).Append("'");

            builder.Append(";");

            return builder.ToString();
        }

        private string ToTypescriptInterface(Type type) {
            var builder = new StringBuilder();

            var fields = GetAllFields(type);

            var importBuilder = new StringBuilder();

            builder.Append("export interface ").Append(type.Name).Append(" {").Append(Environment.NewLine);

            foreach (var field in fields) {
                var underlying = Nullable.GetUnderlyingType(field.FieldType);

                string name;
                string typeName;

                if (underlying != null) {
                    name     = FixName(field.Name) + "?";
                    typeName = TypeName(underlying);
                }
                else {
                    name     = FixName(field.Name);
                    typeName = TypeName(field.FieldType);
                }

                if (typeName == "BoolFilter") {
                    importBuilder.Append("import {BoolFilter} from 'open-react-lib';").Append(Environment.NewLine);
                }
                else if (field.FieldType.Namespace?.Contains(".Dtos") ?? false) {
                    importBuilder.Append(ToImport(field.FieldType)).Append(Environment.NewLine);
                }

                builder.Append("    ").Append(name).Append(": ").Append(typeName).Append(';')
                       .Append(Environment.NewLine);
            }

            builder.Append("}");

            if (importBuilder.Length == 0)
                return builder.ToString();

            return importBuilder.Append(Environment.NewLine).Append(builder).ToString();
        }

        private static IEnumerable<FieldInfo> GetAllFields(Type type) {
            var list = new List<FieldInfo>();

            list.AddRange(type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance));

            if (type.BaseType != typeof(object)) {
                list.AddRange(GetAllFields(type.BaseType));
            }

            return list;
        }

        private static string TypeName(Type type) {
            if (type.IsArray)
                return TypeName(type.GetElementType()) + "[]";

            if (type.Namespace?.Contains(".Dtos") ?? false)
                return type.Name;

            if (type == typeof(Guid))
                return "string";

            if (type.Name == "IFormFile")
                return "File";

            switch (Type.GetTypeCode(type)) {
                case TypeCode.Boolean:
                    return "boolean";
                case TypeCode.Byte:
                case TypeCode.Char:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    return "number";
                case TypeCode.DateTime:
                case TypeCode.String:
                    return "string";
                case TypeCode.Empty:
                case TypeCode.Object:
                case TypeCode.DBNull:
                case TypeCode.Single:
                default:
                    return "any";
            }
        }

        private static string ToTypescriptName(MemberInfo type, bool ext = true) {
            var builder = new StringBuilder();

            foreach (var c in type.Name) {
                if (char.IsUpper(c)) {
                    if (builder.Length != 0)
                        builder.Append('-');
                    builder.Append(char.ToLower(c));
                }
                else {
                    builder.Append(c);
                }
            }

            if (ext)
                builder.Append(".ts");

            return builder.ToString();
        }

        private static string FixName(string value) {
            if (value.StartsWith('<'))
                value = value.Substring(1, value.IndexOf('>') - 1);

            if (!char.IsUpper(value[0]))
                return value;

            return char.ToLower(value[0]) + value.Substring(1);
        }
    }
}