using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data.SqlClient;
using System.Data;

namespace CRD.Utils
{
    public class TransactionWrapper : IDisposable
    {
        public SqlConnection Connection { get; set; }
        public IDbTransaction Transaction { get; set; }
       
        public void Commit()
        {
            if (this.Transaction != null)
            {
                this.Transaction.Commit();
            }
        }

        public void Dispose()
        {
            if (Transaction != null)
            {
                Transaction.Dispose();
            }

            if (Connection.State == System.Data.ConnectionState.Open)
            {
                Connection.Close();
            }

            Connection.Dispose();

            GC.SuppressFinalize(this);
        }
    }
}

