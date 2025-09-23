namespace PickleballStore.DAL.DataContext.Entities
{
    public class SearchQuickLink : TimeStample
    {
        public string Title { get; set; } = null!; 
        public string Url { get; set; } = null!; 
        public int SearchInfoId { get; set; }
        public SearchInfo SearchInfo { get; set; } = null!;
    }
}
