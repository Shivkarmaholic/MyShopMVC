using Microsoft.Data.SqlClient;
using System.Data;

namespace MyShop.Context
{
    public class DbContext
    {
        private readonly string _connectionString;
        private readonly IConfiguration _configuration;
        public DbContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        public IDbConnection CreateConnection() => new SqlConnection(_connectionString);
    }
}

