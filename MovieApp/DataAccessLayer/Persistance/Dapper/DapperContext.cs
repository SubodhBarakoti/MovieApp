using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Persistance.Dapper
{
    public class DapperContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public DapperContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection")?? throw new InvalidOperationException("No Default Conneciton Found");    
        }
        public IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
