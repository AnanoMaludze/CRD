using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data.SqlClient;
using System.Data;
using CRD.Utils;
using Dapper;

namespace CRD.Repository
{
    public class BaseRepository
    {

        protected readonly IConfiguration _configuration;
        protected readonly string connectionString;

        public BaseRepository(IConfiguration configuration)
        {
            this._configuration = configuration;
            connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        public SqlConnection Connection
        {
            get
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection");

                return new SqlConnection(connectionString);
            }
        }
       
    }
}
