using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBSyncro
{
    public interface IDatabaseConnection
    {
        public void Open();
        public void Close();

        public DataTable GetSchema();
        public DataTable GetSchema(string collectionName);
        public DataTable GetSchema(string collectionName, string[] restrictionValues);
    }
}
