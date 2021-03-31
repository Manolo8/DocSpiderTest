namespace Shared.Utils.Selectors {
    public class SelectorLevel : Selector {
        public long? ParentId { get; set; }
        public bool  Children { get; set; }
    }
}