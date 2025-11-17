namespace PIT2.Models
{
    public class Produto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Photo { get; set; }
        public int Topping { get; set; }
        public int Flavor { get; set; }
        public int Quantity { get; set; }
        public bool Avaliable { get; set; }
    }
}
