using Microsoft.Data.SqlClient;
using PIT2.Factory;
using PIT2.Helpers.Extensions;
using PIT2.Models;
using System.Globalization;
using System.Net;
using System.Net.Mail;

namespace PIT2.Service
{
    public class PedidoService
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly CupomService _cupomService;

        public PedidoService(IDbConnectionFactory connectionFactory, CupomService cupomService)
        {
            _cupomService = cupomService;
            _connectionFactory = connectionFactory;
        }

        public bool FinalizarPedido(Carrinho carrinho, out Pedido pedido)
        {
            pedido = null;

            if (!MontaPedido(carrinho, ref pedido))
            {
                return false;
            }

            if (!SalvaPedido(pedido))
            {
                return false;
            }

            if (!SalvaItensPedido(pedido))
            {
                return false;
            }

            /// Logica para processar pagamento via API sera adicionada aqui

            /// Logica para envio de nota fiscal por email 
            EnviarNotaFiscal(pedido);

            return true;
        }

        public async Task<Pedido> GetPedidoById(Guid id)
        {
            var conn = _connectionFactory.Create();
            conn.Open();
            var cmdStr = $"SELECT * FROM Pedido WHERE Id = '{id}'";

            var cmd = new SqlCommand(cmdStr, conn);
            var reader = await cmd.ExecuteReaderAsync();

            return reader.MapToList<Pedido>()[0];
        }

        private bool SalvaPedido(Pedido pedido)
        {
            var conn = _connectionFactory.Create();
            conn.Open();
            var cmdStr = $"INSERT INTO Pedido (Id, Total, Desconto, DataPedido, Email, Customizacao) " +
                $"         VALUES ('{pedido.Id}'," +
                $"          {pedido.Total.ToString(CultureInfo.InvariantCulture)}," +
                $"          {pedido.Desconto.ToString(CultureInfo.InvariantCulture)}," +
                $"          '{pedido.DataPedido:yyyy-MM-dd HH:mm:ss}'," +
                $"          '{pedido.Email}'," +
                $"          '{pedido.Customizacao}')";

            var cmd = new SqlCommand(cmdStr, conn);
            cmd.ExecuteNonQuery();
            return true;
        }
        private bool SalvaItensPedido(Pedido pedido)
        {
            var conn = _connectionFactory.Create();
            conn.Open();
            var cmdStr = $"INSERT INTO PedidoItens (IdPedido, IdItem) VALUES";

            foreach (var item in pedido.Itens)
            {
                cmdStr += $" ('{pedido.Id}', '{item.Id}'),";
            }

            cmdStr = cmdStr.TrimEnd(',');

            var cmd = new SqlCommand(cmdStr, conn);
            var result = cmd.ExecuteNonQuery();

            return true;
        }
        private bool MontaPedido(Carrinho carrinho, ref Pedido pedido)
        {
            if (carrinho == null || carrinho.Itens == null || !carrinho.Itens.Any())
            {
                pedido = null;
                return false;
            }

            var descontoPerc = _cupomService.GetDesconto(carrinho.Coupon).Result;

            var desconto = (carrinho.Itens.Sum(item => item.Price * item.Quantity) * descontoPerc) / 100;

            var totalCalculado = carrinho.Itens.Sum(item => item.Price * item.Quantity) - desconto + 15;

            pedido = new Pedido
            {
                Id = Guid.NewGuid(),
                Total = totalCalculado,
                Email = carrinho.Email,
                Itens = carrinho.Itens,
                Desconto = desconto,
                DataPedido = DateTime.UtcNow
            };

            if (carrinho.UsarCustomizacao)
            {
                pedido.Customizacao = carrinho.Customizacao;
                pedido.Total += 20;
            }

            return true;
        }

        public void EnviarNotaFiscal(Pedido pedido)
        {
            try
            {
                var smtp = new SmtpClient("smtp.gmail.com", 587)
                {
                    EnableSsl = true,
                    Credentials = new NetworkCredential(
                        "cupcakestore@gmail.com",
                        "12345"
                    )
                };

                var mail = new MailMessage();
                mail.From = new MailAddress("cupcakestore@gmail.com", "Cupcake Store");
                mail.To.Add(pedido.Email);
                mail.Subject = $"Nota Fiscal do Pedido #{pedido.Id}";
                mail.Body = GerarCorpoNotaFiscal(pedido);
                mail.IsBodyHtml = false;

                smtp.Send(mail);
            }
            catch (Exception)
            {

            }

        }

        private string GerarCorpoNotaFiscal(Pedido pedido)
        {
            var sb = new System.Text.StringBuilder();

            sb.AppendLine("====================================");
            sb.AppendLine("           CUPCAKE STORE");
            sb.AppendLine("              NOTA FISCAL");
            sb.AppendLine("====================================");
            sb.AppendLine($"Pedido ID: {pedido.Id}");
            sb.AppendLine($"Data: {pedido.DataPedido:dd/MM/yyyy HH:mm}");
            sb.AppendLine("====================================");
            sb.AppendLine("ITEMS");
            sb.AppendLine("------------------------------------");

            foreach (var item in pedido.Itens)
            {
                sb.AppendLine($"{item.Name}");
                sb.AppendLine($"  Quantidade : {item.Quantity}");
                sb.AppendLine($"  Preço unit.: R$ {item.Price}");
                sb.AppendLine($"  Subtotal   : R$ {item.Price * item.Quantity}");
                sb.AppendLine("------------------------------------");
            }

            sb.AppendLine($"TOTAL: R$ {pedido.Total}");
            sb.AppendLine($"DESCONTO: R$ {pedido.Desconto}");
            sb.AppendLine("====================================");
            sb.AppendLine();
            sb.AppendLine("Rastreamento do Pedido:");
            sb.AppendLine("https://rastreamento.cupcakestore.fake/track/" + pedido.Id);
            sb.AppendLine();
            sb.AppendLine("Obrigado por comprar conosco!");
            sb.AppendLine("Cupcake Store");

            return sb.ToString();
        }
    }
}
