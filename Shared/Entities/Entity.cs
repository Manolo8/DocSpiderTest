namespace Shared.Entities {
    public abstract class Entity {
        public long Id { get; set; }

        public bool IsNew() {
            return Id == 0;
        }
    }
}