using System;

namespace Shared.Exceptions {
    public class FieldNotUpdatable : Exception {
        public string Field { get; set; }

        public FieldNotUpdatable(string field) : base("field_not_updatable") {
            Field = field;
        }
    }
}