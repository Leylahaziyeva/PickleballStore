using System.ComponentModel.DataAnnotations.Schema;

namespace PickleballStore.DAL.DataContext.Entities
{
    public class OrderItem : TimeStample
    {
        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;

        public int ProductId { get; set; }
        public Product Product { get; set; } = null!; 

        public string ProductName { get; set; } = null!;
        public string? Color { get; set; }
        public string? ImageUrl { get; set; }

        public int Quantity { get; set; }
        public decimal Price { get; set; }

        [NotMapped]
        public decimal Subtotal => Quantity * Price;
    }
}
