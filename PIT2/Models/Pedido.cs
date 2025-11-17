namespace PIT2.Models
{
    public class Pedido
    {
        public Guid Id { get; set; }
        public decimal Total { get; set; }
        public decimal Desconto { get; set; }
        public string Email { get; set; }
        public string Customizacao { get; set; }
        public List<Produto> Itens { get; set; }
        public DateTime DataPedido { get; set; }
    }
}
