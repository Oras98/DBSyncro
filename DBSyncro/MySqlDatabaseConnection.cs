using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using FirebirdSql.Data.FirebirdClient;

namespace DBSyncro
{
    public class MySqlDatabaseConnection : IDatabaseConnection
    {
        private MySqlConnection connection;

        public MySqlDatabaseConnection(string connectionString)
        {
            connection = new MySqlConnection(connectionString);
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
