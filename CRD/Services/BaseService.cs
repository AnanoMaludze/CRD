using CRD.Utils;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.Data.SqlClient;

namespace CRD.Services
{
    public class BaseService
    {
        protected readonly IConfiguration _configuration;
        protected readonly string connectionString;


        public BaseService(IConfiguration configuration)
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
        public TransactionWrapper GetTransactionWrapper(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            var connection = this.Connection;

            connection.Open();

            return new TransactionWrapper
            {
                Connection = connection,
                Transaction = connection.BeginTransaction(isolationLevel),
               
            };
        }
        public TransactionWrapper GetTransactionWrapperWithNoLock(IsolationLevel isolationLevel = IsolationLevel.ReadUncommitted)
        {
            var connection = this.Connection;

            connection.Open();

            return new TransactionWrapper
            {
                Connection = connection,
                Transaction = connection.BeginTransaction(isolationLevel),
               
            };
        }

        public TransactionWrapper GetTransactionWrapperWithoutTransaction(bool useMirror = false)
        {
            var connection = this.Connection;

            return new TransactionWrapper
            {
                Connection = connection,
                Transaction = null
            };
        }

    }
}
