using Microsoft.Data.SqlClient;
using PIT2.Factory;

namespace PIT2.Service
{
    public class CupomService
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public CupomService(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<decimal> GetDesconto(string cupom)
        {
            var conn = _connectionFactory.Create();

            conn.Open();

            var cmd = new SqlCommand($"SELECT Desconto FROM Cupom WHERE Avaliable = 1 AND Cupom = '{cupom.ToUpper()}'", conn);

            var reader = await cmd.ExecuteReaderAsync();

            if (reader.Read())
            {
                return reader.GetDecimal(0);
            }

            return 0;
        }
    }
}
