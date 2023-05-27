using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using FirebirdSql.Data.FirebirdClient;
using System.Diagnostics.CodeAnalysis;

namespace DBSyncro
{
    public class FbDatabaseConnection : IDatabaseConnection
    {
        private FbConnection connection;

        public FbDatabaseConnection(string connectionString)
        {
            connection = new FbConnection(connectionString);
        }

        public void Open()
        {
            connection.Open();
        }

        public void Close()
        {
            connection.Close();
        }

        public DataTable GetSchema() => connection.GetSchema();

        public DataTable GetSchema(string collectionName) => connection.GetSchema(collectionName);

        public DataTable GetSchema(string collectionName, string[] restrictionValues) => connection.GetSchema(collectionName, restrictionValues);

        public void Dispose()
        {
            connection.Dispose();
        }
    }
}
