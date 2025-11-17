namespace PIT2.Models
{
    public class Carrinho
    {
        public List<Produto> Itens { get; set; } = new List<Produto>();
        public decimal Total { get; set; }
        public string Coupon {  get; set; }
        public string Email { get; set; }
        public bool UsarCustomizacao { get; set; } = false;
        public string Customizacao { get; set; } = string.Empty;
        public Payment Payment { get; set; }
        public DeliveryAddress DeliveryAddress { get; set; }
    }
}
