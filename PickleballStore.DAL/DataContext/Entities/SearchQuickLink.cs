namespace PickleballStore.DAL.DataContext.Entities
{
    public class SearchQuickLink : TimeStample
    {
        public string Title { get; set; } = null!;
        public string Url { get; set; } = null!; 
        public bool IsActive { get; set; } = true;
        public int SearchSectionId { get; set; }
        public SearchSection SearchSection { get; set; } = null!;
    }
}
