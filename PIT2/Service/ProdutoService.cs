using Microsoft.Data.SqlClient;
using PIT2.Factory;
using PIT2.Helpers.Extensions;
using PIT2.Models;

namespace PIT2.Service
{
    public class ProdutoService
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public ProdutoService(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<List<Produto>> GetProdutosAsync()
        {
            var conn = _connectionFactory.Create();

            conn.Open();

            var cmd = new SqlCommand("SELECT * FROM Cupcake WHERE Avaliable = 1", conn);

            var reader = await cmd.ExecuteReaderAsync();

            return reader.MapToList<Produto>();
        }

    }
}
