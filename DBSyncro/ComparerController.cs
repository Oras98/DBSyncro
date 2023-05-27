using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace DBSyncro
{
    public class ComparerController
    {
        IDatabaseConnection conn1, conn2;
        public ComparerController(IDatabaseConnection connString1, IDatabaseConnection connString2)
        {
            this.conn1 = connString1;
            this.conn2 = connString2;
        }

        public Dictionary<string, TableComparisonState>? DBDifferences;

        /// <summary>
        /// This calculates the differences and then get saved inside <DBDifferences> parameter
        /// </summary>
        /// <returns>Return a dictionary containing the databases tables defferences, where the key is the name of the table, 
        /// otherwise returns null if there are connection errors
        /// </returns>
        public Dictionary<string, TableComparisonState>? GetDifferences(bool CreateJSON)
        {
            // Connect to both databases

            try
            {
                conn1.Open();
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error opening connection 1: " + ex.Message);
                return null;
            }

            try
            {
                conn2.Open();
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error opening connection 2: " + ex.Message);
                return null;
            }

            DBDifferences = new Dictionary<string, TableComparisonState>();

            // Get list of tables in each database
            DataTable tables_cnn_1 = conn1.GetSchema("Tables");
            DataTable tables_cnn_2 = conn2.GetSchema("Tables");

            // Compare tables
            foreach (DataRow row1 in tables_cnn_1.Rows)
            {
                string tableName = (string)row1["TABLE_NAME"];
                var table_comparer = new TableComparisonState(tableName);
                DBDifferences.Add(tableName, table_comparer);

                DataRow? rows2 = tables_cnn_2.Rows.Cast<DataRow>().FirstOrDefault(t => (string)t["TABLE_NAME"] == tableName);

                if (!(table_comparer.ExistsInBothDB = rows2 is not null))
                    continue;

                #region compare columns
                DataTable columns_cnn_1 = conn1.GetSchema("Columns", new[] { null, null, tableName });
                DataTable columns_cnn_2 = conn2.GetSchema("Columns", new[] { null, null, tableName });

                foreach (DataRow column_cnn_1 in columns_cnn_1.Rows)
                {
                    string columnName = (string)column_cnn_1["COLUMN_NAME"];
                    DataRow? column_cnn_2 = columns_cnn_2.Rows.Cast<DataRow>().FirstOrDefault(c => (string)c["COLUMN_NAME"] == columnName);

                    if (column_cnn_2 is null)
                    {
                        table_comparer.MissingColumns ??= new();
                        table_comparer.MissingColumns.Add(columnName);
                        continue;
                    }

                    #region compare column schema

                    var column_differences = new ColumnDifferences(columnName);

                    // Retrieve the column schema information
                    var columnSchema = conn1.GetSchema("Columns", new[] { null, null, tableName, columnName });

                    foreach (DataColumn column in columnSchema.Columns)
                    {
                        if (!column_cnn_1[column.ColumnName].Equals(column_cnn_2[column.ColumnName]))
                        {
                            column_differences.differences ??= new();
                            column_differences.differences.Add($"Property: {column.ColumnName}\n Value cnn1: {column_cnn_1[column.ColumnName]};\n Value cnn2: {column_cnn_2[column.ColumnName]}");
                        }
                    }

                    if (column_differences.differences?.Count() > 0)
                    {
                        table_comparer.ColumnsDifferences ??= new();
                        table_comparer.ColumnsDifferences.Add(columnName, column_differences);
                    }

                    #endregion
                }
                #endregion
            }

            if (CreateJSON)
                CreateDifferencesJSON();

            return DBDifferences;
        }

        private void CreateDifferencesJSON()
        {
            // Serialize the object to JSON
            string json = JsonConvert.SerializeObject(DBDifferences);

            // Format the JSON string with indentation
            string formattedJson = JValue.Parse(json).ToString(Formatting.Indented);

            // Save the formatted JSON to a text file
            File.WriteAllText("differences.json", formattedJson);
        }
    }
}
