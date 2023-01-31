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

        public async Task<List<List<dynamic>>> CallProcedureMultipleDataSet(string procedureName, object parameters, TransactionWrapper tw)
        {
            var grid = await tw.Connection.QueryMultipleAsync(
                procedureName,
                parameters,
                transaction: tw.Transaction,
                commandType: System.Data.CommandType.StoredProcedure);

            var result = new List<List<dynamic>>();

            while (!grid.IsConsumed)
            {
                var tempResult = await grid.ReadAsync();

                result.Add(tempResult.ToList());
            }

            return result;
        }

        public async Task<T> CallProcedureSingleValueSetPOCO<T>(string procedure, object parameters, TransactionWrapper tw)
        {
            var singleResult = await tw.Connection.QueryFirstOrDefaultAsync<T>(
                procedure,
                parameters,
                transaction: tw.Transaction,
                commandType: CommandType.StoredProcedure);

            return singleResult;
        }
    }
}
