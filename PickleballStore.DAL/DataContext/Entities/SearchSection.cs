namespace PickleballStore.DAL.DataContext.Entities
{
    public class SearchSection : TimeStample
    {
        public string Title { get; set; } = "Search our site";
        public string Placeholder { get; set; } = "Search";
        public string ActionUrl { get; set; } = "/products/search";
        public string Icon { get; set; } = "icon-search";
        public ICollection<SearchQuickLink> QuickLinks { get; set; } = [];
    }
}
