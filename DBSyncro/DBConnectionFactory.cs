using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBSyncro
{
    internal class DBConnectionFactory
    {
        /// <summary>
        /// Create a connection of the given type
        /// </summary>
        /// <param name="database">Database type (es: firebird, mysql, ecc...)</param>
        /// <param name="connection_string">Connection string according to the database type</param>
        /// <returns>Return the database connection instance if the type is implemented, null otherwise</returns>
        internal IDatabaseConnection? GetInstance(string database, string connection_string)
        {
            if  (database == "firebird")
                return new FbDatabaseConnection(connection_string);

            if (database == "mysql")
                return new MySqlDatabaseConnection(connection_string);

            return null;
        }
    }
}
