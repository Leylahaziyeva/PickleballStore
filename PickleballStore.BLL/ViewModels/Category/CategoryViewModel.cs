namespace PickleballStore.BLL.ViewModels.Category
{
    public class CategoryViewModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? ImageName { get; set; }
        public bool IsDeleted { get; set; }
    }
}
