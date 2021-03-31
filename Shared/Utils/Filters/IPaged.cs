namespace Shared.Utils.Filters {
    public interface IPaged {
        public int    Size       { get; set; }
        public int    Page       { get; set; }
        public string SortColumn { get; set; }
        public bool   SortAsc    { get; set; }
    }
}