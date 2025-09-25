namespace PickleballStore.DAL.DataContext.Entities
{
    public class Language : TimeStample
    {
        public required string Name { get; set; }
        public bool IsDefault { get; set; } = false;
    }
}
